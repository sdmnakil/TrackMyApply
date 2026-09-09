# TrackMyApply

TrackMyApply is a full-stack Job Application Tracking System that helps users manage, organize, search, filter, and monitor their job applications from a centralized platform.

## Features

- User registration and login with JWT authentication
- Create, view, update, and delete job applications
- Search and filter applications by company and status
- Pagination for job application listings
- User-specific data isolation
- Dashboard with application statistics
- Global exception handling
- RESTful Web API architecture

## Backend Architecture

The backend is built with ASP.NET Core Web API using a layered architecture:

- Controllers – Handle HTTP requests and API endpoints
- Services – Contain business logic
- Repositories – Handle database operations
- DTOs – Control data transfer between client and API
- Middleware – Provides global exception handling
- Entity Framework Core – ORM for database operations
- JWT Authentication – Secures protected API endpoints

## Design Patterns & Principles

- Repository Pattern
- Service Layer Pattern
- Dependency Injection
- DTO Pattern
- Layered Architecture

## Technology Stack

### Backend
- C#
- ASP.NET Core Web API
- Entity Framework Core
- MySQL
- JWT Authentication

### Frontend
- HTML
- CSS
- JavaScript

### Tools & Deployment
- Docker
- Git & GitHub
- Swagger / OpenAPI

## Project Structure

```text
TrackMyApply/
├── Auth/
├── Controllers/
├── DTOs/
├── Data/
├── Exceptions/
├── Middleware/
├── Migrations/
├── Models/
├── Repositories/
├── Services/
└── JobApplicationTrackerFrontend/
