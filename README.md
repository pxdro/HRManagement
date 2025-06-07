**HRManagement Application**

This repository contains the HRManagement full-stack application implemented with ASP.NET Core 7 and Angular 16.
It was developed as part of a technical assessment to demonstrate clean architecture, efficient data access, and client-side state management with NgRx.

---

## Table of Contents

1. [Overview](#overview)
2. [Tech Stack](#tech-stack)
3. [Architecture](#architecture)
4. [Prerequisites](#prerequisites)
5. [Getting Started](#getting-started)

   * [Backend Setup](#backend-setup)
   * [Frontend Setup](#frontend-setup)
6. [Database Migrations & Seeding](#database-migrations--seeding)
7. [Running the Application](#running-the-application)
8. [API Documentation](#api-documentation)
9. [Testing](#testing)
10. [Feature Highlights](#feature-highlights)
11. [Assumptions & Future Improvements](#assumptions--future-improvements)

---

## Overview

This application provides CRUD operations for managing Departments and Employees. It implements:

* A RESTful API with EF Core, Repository & Unit-of-Work patterns
* FluentValidation-based input validation and global error handling middleware
* Optimistic concurrency via EF Core `[Timestamp]` tokens
* Angular 16 frontend with NgRx state management, entity adapters, and reactive forms

---

## Tech Stack

* **Backend**: .NET 7, ASP.NET Core 7.0, Entity Framework Core 7.0
* **Database**: SQL Server 2022
* **Frontend**: Angular 16, NgRx 16
* **Testing**: xUnit for unit tests, Microsoft.AspNetCore.Mvc.Testing for integration tests

---

## Architecture

**Backend Layers**:

* **Domain**: Entities and business rules
* **Application**: DTOs and service interfaces
* **Infrastructure**: EF Core `AppDbContext`, Repositories, Unit of Work, Services
* **API**: Controllers, Middleware, DTO mapping

**Frontend Structure**:

* **Core**: Guards, Interceptors, Shared services
* **Feature Modules**: Departments module with components, state (`actions`, `reducers`, `effects`, `selectors`)
* **State Management**: NgRx Entity Adapters for normalization

---

## Prerequisites

* .NET 7 SDK
* Node.js 18+ and npm
* SQL Server 2022 instance

---

## Getting Started

### Backend Setup

1. Clone the repository:

   ```bash
   git clone https://github.com/pxdro/HRManagement.git
   cd HRManagement/backend/HRManagement.Api
   ```
2. Install dependencies and apply migrations:

   ```bash
   dotnet restore
   dotnet ef database update
   ```
3. Configure connection string in `appsettings.json`:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.;Database=HRManagement;Trusted_Connection=True;"
   }
   ```

### Frontend Setup

1. Navigate to the UI project:

   ```bash
   cd ../../frontend/HRManagement.UI
   ```
2. Install npm packages:

   ```bash
   npm install
   ```
3. Update API base URL in `environment.ts` if needed.

---

## Database Migrations & Seeding

* Migrations are located under `HRManagement.Infrastructure/Migrations`.
* To add a new migration:

  ```bash
  dotnet ef migrations add <MigrationName>
  dotnet ef database update
  ```

---

## Running the Application

* **Backend**:

  ```bash
  dotnet run --project HRManagement.Api
  ```
* **Frontend**:

  ```bash
  ng serve --open
  ```

---

## API Documentation

* Swagger is available at `https://localhost:<port>/swagger`.

* Endpoints:

  * `GET /departments`
  * `GET /departments/{id}`
  * `POST /departments`
  * `PUT /departments/{id}`
  * `DELETE /departments/{id}`

* Similar routes exist for `employees`.

---

## Testing

* **Unit Tests**:

  ```bash
  dotnet test HRManagement.Tests/HRManagement.Tests.Domain
  ```
* **Integration Tests**:

  ```bash
  dotnet test HRManagement.Tests/HRManagement.Tests.Integration
  ```

---

## Feature Highlights

* **Global Error Handling**: Custom middleware returns consistent error responses
* **Optimistic Concurrency**: `[Timestamp] RowVersion` properties and 409 Conflict handling
* **Eager Loading**: Repository methods support `include` lambdas for related data
* **NgRx Entity**: State normalization, caching, and reactive forms with real-time validation

---

## Assumptions & Future Improvements

* **Assumptions**:

  * Connection string uses Windows Authentication
  * No external authentication currently (JWT ready in interceptor)

* **Future Improvements**:

  * Integrate Serilog for structured logging
  * Implement client-side caching strategy or `ngrx-data`
  * Add more comprehensive Swagger XML comments
  * Expand optimistic-concurrency to bulk updates
  * Add UI theming and responsive layouts using Angular Material

---

Thank you for reviewing this assessment submission! Feel free to reach out for any clarifications.
