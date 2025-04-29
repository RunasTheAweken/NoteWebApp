
# 📘 Notes API

**Notes API** is a RESTful web API built with ASP.NET Core that allows users to register, manage profiles, and create, update, retrieve, and delete notes.  

This project is designed as a pet project to demonstrate CRUD operations, dependency injection, logging, and basic security practices in ASP.NET Core.

## ✨ Features

### 👤 User Management

- Register a new user with a nickname, email, and password.
- Retrieve user details and their notes.
- Update a user's nickname and password.
- Delete a user (automatically deletes all associated notes).
- List all users (excluding sensitive data like passwords).

### 📝 Note Management

- Create a note for a specific user.
- Update an existing note.
- Retrieve all notes for a user or all notes in the system.
- Delete a note.

### 🔐 Security

- Passwords are hashed using a custom `IHasher` interface.
- User deletion and updates require email and password verification.

### 📑 Documentation

- Swagger UI is available in development mode for API exploration and testing.

---

## ⚙️ Technologies

- **ASP.NET Core 8.0** – Web framework
- **Entity Framework Core** – ORM
- **SQL Server** – Relational database
- **Serilog** – Logging framework (console/debug)
- **Swagger / OpenAPI** – API documentation and testing
- **C#** – Programming language

---

## 📋 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (Express or Developer Edition)
- [Git](https://git-scm.com/)
- Optional: [Postman](https://www.postman.com/) for testing

---

## 🚀 Installation

### 1. Clone the repository:
```bash
git clone https://github.com/your-username/notes-api.git
cd notes-api
```

### 2. Restore dependencies:
```bash
dotnet restore
```

### 3. Configure the database:

Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=NotesDb;Trusted_Connection=True;"
  }
}
```

### 4. Apply migrations:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Run the application:
```bash
dotnet run
```

The API will be available at: `https://localhost:5001`  
Swagger UI: `https://localhost:5001/swagger`

---

## 📡 API Endpoints

### 👤 User Endpoints

#### `POST /users`  
Register a new user.  
**Request Body**:
```json
{
  "nickname": "john_doe",
  "email": "john@example.com",
  "password": "SecurePass123"
}
```

#### `GET /users/{id}`  
Get user details and their notes.

#### `PUT /users/{id}`  
Update user nickname and password.  
**Request Body**:
```json
{
  "nickname": "john_updated",
  "email": "john@example.com",
  "password": "NewPass123"
}
```

#### `DELETE /users/{id}`  
Delete a user (requires email and password).  
**Request Body**:
```json
{
  "email": "john@example.com",
  "password": "SecurePass123"
}
```

#### `GET /users/list`  
Retrieve a list of all users (without passwords).

---

### 📝 Note Endpoints

#### `POST /notes/{userId}`  
Create a note for a user.  
**Request Body**:
```json
{
  "title": "My Note",
  "content": "Some content"
}
```

#### `PUT /notes/{noteId}`  
Update a note.  
**Request Body**:
```json
{
  "title": "Updated Note",
  "content": "Updated content"
}
```

#### `GET /notes/{userId}`  
Get all notes for a specific user.

#### `GET /notes`  
Get all notes in the system.

#### `DELETE /notes/{noteId}`  
Delete a specific note.

---

## 🧱 Project Structure

```
NotesApi/
├── Controllers/
│   ├── UserController.cs         # Handles user operations
│   ├── NotesController.cs        # Handles note operations
├── Models/
│   ├── User.cs                   # User entity and DTOs
│   ├── Note.cs                   # Note entity and DTOs
├── Context/
│   ├── MyDbContext.cs            # EF Core DbContext
├── Program.cs                    # Application entry point
├── appsettings.json              # Configuration
└── README.md                     # This file
```

---

## 🔐 Security Notes

- Passwords are hashed using a custom `IHasher` implementation.
- Email and password are required for deleting and updating users.
- ⚠️ `GET /notes` endpoint returns all notes without authentication — avoid this in production without proper security (e.g., JWT or API key).

---

## 🤝 Contributing

Contributions are welcome!  
Fork the repository, create a feature branch, and open a pull request.

---

## 📄 License

This project is licensed under the **MIT License**.

---

## 📬 Contact

For questions or feedback, contact [your-username] on GitHub or via email at [your-email@example.com].
```

---
