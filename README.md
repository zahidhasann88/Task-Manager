# Task-Manager
TaskManager is a web application for managing tasks and assignments. It allows users to create and assign tasks to other users, track the progress of each task, and receive notifications when tasks are completed.

The application is built using a three-tier architecture, with separate layers for the presentation, business logic, and data access. The presentation layer is implemented as an ASP.NET Core API, which exposes a set of RESTful endpoints for interacting with the application. The business logic layer is implemented as a set of services that perform the core functionality of the application, such as creating and updating tasks. The data access layer is implemented using Entity Framework Core, which provides an object-relational mapping framework for interacting with a SQL Server database.

The application includes several key features, including:

User authentication and authorization using JSON Web Tokens (JWT)
CRUD (Create, Read, Update, Delete) operations for tasks and assignments
Real-time notifications using SignalR
Integration with an external email service for sending notifications
Automated tests for the API and infrastructure layers using xUnit and Moq
MyTaskManager is designed to be scalable and maintainable, with a clear separation of concerns between the different layers of the application. The codebase is well-organized and follows best practices for software development, including the SOLID principles and dependency injection.


