# SPA Salon Management System

## Project Description

ToDo

## Features

ToDo

## API Documentation

The API documentation is available at the following link:

[API Documentation](https://documenter.getpostman.com/view/28707892/2sAXxJiExN)

## Technologies

ToDo

## How to Run the Project

1. Clone the repository:

   ```bash
   git clone https://github.com/zml18x/SMS-Backend
   ```
   
2. Navigate to the project folder:

   ```bash
   cd SMS-Backend
   ```

3. Start the application using Docker, which will run PostgreSQL and pgAdmin:

   ```bash
   cd docker-compose up
   ```

4. Navigate to the start project source directory:

   ```bash
   cd src/SpaManagementSystem.WebApi
   ```

5. Set user secrets for SendGrid API:

   ```bash
   dotnet user-secrets set SENDGRID_API_KEY your_apikey
   dotnet user-secrets set SENDGRID_SENDER_EMAIL your_email_sender
   ```
  
7. Run the project:

   ```bash
   cd dotnet run
   ```

   
   
