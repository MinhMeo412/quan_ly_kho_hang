# Warehouse Manager

<div align="center">
  <a href="https://img.shields.io/badge/app-console-green"><img alt="Console App" src="https://img.shields.io/badge/app-console-green"></a>
  <a href="https://img.shields.io/badge/license-GPLv3-purple"><img alt="GPL v3.0" src="https://img.shields.io/badge/license-GPLv3-purple"></a>
  <a href="https://img.shields.io/badge/.NET-8.0-blue"><img alt=".NET 8.0" src="https://img.shields.io/badge/.NET-8.0-blue"></a>
</div>

Warehouse Manager is an application designed to streamline warehouse operations, including inventory management, order processing, and reporting.

## Table of Contents
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [Testing](#testing)
- [Project Structure](#project-structure)
- [License](#license)
- [Contact Information](#contact-information)
- [Acknowledgments](#acknowledgments)

## Features

- **Inventory Management:** Track and manage inventory levels, including adding, updating, and removing items.
- **Order Processing:** Create, update, and manage customer orders, including order status tracking.
- **Reporting:** Generate reports on inventory, sales, and order history.
- **User Authentication:** Secure user login and role-based access control.
- **Database Integration:** Seamless integration with MySQL for data storage and retrieval.
- **Excel Export/Import:** Export inventory and order data to Excel files and import data from Excel files.
- **Console Interface:** Interactive and user-friendly console interface.
- **Search Functionality:** Search capabilities to find items and orders by various criteria.

### Demo

![Demo GIF](https://i.imgur.com/DDj1tlY.gif)

## Installation

### Prerequisites
- [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- [MySQL](https://dev.mysql.com/downloads/mysql/) (latest version)

### Steps
1. Clone the repository:
    ```bash
    git clone git@github.com:MinhMeo412/quan_ly_kho_hang.git
    ```
2. Navigate to the scripts directory:
    ```bash
    cd quan_ly_kho_hang/db/scripts
    ```
3. Set up the MySQL database:
    - Start your MySQL server.
    - Log in using your mysql account:
        ```bash
        mysql -u your-username -p
        ```
    - Drop or rename the `warehouse` database if it exists:
        ```sql
        DROP DATABASE warehouse;
        ```
    - Execute the `main.sql` script to set up the database schema and initial data:
        ```sql
        SOURCE main.sql;
        ```

## Usage

1. Navigate to the application directory:
    ```bash
    cd quan_ly_kho_hang/src/WarehouseManager
    ```
2. Run the application:
    ```bash
    dotnet run
    ```

## Contributing

1. Fork the repository.
2. Create a new branch:
    ```bash
    git checkout -b feature/your-feature-name
    ```
3. Commit your changes:
    ```bash
    git commit -m "Add feature"
    ```
4. Push to the branch:
    ```bash
    git push origin feature/your-feature-name
    ```
5. Create a new Pull Request.

## Testing

Run the test suite with the following command:
```bash
[Nothing for the time being]
```

## Project Structure
- **`db/scripts/`** - Database scripts.
- **`docs/`** - Project documentation.
- **`src/WarehouseManager/`** - Source code for the Warehouse Manager application.
- **`test/WarehouseManagerTest/`** - Tests for the Warehouse Manager application.
- **`.gitignore`** - Files and directories to be ignored by git.
- **`LICENSE`** - License for the project.
- **`README.md`** - This file.

## License

This project is licensed under the GNU General Public License v3.0 - see the [LICENSE](./LICENSE) file for details.

## Contact Information

For any questions or feedback, please open an issue on this repository.

## Acknowledgments

- [Terminal.Gui](https://www.nuget.org/packages/Terminal.Gui) - A library to create console applications with a graphical user interface.
- [MySql.Data](https://www.nuget.org/packages/MySql.Data) - A library to interact with MySQL databases.
- [EPPlus](https://www.nuget.org/packages/EPPlus) - A library to manage Excel spreadsheets.