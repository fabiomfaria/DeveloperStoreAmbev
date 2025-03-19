# Developer Evaluation Project - Sale API

## Project Description

This project implements a complete REST API for sales management following Domain-Driven Design (DDD) principles. The application provides a full CRUD for sales records, including information such as sale number, date, customer, total value, branch, products, quantities, unit prices, discounts, and sale status.

### Main Features

- Creation, reading, updating, and deletion of sales
- Automatic application of discounts based on item quantity
- Cancellation of complete sales or specific items
- Publication of business events (SaleCreated, SaleModified, SaleCancelled, ItemCancelled)
- Business rules validation
- Pagination, filtering, and sorting in query endpoints

### Business Rules

- Purchases above 4 identical items have a 10% discount
- Purchases between 10 and 20 identical items have a 20% discount
- It's not possible to sell more than 20 identical items
- Purchases below 4 items cannot have a discount

## Technologies Used

### Backend
- .NET 8.0
- C#
- Entity Framework Core
- MediatR
- AutoMapper
- Rebus (for event publishing)

### Database
- PostgreSQL (relational data)
- MongoDB (logs and events)

### Tests
- xUnit
- NSubstitute
- Bogus (Faker)

## Project Structure

The project follows a Clean Architecture with clear separation between layers:

```
src/
├── Ambev.DeveloperEvaluation.ORM/
├── Ambev.DeveloperEvaluation.MongoDB/
├── Ambev.DeveloperEvaluation.WebApi/
├── Ambev.DeveloperEvaluation.Application/ 
├── Ambev.DeveloperEvaluation.Domain/     
├── Ambev.DeveloperEvaluation.IoC/
└── Ambev.DeveloperEvaluation.Common/

tests/
├── Ambev.DeveloperEvaluation.Unit/ 
├── Ambev.DeveloperEvaluation.Integration/ 
└── Ambev.DeveloperEvaluation.Functional/ 
```

## How to Run the Project

### Prerequisites
- .NET SDK 8.0
- PostgreSQL
- MongoDB
- Docker (optional)

### Local Configuration

1. Clone the repository:
```
git clone https://github.com/your-username/ambev-developer-evaluation.git
cd ambev-developer-evaluation
```

2. Configure the connection strings in the `appsettings.json` file:
```json
{
  "ConnectionStrings": {
    "PostgreSQL": "Host=localhost;Database=ambev_sales;Username=postgres;Password=your_password",
    "MongoDB": "mongodb://localhost:27017/ambev_events"
  }
}
```

3. Execute the database migrations:
```
cd src/Ambev.DeveloperEvaluation.API
dotnet ef database update
```

4. Start the application:
```
dotnet run
```

The API will be available at `https://localhost:5001/swagger`

### Using Docker

Alternatively, you can use Docker to run the project:

```
docker-compose up -d
```

## How to Run the Tests

To run all tests:
```
dotnet test
```

To run a specific type of tests:
```
dotnet test tests/Ambev.DeveloperEvaluation.Unit
dotnet test tests/Ambev.DeveloperEvaluation.Integration
dotnet test tests/Ambev.DeveloperEvaluation.Functional
```

## API Endpoints

### Sales

- `GET /api/sales` - Gets list of sales with pagination, filtering, and sorting
- `GET /api/sales/{id}` - Gets details of a specific sale
- `POST /api/sales` - Creates a new sale
- `PUT /api/sales/{id}` - Updates an existing sale
- `DELETE /api/sales/{id}` - Cancels a sale
- `DELETE /api/sales/{saleId}/items/{itemId}` - Cancels a specific item from a sale

## Development Standards

This project follows patterns and practices such as:

- DDD (Domain-Driven Design)
- CQRS (Command Query Responsibility Segregation)
- Repository Pattern
- Mediator Pattern
- Unit of Work
- Clean Architecture
- GitFlow for version control
- Semantic commits
