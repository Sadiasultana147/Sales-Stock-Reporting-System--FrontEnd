🚀 Used Technologies
Database: Microsoft SQL Server

API: ASP.NET Core Web API

Frontend: ASP.NET Core MVC

AJAX & jQuery: For client-side server requests

Reporting: Microsoft Report Builder

ORM: Entity Framework Core

Project Setup & Running Instructions

📌 1. Database Setup
Open SQL Server Management Studio.

Run this command to get your default data path:EXEC sp_helpfile

Replace the path Of FileName in the script execute it to create the database with sample data.

⚙️ 2. API Setup
Open the API solution in Visual Studio.

In appsettings.json, update the connection string to point to your SQL Server

Run the API project.

Use Swagger UI to test endpoints:

Product Management (CRUD)

Sales Management

User Registration & Login

Authenticate using JWT Token via the Authorize button in Swagger after login.


🖥️ 3. Frontend Setup

Open the MVC project in Visual Studio.

Run the application.

Login using default credentials:

Username: Admin

Password: Admin123456

Or register a new user and then login.

All product, sales, and reporting features require authentication.

✅ Features

🔐 Authentication

User registration and secure login (hashed passwords)

Authenticated access to all pages

📦 Product Management

CRUD operations
Fields: Product ID, Name, SKU, Price, Stock, Description

Soft delete with IsDeleted flag

💰 Sales Management

Select product and enter quantity

Auto-calculated total sale price

Validation: Cannot sell more than stock

Sale record includes ProductID, Qty, Price, Date

📈 Reporting

Current Stock Report: Product Name, SKU, Price, Stock

Date-wise Report: Opening Stock, Sold Quantity, Closing Stock

🔍 Pagination & Search

Pagination (10 items per page)

Search by Product Name or SKU

🔧 Swagger Integration

JWT-based Authorization in Swagger

All APIs testable via Swagger UI

🌟 Extra Features

📊 Chart.js Integration for:

Product Stock Chart (Current Year)

Product Sale Chart

🧹 Clean UI with Bootstrap and jQuery

🧱 Follows Clean Architecture (Domain, Application, Infrastructure, Presentation)
