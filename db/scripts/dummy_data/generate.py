from InquirerPy import prompt
from faker import Faker
from random import Random
from uuid import uuid4

class Row:
    data_types = [
        "full_name", "email", "phone_number", "username", "password", "description", "address", "district", "city",
        "postal_code", "country", "datetime", "website_link", "image_url", "company_name", "product_name",
        "product_category", "product_size", "number", "random_number", "color", "permission_level", "uuid", "shipment_status"
    ]
    starting_index = {}
    faker = Faker()

    generators = {
        "full_name": faker.name,
        "email": faker.company_email,
        "phone_number": faker.phone_number,
        "username": faker.user_name,
        "password": faker.password,
        "description": lambda: f"{faker.text()}{faker.text()}",
        "address": faker.street_address,
        "district": lambda: Random().choice([
            "Ba Đình", "Hoàn Kiếm", "Hai Bà Trưng", "Đống Đa", "Tây Hồ", "Cầu Giấy", "Thanh Xuân", "Hoàng Mai",
            "Long Biên", "Bắc Từ Liêm", "Nam Từ Liêm", "Thanh Trì", "Gia Lâm", "Đông Anh", "Sóc Sơn", "Mê Linh",
            "Hà Đông", "Sơn Tây", "Ba Vì", "Phúc Thọ", "Đan Phượng", "Hoài Đức", "Quốc Oai", "Thạch Thất",
            "Chương Mỹ", "Thanh Oai", "Thường Tín", "Phú Xuyên", "Mỹ Đức", "Mê Linh"
        ]),
        "city": faker.city,
        "postal_code": faker.postalcode,
        "country": faker.country,
        "datetime": lambda: faker.date_time().strftime('%Y-%m-%d %H:%M:%S'),
        "website_link": faker.url,
        "image_url": lambda: f"{faker.url()}/{faker.word()}-image.png",
        "company_name": faker.company,
        "product_name": faker.company,
        "product_category": lambda: Random().choice([
            "Electronics", "Clothing", "Home and Kitchen", "Books", "Toys", "Beauty and Personal Care", "Sports and Outdoors",
            "Health and Wellness", "Automotive", "Tools and Home Improvement"
        ]),
        "product_size": lambda: Random().choice(["XS", "S", "M", "L", "XL", "XXL", "One Size"]),
        "number": lambda self, current_column: Row.starting_index[self.columns[current_column]] + self.index,
        "random_number": lambda self, current_column: Random().randint(*Row.starting_index[self.columns[current_column]]),
        "color": faker.color_name,
        "permission_level": lambda: Random().randint(0, 4),
        "uuid": lambda: str(uuid4()),
        "shipment_status": lambda: Random().choice(['Completed', 'Processing'])
    }

    def __init__(self, columns, index):
        self.columns = columns
        self.index = index
        self.values = []

    def generate(self):
        self.values = [
            Row.generators[col](self, i) if col in ["number", "random_number"] else Row.generators[col]()
            for i, col in enumerate(self.columns)
        ]

def get_column_details(column_count):
    column_names = [input(f"Enter the name of column {i + 1}: ") for i in range(column_count)]
    column_types = []

    for i in range(column_count):
        questions = [{
            'type': 'list',
            'name': 'choice',
            'message': f'Select a data type for the {column_names[i]} column:',
            'choices': Row.data_types
        }]
        column_type = prompt(questions)['choice']
        column_types.append(column_type)
        
        if column_type in ["number", "random_number"]:
            Row.starting_index[column_names[i]] = (
                int(input(f"Lowest: ")), int(input(f"Highest: "))
            ) if column_type == "random_number" else int(input(f"Enter starting index for {column_names[i]}: "))
        else:
            Row.starting_index[column_names[i]] = 0

    return column_names, column_types

def write_to_file(file_name, table_name, column_names, rows):
    with open(file_name, "w") as file:
        file.write(f"INSERT INTO {table_name}({', '.join(column_names)}) VALUES\n")
        values = [
            f"({', '.join([f'\'{val}\'' for val in row.values])})"
            for row in rows
        ]
        file.write(",\n".join(values) + ";")

def main():
    table_name = input("Enter table name: ")
    column_count = int(input("Enter number of columns: "))
    column_names, column_types = get_column_details(column_count)
    row_count = int(input("Enter number of rows you want to generate: "))
    file_name = input("Enter the name of the file you want to generate: ")

    rows = [Row(column_types, i) for i in range(row_count)]
    for row in rows:
        row.generate()

    write_to_file(file_name, table_name, column_names, rows)
    print("File generated successfully.")

if __name__ == "__main__":
    main()