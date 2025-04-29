Notes API
Overview
Notes API is a RESTful web API built with ASP.NET Core, allowing users to register, manage their profiles, and create, update, retrieve, and delete notes. Each user can have multiple notes, and the API ensures data integrity by removing all notes associated with a user when their account is deleted. The API uses Entity Framework Core for database operations and SQL Server as the database.
This project is designed as a pet project to demonstrate CRUD operations, dependency injection, logging, and basic security practices in ASP.NET Core.
Features

User Management:
Register a new user with a nickname, email, and password.
Retrieve user details and their associated notes.
Update user nickname and password.
Delete a user account (automatically deletes all their notes).
List all users (excluding sensitive data like passwords).


Note Management:
Create a note for a specific user.
Update an existing note.
Retrieve all notes for a user or all notes in the system.
Delete a note.


Security:
Passwords are hashed using a custom IHasher interface.
User deletion and updates require email and password verification.


Documentation:
Swagger UI for interactive API exploration (available in development mode).



Technologies

ASP.NET Core 8.0: Web framework for building the API.
Entity Framework Core: ORM for database operations.
MSSQL Server: Relational database for storing users and notes.
Serilog: Logging framework (configurable via console and debug output).
Swagger/OpenAPI: API documentation and testing interface.
C#: Programming language.

Prerequisites

.NET 8.0 SDK
SQL Server (Express or Developer edition)
Git (for cloning the repository)
Optional: Postman for testing API endpoints

Installation

Clone the repository:
git clone https://github.com/your-username/notes-api.git
cd notes-api


Restore dependencies:
dotnet restore


Configure the database:

Update the connection string in appsettings.json:{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=NotesDb;Trusted_Connection=True;"
  }
}


Apply database migrations to create the schema:dotnet ef migrations add InitialCreate
dotnet ef database update




Run the application:
dotnet run

The API will be available at https://localhost:5001 (or your configured port).

Access Swagger UI (in development mode):

Open https://localhost:5001/swagger in your browser to explore and test the API.



API Endpoints
Below are the main API endpoints. Use Swagger UI or Postman to test them.
User Endpoints

POST /users

Register a new user.
Request body:{
  "nickname": "john_doe",
  "email": "john@example.com",
  "password": "SecurePass123"
}


Response: 200 OK with message "User 'john_doe' registered"


GET /users/{id}

Get user details and their notes by ID.
Response:{
  "username": "john_doe",
  "notes": [
    { "id": 1, "title": "My Note", "content": "Some content", "userId": 1 }
  ]
}




PUT /users/{id}

Update user nickname and password.
Request body:{
  "nickname": "john_updated",
  "email": "john@example.com",
  "password": "NewPass123"
}


Response: 200 OK with message "User 'john_updated' updated"


DELETE /users/{id}

Delete a user and all their notes (requires email and password).
Request body:{
  "email": "john@example.com",
  "password": "SecurePass123"
}


Response: 204 No Content


GET /users/list

Get a list of all users (excluding passwords).
Response:[
  { "id": 1, "nickname": "john_doe", "email": "john@example.com" }
]





Note Endpoints

POST /notes/{userId}

Create a note for a user.
Request body:{
  "title": "My Note",
  "content": "Some content"
}


Response: 201 Created with note details


PUT /notes/{noteId}

Update an existing note.
Request body:{
  "title": "Updated Note",
  "content": "Updated content"
}


Response: 200 OK with updated note details


GET /notes/{userId}

Get all notes for a user.
Response:{
  "username": "john_doe",
  "notes": [
    { "id": 1, "title": "My Note", "content": "Some content", "userId": 1 }
  ]
}




GET /notes

Get all notes in the system.
Response:[
  { "id": 1, "title": "My Note", "content": "Some content", "userId": 1 }
]




DELETE /notes/{noteId}

Delete a note.
Response: 204 No Content



Project Structure
NotesApi/
├── Controllers/
│   ├── UserController.cs       # Handles user-related operations
│   ├── NotesController.cs      # Handles note-related operations
├── Models/
│   ├── User.cs                 # User entity and DTO for user operations
│   ├── Note.cs                 # Note entity and DTO for user operations
├── Context/
│   ├── MyDbContext.cs          # EF Core DbContext
├── Program.cs                  # Application entry point
├── appsettings.json            # Configuration (connection strings, etc.)
└── README.md                   # This file

Security Notes

Passwords are hashed using a custom IHasher implementation.
User deletion and updates require email and password verification to prevent unauthorized access.
Warning: The GET /notes endpoint returns all notes without authentication, which may expose sensitive data. For production, add authentication (e.g., JWT or API key).

Contributing
Contributions are welcome! Please fork the repository, create a feature branch, and submit a pull request.
License
This project is licensed under the MIT License.
Contact
For questions or feedback, contact [your-username] on GitHub or via email at [your-email@example.com].
