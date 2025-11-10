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
| UI    | Unit + Integration| xUnit |
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
