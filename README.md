# Balls R Us - E-commerce Platform for Golf Equipment

[![en](https://img.shields.io/badge/lang-en-red.svg)](https://github.com/ocharron/BallsRUs/blob/master/README.md)
[![fr](https://img.shields.io/badge/lang-fr-blue.svg)](https://github.com/ocharron/BallsRUs/blob/master/README_fr.md)

Welcome to Balls R Us, an (fictive) e-commerce platform specializing in the sale of high-quality golf equipment. Whether you're a passionate amateur or an experienced professional, Balls R Us offers a diverse range of products to meet all your needs on the golf course.

---

## Technologies Used

- **Framework**: ASP.NET Core (.NET 8.0)
- **Database**: Microsoft SQL Server 2022
- **ORM**: Entity Framework Core
- **Payments**: Stripe

---

## Installation

To install and run Balls R Us on your local machine, please follow these steps:

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio](https://visualstudio.microsoft.com/downloads/) or a text editor of your choice (Visual Studio Code, etc.)
- [Microsoft SQL Server 2022](https://www.microsoft.com/en-ca/sql-server/sql-server-downloads) (or later version)
- [Stripe](https://stripe.com/en-ca) API Key

### Instructions

1. **Clone the Repository**
   ```bash
   git clone https://github.com/ocharron/BallsRUs.git
   ```

2. **Database Configuration**
   - Create a new database in SQL Server.
   - Update the connection string in the `appsettings.json` file with your database details.
   - Execute a Migration and an Update-Database. (Code First)

3. **Stripe Configuration**
   - Obtain a Stripe API Key (secret key) from the Stripe dashboard.
   - Add this API key to the `appsettings.json` file.

4. **Compilation and Execution**
   - Open the project in Visual Studio or use the command line.
   - Run the following command to restore dependencies:
     ```bash
     dotnet restore
     ```
   - Then, run the application:
     ```bash
     dotnet run
     ```

---

## Key Features

1. **Content management system (CMS)**: Manage the catalog and the products from a tailor-made content management system.
2. **Product Catalog**: Browse through a wide range of golf equipment, including clubs, balls, bags, shoes, etc.
3. **Shopping Cart Management**: Add products to your cart and manage them easily before proceeding to checkout.
4. **Secure Payment**: Use Stripe for safe and secure payment transactions.
5. **User Management**: Register, login, and manage your user profile for a personalized experience.

---

## Contribution

Contributions are welcome! If you'd like to use Balls R Us, please follow these steps:

1. Fork the project
2. Create a branch for your feature (`git checkout -b feature/YourFeatureName`)
3. Commit your changes (`git commit -m 'Add a new feature'`)
4. Push to the branch (`git push origin feature/YourFeatureName`)
5. Submit a pull request

---

## Author

This project was developed by [Olivier Charron](https://github.com/ocharron), [Xavier Côté-Daviau](https://github.com/xavcd), [Mahdi Ellili](https://github.com/mahdilili) and [Michael Meilleur](https://github.com/MichaelMeilleur).

---

## Disclaimer

This is a fictitious project created solely for demonstration and educational purposes. Any resemblance to real products, services, or companies is purely coincidental. Several media used on the application are not royalty free.