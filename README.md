This repository contains a comprehensive e-commerce application built using a microservices architecture with .NET 8. The project is the result of a detailed microservices demonstrating best practices in building scalable and maintainable distributed systems.

## Project Overview

![image](https://github.com/user-attachments/assets/60900614-3b56-440d-b0ab-900e77be5aff)

This e-commerce application consists of multiple microservices that work together to provide a complete online food shopping experience. The microservices include:

- Product Microservice
- .NET Identity Microservice
- Coupon Microservice
- Shopping Cart Microservice
- Order Microservice
- Email Microservice
- Payment Microservice
- Ocelot API Gateway
- MVC Web Application

## Technologies Used

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Azure Service Bus
- Ocelot API Gateway
- .NET Identity for Authentication and Authorization

## Prerequisites

To run this project, you'll need:

- Visual Studio 2019 or later (2022 recommended for .NET 8)
- SQL Server Management Studio
- .NET 8 SDK
- Azure Service Bus account (for message queue implementation)

## Getting Started

1. Clone the repository:
2. Open the solution file in Visual Studio.
3. Set up your SQL Server connection strings in the `appsettings.json` file of each microservice.
4. Set up your Azure Service Bus connection string in the relevant microservices.
5. Run the database migrations for each microservice that uses a database:
6. Set multiple startup projects in Visual Studio to run all microservices simultaneously.
7. Run the application.

## Project Structure

The solution is structured with a clean approach:

- Each microservice has its own folder containing:
- API project
- The Ocelot API Gateway project is in its own folder
- The MVC Web Application is in a separate folder

## Running the Application

1. Ensure all microservices are set to start.
2. Run the application from Visual Studio.
3. The Ocelot API Gateway will route requests to the appropriate microservices.
4. Access the MVC Web Application to interact with the e-commerce platform.

## **Contributing:**

Contributions are welcome! Please fork the repository and submit pull requests for any enhancements or bug fixes.

## **License:**

This project is licensed under the MIT License. See the LICENSE file for more details.
