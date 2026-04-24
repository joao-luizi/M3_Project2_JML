*This project has been created as part of the upskill .net curriculum by [@joao-luizi](https://github.com/joao-luizi), [@llm-Lucas](https://github.com/llm-Lucas),  [@maialuis87-bit](https://github.com/maialuis87-bit).*

This is a Web API project; to test it, use Swagger at [https://localhost:44344/swagger](https://localhost:44344/swagger).

Ensure IIS (or IIS Express) is enabled and used to run the application.

---

## Overview

This project consists of a Web API developed to support the management of a library system (XPTO Library), which includes a central unit and multiple regional branches.

The system is designed to handle:
- Management of works (books) and their copies
- User and reader registration and authentication
- Loan and return operations
- Transfers of copies between library branches
- Reader status tracking and suspension rules
- Basic statistical analysis to support decision-making

The API is intended to serve both:
- Administrative operations (internal users)
- Reader-facing operations (library users)

---

## Functional Highlights

- Register and authenticate internal users and readers
- CRUD operations for works (books)
- Manage and distribute copies across multiple branches
- Loan system with constraints:
  - Maximum of 4 active loans per reader
  - Maximum loan period of 15 days
- Automatic handling of:
  - Late returns and reader suspension
  - Reader reactivation
- Transfer of copies between branches (ensuring minimum availability)
- Reader features:
  - Search available works
  - View current loans with status indicators:
    - **ATRASO** (late)
    - **Devolução URGENTE** (less than 3 days remaining)
    - **Devolver em breve** (less than 5 days remaining)
  - View loan history
- Optional statistical data for analysis and decision support

---

## Installation

You need to have SQL Server installed.

In the solution root, you will find a `DBTools` folder containing a script that creates the database with all required objects and seeds it with initial data.

This will create a database called `M3_ProjectII`, provided one does not already exist.

### Steps

1. Clone the repository
2. Open the solution in Visual Studio
3. Execute the SQL script located in:
`/DBTools`
4. Confirm the connection string in `appsettings.json` matches your SQL Server instance
5. Run the project using **IIS Express** (or IIS)

---

## Running the Application

- Start the project using IIS Express
- Open your browser at:
- https://localhost:44344/swagger
- Use Swagger UI to explore and test the available endpoints

---

## Notes

- This project is API-only (no frontend included)
- Swagger is the primary interface for interacting with the system
- The database must be initialized before running the application
- The project assumes a local development environment using IIS Express

---

## Tech Stack

- .NET Web API
- SQL Server
- ADO.NET / Repository pattern (based on project structure)
- Swagger (OpenAPI)

---

## Academic Context

This project was developed as part of the **Upskill .NET Program**, focusing on backend development, architecture, and data management practices in a real-world inspired scenario.