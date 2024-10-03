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

3. Generate HTTPS certificates:

   ```bash
   dotnet dev-certs https --export-path ./certs/aspnetapp.pfx --password your_password
   ```
   Note: Ensure that the password used here matches the one in the Dockerfile for the API.

4. Start the Docker containers (this will run WebAPI, PostgreSQL and pgAdmin):

   ```bash
   docker-compose up
   ```
