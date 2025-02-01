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
   
2. Configure Sendgrid API Key and Sender Email:

   Open the Dockerfile and and add the following environment variables for Sendgrid:
   ```bash
   ENV SENDGRID_API_KEY your_sendgrid_api_key
   ENV SENDGRID_SENDER_EMAIL your_sender_email@example.com
   ```

3. Navigate to the project folder:

   ```bash
   cd SMS-Backend
   ```

4. Start the Docker containers (this will run WebAPI, PostgreSQL and pgAdmin):

   ```bash
   docker-compose up
   ```
