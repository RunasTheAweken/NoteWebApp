# 📘 Notes API

**Notes API** is a RESTful web API built with ASP.NET Core that allows users to register, manage profiles, and create, update, retrieve, and delete notes.  

This project is designed as a pet project to demonstrate CRUD operations, dependency injection, logging, and basic security practices in ASP.NET Core.

---

## 🧭 Table of Contents

- [Features](#features)
- [Technologies](#technologies)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [API Endpoints](#api-endpoints)
  - [User Endpoints](#user-endpoints)
  - [Note Endpoints](#note-endpoints)
- [Project Structure](#project-structure)
- [Security Notes](#security-notes)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

---

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
