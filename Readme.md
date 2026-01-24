# Specialty Coffee Shop – Backend

Try it here: https://specialty-coffee-shop-evcqgzfygydrajaj.westeurope-01.azurewebsites.net/

ASP.NET Core backend for the Specialty Coffee Shop project.  
This application provides a REST API for product catalog management, cart calculation, checkout, and admin authentication, and serves an Angular single-page application from `wwwroot` in production.

The backend is designed to demonstrate clean API design, practical authentication, database integration, and real-world deployment readiness.

Front-end is compiled, source code can be found at: https://github.com/kuninsoft/specialty-coffee-shop-frontend

---

## Overview

The backend acts as the core of the application’s business logic. It exposes endpoints for browsing products, managing cart calculations, placing orders, and administering the catalog. Authentication is handled via secure cookies, keeping the client lightweight while ensuring protected admin access.

The project is structured to resemble a production-ready application rather than a demo, with clear separation between API, domain logic, and persistence.

---

## Features

- Product catalog API (list, details, stock, pricing, discounts)
- Cart calculation logic (subtotal, discounts, total)
- Checkout endpoint with shipping information
- Admin authentication using cookies
- Admin product management (create, delete)
- Image upload support via `multipart/form-data`
- Serves Angular frontend from `wwwroot` in production

---

## Tech Stack

- ASP.NET Core (.NET)
- Entity Framework Core
- SQL Server (Azure SQL compatible)
- Cookie-based authentication
- RESTful API design

---

## Architecture Overview

- **Controllers**  
  Handle HTTP requests and responses, keeping endpoints thin and focused.

- **Services**  
  Contain business logic such as cart calculation, authentication checks, and product management.

- **Data Access**  
  Entity Framework Core is used for database access and migrations, with clear separation from controllers.

- **DTOs**  
  Explicit request and response models are used to keep API contracts clean and stable.

- **Authentication**  
  Admin access is protected using cookie-based authentication. A successful login is validated server-side without exposing user data to the client.

- **Frontend Integration**  
  The Angular application is served from `wwwroot`, with SPA routing fallback configured to support client-side navigation.

---

## Deployment Notes

The backend is designed to run on Azure App Service and connect to Azure SQL Database using environment-based configuration. Connection strings and environment variables are supplied via application settings rather than hardcoded configuration files.

---

## Purpose

This project was built as a portfolio-quality application to demonstrate practical backend development skills, including API design, authentication, database integration, and cloud deployment readiness.
