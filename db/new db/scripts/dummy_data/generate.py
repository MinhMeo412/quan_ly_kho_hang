from InquirerPy import prompt
from faker import Faker
from random import Random
from uuid import uuid4


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
        elif column_types[i] == "random_number":
            print(f"Enter range for {column_names[i]}:")
            start = int(input(f"Lowest: "))
            end = int(input(f"Highest: "))
            Row.starting_index[column_names[i]] = (start, end)
        else:
            Row.starting_index[column_names[i]] = 0
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
            else:
                values += ";"
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
        "company_name",
        "product_name",
        "product_category",
        "product_size",
        "number",
        "random_number",
        "color",
        "permission_level",
        "uuid",
        "shipment_status"
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
            if self.columns[i] == "company_name":
                value = Row.generate_company_name()
            if self.columns[i] == "product_name":
                value = Row.generate_product_name()
            if self.columns[i] == "product_category":
                value = Row.generate_product_category()
            if self.columns[i] == "product_size":
                value = Row.generate_product_size()
            if self.columns[i] == "number":
                value = Row.generate_number(self, i)
            if self.columns[i] == "random_number":
                value = Row.generate_random_number(self, i)
            if self.columns[i] == "color":
                value = Row.generate_color()
            if self.columns[i] == "permission_level":
                value = Row.generate_permission_level()
            if self.columns[i] == "uuid":
                value = Row.generate_uuid()
            if self.columns[i] == "shipment_status":
                value = Row.generate_shipment_status()

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
        districts = ["Ba Đình", "Hoàn Kiếm", "Hai Bà Trưng", "Đống Đa", "Tây Hồ", "Cầu Giấy", "Thanh Xuân", "Hoàng Mai", "Long Biên", "Bắc Từ Liêm", "Nam Từ Liêm", "Thanh Trì", "Gia Lâm", "Đông Anh",
                     "Sóc Sơn", "Mê Linh", "Hà Đông", "Sơn Tây", "Ba Vì", "Phúc Thọ", "Đan Phượng", "Hoài Đức", "Quốc Oai", "Thạch Thất", "Chương Mỹ", "Thanh Oai", "Thường Tín", "Phú Xuyên", "Mỹ Đức", "Mê Linh"]
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

    def generate_company_name():
        return Faker().company()


    def generate_product_name():
        adjectives = [
            "Advanced", "Professional", "Ultimate", "Eco", "Smart", "Intelligent", "Portable", "Compact", "Premium", "Elite",
            "Durable", "Innovative", "Efficient", "Reliable", "High-Performance", "Lightweight", "Powerful", "User-Friendly",
            "Versatile", "Flexible", "Affordable", "Cutting-Edge", "Eco-Friendly", "Robust", "Sleek", "Stylish", "Modern",
            "Classic", "High-Tech", "Next-Gen", "Energy-Saving", "Sustainable", "Ergonomic", "Precision", "Advanced", "Pro",
            "Max", "Ultra", "Hyper", "Super", "Mega", "Mini", "Nano", "Compact", "Pocket", "Portable", "Wireless", "Bluetooth",
            "Smart", "Intelligent", "AI-Powered", "Digital", "Analog", "Electric", "Solar", "Battery-Powered", "Rechargable",
            "Quick-Charge", "Fast", "Rapid", "Efficient", "Reliable", "Durable", "Heavy-Duty", "Industrial", "Commercial",
            "Residential", "Office", "Home", "Outdoor", "Indoor", "Waterproof", "Weatherproof", "Shockproof", "Impact-Resistant",
            "Scratch-Resistant", "Heat-Resistant", "Cold-Resistant", "Fire-Resistant", "Rustproof", "Corrosion-Resistant",
            "UV-Resistant", "Anti-Static", "Antimicrobial", "Hypoallergenic", "Bio-Degradable", "Eco-Conscious", "Recycled",
            "Natural", "Organic", "Synthetic", "Non-Toxic", "Chemical-Free", "Pesticide-Free", "GMO-Free", "All-Natural",
            "Vegan", "Gluten-Free", "Keto", "Low-Carb", "High-Protein", "Sugar-Free", "Low-Fat", "Low-Calorie", "Diet",
            "Healthy", "Nutritious", "Fortified", "Enriched", "Functional", "Multi-Functional", "Specialty", "Gourmet"
        ]
        
        nouns = [
            "Gadget", "Device", "System", "Tool", "Instrument", "Appliance", "Machine", "Unit", "Item", "Product",
            "Component", "Element", "Part", "Accessory", "Attachment", "Addon", "Module", "Kit", "Package", "Set",
            "Series", "Model", "Edition", "Version", "Type", "Kind", "Class", "Category", "Style", "Design", "Pattern",
            "Frame", "Body", "Shell", "Case", "Cover", "Sleeve", "Holder", "Stand", "Mount", "Bracket", "Base", "Platform",
            "Dock", "Station", "Hub", "Center", "Console", "Terminal", "Interface", "Panel", "Screen", "Display", "Monitor",
            "Keyboard", "Mouse", "Trackpad", "Remote", "Controller", "Joystick", "Gamepad", "Adapter", "Converter",
            "Connector", "Cable", "Wire", "Cord", "Charger", "Battery", "Powerbank", "Inverter", "Transmitter", "Receiver",
            "Antenna", "Satellite", "Sensor", "Detector", "Alarm", "Beacon", "Light", "Lamp", "Bulb", "Fixture", "Projector",
            "Camera", "Lens", "Microscope", "Telescope", "Binoculars", "Scope", "Mic", "Speaker", "Headphones", "Earbuds",
            "Amplifier", "Equalizer", "Mixer", "Turntable", "Record", "CD", "DVD", "Blu-ray", "Player", "Recorder", "Scanner",
            "Printer", "Copier", "Fax", "Shredder", "Plotter", "Cutter", "Drill", "Saw", "Hammer", "Wrench", "Screwdriver",
            "Pliers", "Multitool", "Knife", "Scissors", "Blade", "Cutter", "Punch", "Chisel", "File", "Grinder", "Polisher",
            "Buffer", "Sander", "Welder", "Torch", "Blowtorch", "Bunsen", "Burner", "Oven", "Stove", "Microwave", "Kettle",
            "Toaster", "Blender", "Mixer", "Grinder", "Juicer", "Processor", "Cutter", "Chopper", "Press", "Mincer", "Mill",
            "Crusher", "Grater", "Slicer", "Peeler", "Opener", "Strainer", "Funnel", "Whisk", "Ladle", "Tongs", "Spatula",
            "Spoon", "Fork", "Knife", "Chopsticks", "Bowl", "Plate", "Tray", "Cup", "Glass", "Mug", "Pitcher", "Decanter",
            "Bottle", "Jar", "Can", "Container", "Box", "Bag", "Bin", "Basket", "Hanger", "Hook", "Clip", "Pin", "Stapler",
            "Tape", "Glue", "Adhesive", "Sealant", "Caulk", "Putty", "Filler", "Cleaner", "Polish", "Wax", "Oil", "Lubricant",
            "Grease", "Solvent", "Detergent", "Disinfectant", "Sanitizer", "Soap", "Shampoo", "Conditioner", "Lotion", "Cream",
            "Gel", "Spray", "Aerosol", "Foam", "Mousse", "Powder", "Tablet", "Capsule", "Pill", "Vitamin", "Supplement",
            "Probiotic", "Enzyme", "Extract", "Herb", "Spice", "Seasoning", "Sauce", "Dressing", "Marinade", "Paste", "Spread",
            "Butter", "Cheese", "Yogurt", "Milk", "Cream", "Ice Cream", "Sorbet", "Gelato", "Pudding", "Custard", "Jelly",
            "Jam", "Preserve", "Honey", "Syrup", "Molasses", "Sugar", "Sweetener", "Salt", "Pepper", "Vinegar", "Oil", "Fat",
            "Butter", "Margarine", "Shortening", "Lard", "Ghee", "Tallow", "Suet", "Bacon", "Ham", "Sausage", "Patty",
            "Meatball", "Steak", "Chop", "Cutlet", "Filet", "Loin", "Rib", "Roast", "Brisket", "Shank", "Shoulder", "Wing",
            "Drumstick", "Thigh", "Breast", "Tenderloin", "Ribeye", "Sirloin", "Strip", "T-bone", "Porterhouse", "Rump",
            "Round", "Flank", "Skirt", "Hanger", "Oxtail", "Neck", "Offal", "Tripe", "Liver", "Kidney", "Heart", "Brain",
            "Tongue", "Truffle", "Mushroom", "Fungus", "Yeast", "Mold", "Bacteria", "Virus", "Protozoa", "Algae", "Seaweed",
            "Kelp", "Spirulina", "Chlorella", "Diatom", "Lichen", "Fungi", "Mycelium", "Spores", "Mushrooms", "Agaricus",
            "Amanita", "Boletus", "Cantharellus", "Cortinarius", "Entoloma", "Hebeloma", "Inocybe", "Laccaria", "Lactarius",
            "Lepiota", "Leucoagaricus", "Marasmius", "Mycena", "Omphalotus", "Panellus", "Pholiota", "Pleurotus", "Psilocybe",
            "Russula", "Stropharia", "Tricholoma", "Xerocomus", "Cordyceps", "Ophiocordyceps", "Beauveria", "Metarhizium",
            "Aspergillus", "Penicillium", "Rhizopus", "Mucor", "Candida", "Cryptococcus", "Trichophyton", "Microsporum",
            "Epidermophyton", "Histoplasma", "Coccidioides", "Blastomyces", "Paracoccidioides", "Sporothrix", "Fonsecaea",
            "Cladophialophora", "Exophiala", "Madurella", "Pseudallescheria", "Scedosporium", "Alternaria", "Curvularia",
            "Bipolaris", "Fusarium", "Acremonium", "Cladosporium", "Chaetomium", "Stachybotrys", "Aspergillus", "Penicillium",
            "Rhizopus", "Mucor", "Candida", "Cryptococcus", "Trichophyton", "Microsporum", "Epidermophyton", "Histoplasma",
            "Coccidioides", "Blastomyces", "Paracoccidioides", "Sporothrix", "Fonsecaea", "Cladophialophora", "Exophiala",
            "Madurella", "Pseudallescheria", "Scedosporium", "Alternaria", "Curvularia", "Bipolaris", "Fusarium", "Acremonium",
            "Cladosporium", "Chaetomium", "Stachybotrys", "Aspergillus", "Penicillium", "Rhizopus", "Mucor", "Candida",
            "Cryptococcus", "Trichophyton", "Microsporum", "Epidermophyton", "Histoplasma", "Coccidioides", "Blastomyces",
            "Paracoccidioides", "Sporothrix", "Fonsecaea", "Cladophialophora", "Exophiala", "Madurella", "Pseudallescheria",
            "Scedosporium", "Alternaria", "Curvularia", "Bipolaris", "Fusarium", "Acremonium", "Cladosporium", "Chaetomium",
            "Stachybotrys"
        ]

        adjective = Random().choice(adjectives)
        noun = Random().choice(nouns)
        version = Random().randint(1, 10000)
        return f"{adjective} {noun} Version {version}"
        
    def generate_product_category():
        product_categories = ["Electronics", "Clothing", "Home and Kitchen", "Books", "Toys", "Beauty and Personal Care",
                              "Sports and Outdoors", "Health and Wellness", "Automotive", "Tools and Home Improvement"]
        product_category = Random().choice(product_categories)
        return product_category

    def generate_product_size():
        product_sizes = ["XS", "S", "M", "L", "XL", "XXL", "One Size", "Very Small", "Small", "Medium", "Big", "Very Big"]
        product_size = Random().choice(product_sizes)
        return product_size

    def generate_number(self, current_column):
        return Row.starting_index[list(Row.starting_index.keys())[current_column]] + self.index

    def generate_random_number(self, current_column):
        a = Row.starting_index[list(Row.starting_index.keys())[current_column]][0]
        b = Row.starting_index[list(Row.starting_index.keys())[current_column]][1]
        return Random().randint(a, b)

    def generate_color():
        color_name = Faker().color_name()
        upper_case_letters = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"]
        new_color_name = ""
        for char in color_name:
            if char in upper_case_letters:
                new_color_name += f" {char}"
            else:
                new_color_name += char
        return new_color_name.strip()

    def generate_permission_level():
        return Random().randint(0, 4)

    def generate_uuid():
        return uuid4()
    
    def generate_shipment_status():
        statuses = ['Processing']
        status = Random().choice(statuses)
        return status


if __name__ == "__main__":
    main()