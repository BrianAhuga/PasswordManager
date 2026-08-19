# PasswordManager

A password management application built with **ASP.NET Core MVC and .NET 8**, designed to generate, store, organize, and manage passwords for personal accounts.

The project demonstrates practical implementation of password generation, password hashing, Entity Framework Core, SQL Server, MVC architecture, validation, and paginated data management.

> **Note:** This project is intended for learning and demonstration purposes and should not be used as a production password vault without additional security hardening.

## Overview

**PasswordManager** provides a simple interface for managing credentials associated with different websites and services.

Users can create password entries by specifying the website, account owner, and email address. The application generates a password automatically, applies password hashing, and stores the resulting record in the database.

The application also provides a paginated view of stored password entries and masks sensitive information when displaying password details.

## Features

* Generate strong passwords
* Create password entries
* Store website and account information
* Password hashing using PBKDF2
* Random salt generation
* Password verification
* Duplicate password-entry detection
* Password detail view
* Masked password display
* Masked email display
* Password reveal workflow
* Form validation
* SQL Server database integration
* Entity Framework Core
* Database migrations
* Paginated password-entry listing
* MVC architecture
* Anti-forgery protection

The password generation and hashing workflow is implemented in `PasswordEntriesController`, while password records are represented through the `PasswordEntry` model.

## Technology Stack

| Technology                  | Purpose                               |
| --------------------------- | ------------------------------------- |
| **C#**                      | Primary programming language          |
| **.NET 8**                  | Application framework                 |
| **ASP.NET Core MVC**        | Web application architecture          |
| **Entity Framework Core 8** | Data access and ORM                   |
| **SQL Server**              | Database                              |
| **X.PagedList**             | Pagination                            |
| **Razor Views**             | Server-rendered UI                    |
| **PBKDF2**                  | Password hashing                      |
| **Cryptographic APIs**      | Salt generation and password security |

The project targets `.NET 8` and uses Entity Framework Core 8 with SQL Server, alongside `X.PagedList` for pagination.

## Architecture

The application follows the ASP.NET Core MVC architecture:

```text id="y8u9s2"
PasswordManager
│
├── Controllers
│   ├── HomeController.cs
│   └── PasswordEntriesController.cs
│
├── Data
│   └── ApplicationDbContext.cs
│
├── Migrations
│
├── Models
│   ├── ErrorViewModel.cs
│   └── PasswordEntry.cs
│
├── Utility
│
├── Views
│
├── wwwroot
│
├── Program.cs
├── appsettings.json
└── PasswordManager.csproj
```

The repository separates controllers, models, database access, migrations, utility functionality, views, and static assets into dedicated areas.

## Password Management

Each password entry contains information such as:

* Website
* Account owner
* Email address
* Generated password
* Creation date

These properties are represented by the `PasswordEntry` model.

### Password Generation

When a new password entry is created, the application automatically generates a password through the password-generation utility rather than requiring the user to manually create one.

### Password Hashing

Generated passwords are processed using **PBKDF2** with a randomly generated salt before being stored.

The implementation uses:

* 16-byte salt
* 32-byte derived hash
* 10,000 PBKDF2 iterations

The application also provides a verification method for validating a password against its stored hash.

## Password Entry Management

The `PasswordEntriesController` provides functionality for:

* Listing password entries
* Creating new entries
* Viewing entry details
* Detecting duplicate entries
* Revealing stored credentials through the application's verification workflow

Password entries are displayed using pagination, with 15 entries per page.

## Security Features

The project demonstrates several security-related concepts:

### Password Hashing

Passwords are hashed using PBKDF2 with a randomly generated salt before storage.

### Anti-Forgery Protection

The password creation endpoint uses ASP.NET Core's `[ValidateAntiForgeryToken]` attribute to help protect against cross-site request forgery attacks.

### Sensitive Data Masking

Password details are masked when initially displayed, and email addresses are partially obscured.

### Input Validation

The `PasswordEntry` model uses data annotations such as `[Required]` and `[EmailAddress]` to validate user input.

## Getting Started

### Prerequisites

Make sure you have the following installed:

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* Microsoft SQL Server
* Visual Studio 2022 or Visual Studio Code
* Git

### Clone the Repository

```bash
git clone https://github.com/BrianAhuga/PasswordManager.git
```

Navigate into the project:

```bash
cd PasswordManager
```

### Restore Dependencies

```bash
dotnet restore
```

## Configure the Database

Update the connection string in `appsettings.json` with your SQL Server configuration.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=PasswordManager;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Replace `YOUR_SERVER` with your SQL Server instance.

> **Security Note:** Never commit production database credentials or other sensitive configuration values to GitHub.

## Apply Database Migrations

The repository includes Entity Framework Core migrations for managing the database schema.

Run:

```bash
dotnet ef database update
```

If the Entity Framework CLI is not installed:

```bash
dotnet tool install --global dotnet-ef
```

## Run the Application

Start the application with:

```bash
dotnet run
```

Alternatively, open `PasswordManager.sln` in Visual Studio and run the application from there.

## Database

The application uses **Entity Framework Core** to communicate with SQL Server.

The database context is located in:

```text
Data/ApplicationDbContext.cs
```

Entity Framework Core migrations are maintained in:

```text
Migrations/
```

This allows database schema changes to be tracked and applied consistently.

## Learning Objectives

This project demonstrates practical concepts including:

* ASP.NET Core MVC
* .NET 8
* Entity Framework Core
* SQL Server
* Database migrations
* CRUD operations
* Password generation
* Password hashing
* PBKDF2
* Cryptographic salt generation
* Model validation
* Anti-forgery protection
* Pagination
* Razor Views
* MVC application architecture

## Security Disclaimer

This project should be considered a **learning and demonstration application**, not a production password manager.

A production-grade password manager would require additional security measures, including:

* Strong authenticated user accounts
* Authorization and access controls
* Secure encryption for recoverable passwords
* Encryption at rest
* Secure key management
* HTTPS enforcement
* Secrets management
* Multi-factor authentication
* Rate limiting
* Audit logging
* Secure password reveal mechanisms
* Protection against session attacks
* Security testing and auditing

In particular, passwords that users need to retrieve cannot simply be stored as one-way hashes. A production password vault would typically use strong encryption with carefully managed encryption keys.

## Future Improvements

Potential improvements include:

* User authentication and registration
* User-specific password vaults
* AES-based encryption for recoverable passwords
* Secure encryption-key management
* Password categories
* Password search
* Password strength analysis
* Password expiration reminders
* Password generator customization
* Two-factor authentication
* Role-based authorization
* Audit logs
* Secure password sharing
* Import and export functionality
* Password breach monitoring
* Improved responsive UI
* Automated security testing

## Author

**Brian Ahuga**

Software Engineer specializing in scalable software systems, secure backend services, modern web applications, and full-stack development.

GitHub: [BrianAhuga](https://github.com/BrianAhuga)

## License

This project is intended for learning, experimentation, and portfolio demonstration.
