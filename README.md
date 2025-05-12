# Velocity Rental (Car Rental API)
Scope: MVP

![.NET Build & Test & Deploy](https://github.com/bgaprogrammer/velocityrental/actions/workflows/dotnet.yml/badge.svg)

## Project Description
VelocityRental is an API built with .NET 9, following a Cloud Native architecture and ready for deployment on any cloud provider supporting .NET 9.
The API enables car rental and return operations, and calculates rental prices and customer loyalty points based on flexible business rules.

## Technologies Used

### Platform & Language
- **NET 9**: Latest Microsoft framework with cloud native features and performance improvements
- **C# 14**: Leveraging the latest language features for clean and efficient code

### Database
- **SQLite**: Temporary solution for this MVP.

### Tools & Frameworks
- **Visual Studio 2022**: Main IDE for development
- **Entity Framework Core 9.0**: ORM for data access layer
- **Swagger/OpenAPI**: Automatic API documentation and testing

### Testing
- **xUnit**: Unit testing framework
- **Moq**: Dependency mocking
- **FluentAssertions**: Readable assertions for tests


### Patterns & Architecture
- **Clean Architecture**: Clear separation of concerns
- **Repository Pattern**: Data access abstraction
- **Domain-Driven Design**: Focused on domain modeling

---

## Quick Start: How to Test the API

1. **Run the API**
   - Start the project (e.g., with `dotnet run` or from Visual Studio).
   - Open the Swagger UI (usually at `https://localhost:8080/swagger`).

2. **Initialize Master Data**
   - Go to the `MasterData` section in Swagger.
   - Use the `POST /api/MasterData/initialize` endpoint to create demo cars, pricing, and customers.
   - You will see a confirmation message when initialization is complete.

3. **Check Master Data Status (Optional)**
   - Use `GET /api/MasterData/status` to verify if the database is initialized.

4. **Rent a Car**
   - Go to the `Rental` section in Swagger.
   - Use `POST /api/Rental` to create a new rental. Provide a valid customer and car ID (from the initialized data).
   - On success, you'll get the rental details and a unique rental ID.

5. **Return a Car**
   - Use `POST /api/Rental/{id}/return` with the rental ID from the previous step.
   - You can provide a return date if you want to simulate a specific return scenario; if you do not provide a date, today's date will be used by default. This is important for testing late returns or other behaviors.
   - On success, you'll get the updated rental with return info and any fees.

6. **(Optional) Clean All Data**
   - Use `POST /api/MasterData/clean` to delete all data and reset the system.

**Tip:**
- You can always re-initialize the master data after cleaning.
- All endpoints can be tested directly from Swagger UI.

---

## Possible Future Improvements

- **Loyalty Points Tracking History:**
  Currently, loyalty points are simply accumulated as an integer field for each customer, without tracking the specific rental from which each set of points was earned. In future iterations, a dedicated table could be introduced to record a history of loyalty points awarded per rental. This would allow the system to trace the origin of each set of loyalty points, provide detailed breakdowns to users, and enable more advanced reporting or loyalty program features. The final balance could then be calculated as the sum of all historical entries, offering full transparency and auditability.

- **Additional Fees Extensibility:**
  The system currently supports only the most important additional fee type (late return) for the MVP, as represented by the `FeeTypeEnum` and the `AdditionalFees` list in the `Rental` entity. Other fee types (such as baby seat, GPS, cross-border, etc.) might be part of future extensibility but are not yet implemented. In future iterations, the enum and related logic can be expanded to support a wider range of additional fees, enabling more flexible pricing and richer rental scenarios.

Miscellaneous:

- Migration to NoSQL storage.
- Distributed anti-collision/locking (e.g., Redis)
- Integration tests and E2E scenarios
- Advanced monitoring and logging
- Security, authentication, and authorization
- More granular validation and error handling
- More unit tests :)

---
