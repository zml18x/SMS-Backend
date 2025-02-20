# SPA Salon Management System

## Project Description

Spa Management System (SMS) is a solution built with ASP.NET Core WebAPI designed to manage spa salons efficiently. The system provides comprehensive functionalities to manage spa and salon businesses, enabling efficient handling of appointments, employees, customers, payments, and services.

The system follows Clean Architecture principles to ensure scalability and maintainability.

For authentication and authorization, it leverages ASP.NET Core Identity with JWT tokens, ensuring secure access control. Additionally, SendGrid is used for email-based operations, including email confirmation, password reset, and email change verification.

## Features

### Authentication & Authorization

- User registration, login, and token-based authentication (JWT).

- Email confirmation, password reset, and email change workflows using SendGrid.

- Role-based access control with Admin, Manager, and Employee roles.

- User management features like account locking and role assignment.

### Appointment Management

- Create, retrieve, update, and delete appointments.

- Filter appointments by salon, employee, or customer.

- Update appointment status via PATCH requests.

### Customer Management

- Create, update, retrieve, and delete customer profiles.

- Retrieve all customers associated with a specific salon.

### Employee Management

- Manage employee records, including creation, retrieval, and deletion.

- Assign and remove services to/from employees.

- Update employee details and availability.

- Retrieve all employees or specific employees within a salon.

### Employee Availability

- Manage employee availability schedules.

- Retrieve, update, and delete availability slots.

### Payment Management

- Process payments linked to specific appointments.

- Retrieve payment details by appointment or customer.

- Update payment records when necessary.

### Product Management

- Add, update, retrieve, and delete salon products.

- Retrieve a list of all available products.

### Salon Management

- Create and manage salon details, including address and opening hours.

- Retrieve a list of all salons or a specific salon’s details.

- Manage salon opening hours and update salon addresses.

### Service Management

- Add, update, retrieve, and delete services offered by salons.

- Retrieve a list of employees providing a specific service.

### User Management

- Retrieve user details.

- Assign and remove roles for users.

- Lock and unlock user accounts.

### Testing Endpoint

- A /api/test endpoint is available to verify API functionality.

## API Documentation

The API documentation is available at the following link:

[API Documentation](https://documenter.getpostman.com/view/28707892/2sAXxJiExN)

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
   cd SpaManagementSystem-WebApi
   ```

4. Start the Docker containers (this will run WebAPI, PostgreSQL and pgAdmin):

   ```bash
   docker-compose up
   ```