# Task Manager API

Task Manager API is a web application designed for efficient task and assignment management. Users can create, assign, track tasks, and receive notifications upon task completion.

## Overview

TaskManager is a scalable web application that utilizes a three-tier architecture, ensuring a clear separation of presentation, business logic, and data access layers. The presentation layer, implemented as an ASP.NET Core API, exposes RESTful endpoints for interacting with the application. The business logic layer consists of services responsible for core functionalities like task management. The data access layer is facilitated by Entity Framework Core, offering an object-relational mapping framework for PostgreSQL interactions.

### Key Features

- **User Authentication and Authorization**: Utilizes JSON Web Tokens (JWT) for secure access.
- **CRUD Operations for Tasks and Assignments**: Enables Create, Read, Update, and Delete operations.
- **Integration with External Email Service**: Sends notifications through an external email service.

## Challenges Faced

During the development of Task Manager API, several challenges were encountered, including:

- **Secure Implementation of SMTP for Email Notifications**: Ensuring secure and reliable implementation of SMTP for email notifications posed initial hurdles.
- **Consistent Enforcement of Clean Architecture Principles**: Maintaining consistency in adhering to Clean Architecture principles throughout the development process required continuous attention and effort.

## Architecture

TaskManager ensures scalability and maintainability with a robust separation of concerns across its layers. The codebase follows industry best practices, incorporating SOLID principles and dependency injection.

## Getting Started

To begin using the Task Manager API:

1. **Clone the Repository**: Clone the repository from [Task Manager](https://github.com/zahidhasann88/Task-Manager).
2. **Installation**: Install dependencies as specified in the project documentation.
3. **Configuration**: Set up environment variables and configurations.
4. **Usage**: Run the API and explore the functionalities.

## Technologies Used

- **ASP.NET Core**: Presentation layer framework.(Framework for building cross-platform applications.)
- **Entity Framework Core (EF Core)**: Data access layer for PostgreSQL interaction.(Object-Relational Mapping (ORM) framework for .NET.)
- **Web API**: Framework for building HTTP services.
- **PostgreSQL**: Powerful open-source relational database system.



