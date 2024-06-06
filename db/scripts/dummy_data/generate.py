from InquirerPy import prompt
from faker import Faker
from random import Random


def main():

    table_name = input("Enter table name: ")
    column_count = int(input("Enter number of columns: "))
    column_names = []
    column_types = []

    for i in range(column_count):
        column_name = input(f"Enter the name of column {i + 1}: ")
        column_names.append(column_name)

    for i in range(column_count):
        questions = [
            {
                'type': 'list',
                'name': 'choice',
                'message': f'Select a data type for the {column_names[i]} column:',
                'choices': Row.data_types
            }
        ]
        column_types.append(prompt(questions)['choice'])
        if column_types[i] == "number":
            starting_index = int(input(f"Enter starting index for {column_names[i]}: "))
            Row.starting_index[column_names[i]] = starting_index
    row_count = int(input("Enter number of rows you want to generate: "))
    file_name = input("Enter the name of the file you want to generate: ")

    rows = []
    for i in range(row_count):
        rows.append(Row(column_types, i))

    for i in range(row_count):
        rows[i].generate()
    
    with open(file_name, "w") as file:
        print(f"Writing to {file_name}")
        insert_line = f"INSERT INTO {table_name}("
        
        for column_name in column_names:
            insert_line += f"{column_name}, "
        insert_line = f"{insert_line[:-2]}) VALUES\n"
        file.write(insert_line)

        for i in range(row_count):
            values = "("
            for ii in range(column_count):
                values += f"'{rows[i].values[ii]}', "
            values = f"{values[:-2]})"
            if i < row_count - 1:
                values += ",\n"
            file.write(values)
        print("File generated successfully.")

class Row():
    data_types = [
        "full_name",
        "email",
        "phone_number",
        "username",
        "password",
        "description",
        "address",
        "district",
        "city",
        "postal_code",
        "country",
        "datetime",
        "website_link",
        "image_url",
        "product_name",
        "product_category",
        "product_size",
        "number",
        "random_number",
        "color"
    ]
    starting_index = {}

    def __init__(self, columns, index):
        self.columns = columns
        self.index = index

    def generate(self):
        values = []
        print(f"Generating: ", end="")
        for i in range(len(self.columns)):
            if self.columns[i] == "full_name":
                value = Row.generate_full_name()
            if self.columns[i] == "email":
                value = Row.generate_email()
            if self.columns[i] == "phone_number":
                value = Row.generate_phone_number()
            if self.columns[i] == "username":
                value = Row.generate_username()
            if self.columns[i] == "password":
                value = Row.generate_password()
            if self.columns[i] == "description":
                value = Row.generate_description()
            if self.columns[i] == "address":
                value = Row.generate_address()
            if self.columns[i] == "district":
                value = Row.generate_district()
            if self.columns[i] == "city":
                value = Row.generate_city()
            if self.columns[i] == "postal_code":
                value = Row.generate_postal_code()
            if self.columns[i] == "country":
                value = Row.generate_country()
            if self.columns[i] == "datetime":
                value = Row.generate_datetime()
            if self.columns[i] == "website_link":
                value = Row.generate_website_link()
            if self.columns[i] == "image_url":
                value = Row.generate_image_url()
            if self.columns[i] == "product_name":
                value = Row.generate_product_name()
            if self.columns[i] == "product_category":
                value = Row.generate_product_category()
            if self.columns[i] == "product_size":
                value = Row.generate_product_size()
            if self.columns[i] == "number":
                value = Row.generate_number(self, i)
            if self.columns[i] == "random_number":
                value = Row.generate_random_number()
            if self.columns[i] == "color":
                value = Row.generate_color()

            values.append(value)
            print(f"{self.columns[i]}: {value}", end=", ")
        print()
        self.values = values

    def generate_full_name():
        return Faker().name()

    def generate_email():
        return Faker().company_email()

    def generate_phone_number():
        return Faker().basic_phone_number()

    def generate_username():
        return Faker().user_name()

    def generate_password():
        return Faker().password()

    def generate_description():
        return f"{Faker().text()}{Faker().text()}"

    def generate_address():
        return Faker().street_address()

    def generate_district():
        districts = ["Ba Đình", "Hoàn Kiếm", "Hai Bà Trưng", "Đống Đa", "Tây Hồ", "Cầu Giấy", "Thanh Xuân", "Hoàng Mai", "Long Biên", "Bắc Từ Liêm", "Nam Từ Liêm", "Thanh Trì", "Gia Lâm", "Đông Anh", "Sóc Sơn", "Mê Linh", "Hà Đông", "Sơn Tây", "Ba Vì", "Phúc Thọ", "Đan Phượng", "Hoài Đức", "Quốc Oai", "Thạch Thất", "Chương Mỹ", "Thanh Oai", "Thường Tín", "Phú Xuyên", "Mỹ Đức", "Mê Linh"]
        district = Random().choice(districts)
        return district

    def generate_city():
        return Faker().city()

    def generate_postal_code():
        return Faker().postalcode()

    def generate_country():
        return Faker().country()

    def generate_datetime():
        return Faker().date_time().strftime('%Y-%m-%d %H:%M:%S')

    def generate_website_link():
        return Faker().url()

    def generate_image_url():
        return f"{Faker().url()}/{Faker().word()}-image.png"

    def generate_product_name():
        return Faker().company()

    def generate_product_category():
        product_categories = ["Electronics", "Clothing", "Home and Kitchen", "Books", "Toys", "Beauty and Personal Care", "Sports and Outdoors", "Health and Wellness", "Automotive", "Tools and Home Improvement"]
        product_category = Random().choice(product_categories)
        return product_category

    def generate_product_size():
        product_sizes = ["XS", "S", "M", "L", "XL", "XXL", "One Size"]
        product_size = Random().choice(product_sizes)
        return product_size

    def generate_number(self, current_column):
        return Row.starting_index[list(Row.starting_index.keys())[current_column]] + self.index

    def generate_random_number(a = 0, b = 1000):
        return Random().randint(a, b)

    def generate_color():
        return Faker().color_name()


if __name__ == "__main__":
    main()
