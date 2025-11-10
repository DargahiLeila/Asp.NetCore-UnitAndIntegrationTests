# 🧠 ASP.NET Core CQRS Demo with Full Testing

This project demonstrates a manual implementation of the **CQRS (Command Query Responsibility Segregation)** pattern in ASP.NET Core MVC, built on a classic **N-Tier architecture** — now enhanced with **comprehensive unit and integration tests** across all layers.

---

## 🎯 Project Purpose

- Showcase CQRS without MediatR
- Apply clean separation of concerns using N-Tier architecture
- Demonstrate layered design with Domain, DAL, BLL, and UI,MyProject.Tests
- Provide full test coverage using xUnit and FluentAssertions
- Validate business logic and data access with real and mocked scenarios

---

## ⚙️ Technologies Used

- ASP.NET Core MVC
- Entity Framework Core (InMemory + SQL Server)
- xUnit
- FluentAssertions
- Moq
- Manual CQRS (Command/Query interfaces)
- N-Tier Architecture (UI, BLL, DAL, Domain)

---

## 🧪 Test Coverage

| Layer | Type of Test | Tools Used |
|-------|--------------|------------|
| DAL   | Integration  | EF Core InMemory |
| BLL   | Unit + Integration | xUnit, Moq |
| UI    | Unit + Integration| xUnit, Moq |
| Domain | No Tested | - |

### ✅ Covered Scenarios

- Add, Update, Delete users
- Activate/Deactivate users
- Get all users
- Search users by name
- Validate duplicate names


---

## 🚀 How to Run

1. Clone the repository:
   ```bash
   git clone https://github.com/DargahiLeila/Asp.NetCore-UnitAndIntegrationTests.git
  2.Open the solution file (.sln) in Visual Studio 2022 or later.

  3.Make sure your SQL Server instance is running.

  4.Create a SQL Server database manually named db_UnitTest.

  5.Create the required table using the following SQL script:

  CREATE TABLE [dbo].[Tbl_User] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(50),
    [IsDeleted] BIT NOT NULL
);

6.Update the connection string in appsettings.json:

"UnitTestConnectionString": "Data Source=Your-ServerName;Initial Catalog=db_UnitTest;TrustServerCertificate=True;User Id=*;Password=*;"


7.Run the project:
Press Ctrl + F5 or click Start Without Debugging

The browser will open and load the home page

Run the tests:

Open Test Explorer in Visual Studio

Click Run All Tests

📌 Notes
Integration tests use EF Core InMemory for isolation

Unit tests use Moq to simulate dependencies

All tests are written with xUnit and FluentAssertions

## 📂 Repository Structure

```
UnitTest/
├── Domain/              ← Domain models (currently not tested)
├── DAL/                 ← Data Access Layer
├── BLL/                 ← Business Logic Layer
├── UI/                  ← User Interface
├── MyProject.Tests/     ← Test project
│   ├── ApplicationTests/    ← Tests for business/application services
│   ├── DataAccessTests/     ← Tests for repositories and data access
│   └── UITests/             ← Tests for UI layer (controllers, views, interactions)
```

