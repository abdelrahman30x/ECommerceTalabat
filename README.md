# ECommerceTalabat

A full-stack e-commerce platform built with .NET backend and Angular frontend, providing a complete online shopping experience with authentication, product management, shopping cart, orders, and payment processing.

## 🏗️ Architecture

This project follows a **Clean Architecture** pattern with clear separation of concerns:

- **Backend**: .NET 9.0 Web API
- **Frontend**: Angular (in `client` folder)
- **Database**: SQL Server
- **Cache & Storage**: Redis (for API response caching and basket storage)
- **Payment**: Stripe Integration

## 📋 Table of Contents

- [Features](#features)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Configuration](#configuration)
- [Database Setup](#database-setup)

## ✨ Features

### 🔐 Authentication & Authorization

- **User Registration**: Complete user registration with email, username, and profile information
- **Login/Logout**: Secure authentication using JWT tokens
- **Password Management**:
  - Forgot Password
  - Reset Password
  - Change Password
- **Email Confirmation**: Email verification system
- **Profile Management**: Update user profile information
- **Account Management**: Delete user account with password verification
- **Validation**: Check email and username availability
- **Address Management**: Get and update user shipping addresses
- **Token Validation**: Validate JWT tokens

### 🛍️ Product Management

- **Product Listing**: Get all products with advanced filtering and pagination
- **Product Details**: Get product by ID
- **Product Filtering**: Filter by brand, type, and search terms
- **Product Sorting**: Multiple sorting options (price, name, etc.)
- **Brands & Types**: Get all available brands and product types
- **Redis Response Caching**: API responses (products, brands, types) are cached in Redis using `RedisCacheAttribute` for improved performance and reduced database load

### 🛒 Shopping Cart (Basket)

- **Get Basket**: Retrieve user's shopping cart by basket ID
- **Create/Update Basket**: Add or update items in the shopping cart
- **Delete Basket**: Remove shopping cart
- **Redis Basket Storage**: Shopping cart is stored in Redis (not in SQL database) for fast access and scalability
- **Persistent Cart**: Cart persists for 30 days in Redis

### 📦 Order Management

- **Create Order**: Place orders with delivery address and method
- **Get User Orders**: Retrieve all orders for authenticated user
- **Get Order Details**: Get specific order by ID
- **Delivery Methods**: Get available delivery methods
- **Order History**: Complete order tracking

### 💳 Payment Processing

- **Stripe Integration**: Create payment intents for secure transactions
- **Payment Intent**: Generate payment intents for basket checkout

### 🔧 Technical Features

- **Custom Exception Middleware**: Centralized error handling
- **AutoMapper**: Object-to-object mapping
- **Unit of Work Pattern**: Transaction management
- **Repository Pattern**: Data access abstraction
- **Specification Pattern**: Flexible query building
- **Redis Caching Strategy**:
  - Response caching for product endpoints using `RedisCacheAttribute`
  - Basket repository using Redis for shopping cart persistence
  - Configurable cache expiration times
- **CORS Configuration**: Cross-origin resource sharing
- **Model Validation**: Comprehensive input validation
- **Logging**: Structured logging throughout the application

## 🛠️ Technology Stack

### Backend

- **.NET 9.0**: Core framework
- **ASP.NET Core Web API**: RESTful API
- **Entity Framework Core**: ORM for database operations
- **SQL Server**: Primary database for products, orders, and user data
- **Redis**:
  - API response caching (products, brands, types) with configurable TTL
  - Basket/shopping cart storage
- **JWT Authentication**: Token-based authentication
- **AutoMapper**: Object mapping
- **Stripe**: Payment processing
- **Identity Framework**: User management

### Frontend

- **Angular**: Frontend framework (in `client` folder)

## 📁 Project Structure

```text
ECommerceTalabat/
├── Core/
│   ├── ECommerceG02.Abstractions/     # Service interfaces
│   ├── ECommerceG02.Domain/            # Domain models and entities
│   └── ECommerceG02.Services/         # Business logic services
├── Infrastructure/
│   ├── ECommerceG02.Persistence/      # Data access layer
│   └── ECommerceG02.Presentation/     # API controllers
├── ECommerceG02.Shared/                # Shared DTOs and common models
├── ECommerceG02.Web/                   # Web API startup and configuration
└── client/                             # Angular frontend application
```

## 🚀 Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB or SQL Server Express)
- Redis Server
- Node.js and npm (for Angular frontend)
- Visual Studio 2022 or VS Code

### Installation

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd ECommerceTalabat
   ```

2. **Configure Database Connection**

   - Update `appsettings.json` with your SQL Server connection string
   - Update Redis connection string

3. **Run Database Migrations**

   ```bash
   cd ECommerceG02.Web
   dotnet ef database update
   ```

4. **Run the Backend**

   ```bash
   cd ECommerceG02.Web
   dotnet run
   ```

5. **Run the Frontend** (in a separate terminal)
   ```bash
   cd client
   npm install
   ng serve
   ```

## 📡 API Endpoints

### Authentication (`/api/Auth`)

| Method | Endpoint                    | Description                 | Auth Required |
| ------ | --------------------------- | --------------------------- | ------------- |
| POST   | `/api/Auth/register`        | Register new user           | No            |
| POST   | `/api/Auth/login`           | User login                  | No            |
| POST   | `/api/Auth/logout`          | User logout                 | Yes           |
| GET    | `/api/Auth/me`              | Get current user            | Yes           |
| POST   | `/api/Auth/confirm-email`   | Confirm email address       | No            |
| POST   | `/api/Auth/forgot-password` | Request password reset      | No            |
| POST   | `/api/Auth/reset-password`  | Reset password              | No            |
| POST   | `/api/Auth/change-password` | Change password             | Yes           |
| PUT    | `/api/Auth/profile`         | Update user profile         | Yes           |
| DELETE | `/api/Auth/account`         | Delete user account         | Yes           |
| GET    | `/api/Auth/check-email`     | Check email availability    | No            |
| GET    | `/api/Auth/check-username`  | Check username availability | No            |
| POST   | `/api/Auth/validate-token`  | Validate JWT token          | No            |
| GET    | `/api/Auth/Address`         | Get user address            | Yes           |
| PUT    | `/api/Auth/Address`         | Update user address         | Yes           |

### Products (`/api/Products`)

| Method | Endpoint               | Description                                            | Auth Required |
| ------ | ---------------------- | ------------------------------------------------------ | ------------- |
| GET    | `/api/Products`        | Get all products (with pagination, filtering, sorting) | No            |
| GET    | `/api/Products/{id}`   | Get product by ID                                      | No            |
| GET    | `/api/Products/Brands` | Get all brands                                         | No            |
| GET    | `/api/Products/Types`  | Get all product types                                  | No            |

**Query Parameters for Products:**

- `pageIndex`: Page number (default: 1)
- `pageSize`: Items per page (default: 6)
- `SortingOption`: Sort option (1-4)
- `SearchValue`: Search term
- `BrandId`: Filter by brand
- `TypeId`: Filter by type

### Basket (`/api/Baskets`)

| Method | Endpoint                      | Description             | Auth Required |
| ------ | ----------------------------- | ----------------------- | ------------- |
| GET    | `/api/Baskets?key={basketId}` | Get basket by ID        | No            |
| POST   | `/api/Baskets`                | Create or update basket | No            |
| DELETE | `/api/Baskets/{id}`           | Delete basket           | No            |

### Orders (`/api/Orders`)

| Method | Endpoint                      | Description          | Auth Required |
| ------ | ----------------------------- | -------------------- | ------------- |
| GET    | `/api/Orders`                 | Get user orders      | Yes           |
| GET    | `/api/Orders/{id}`            | Get order by ID      | Yes           |
| POST   | `/api/Orders`                 | Create new order     | Yes           |
| GET    | `/api/Orders/deliveryMethods` | Get delivery methods | Yes           |

### Payments (`/api/Payments`)

| Method | Endpoint                   | Description           | Auth Required |
| ------ | -------------------------- | --------------------- | ------------- |
| POST   | `/api/Payments/{basketId}` | Create payment intent | Yes           |

## ⚙️ Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ECommerceDb;Trusted_Connection=true;TrustServerCertificate=true",
    "RedisConnection": "localhost",
    "IdentityConnection": "Server=.;Database=ECommerceIdentityDb;Trusted_Connection=true;TrustServerCertificate=true"
  },
  "Jwt": {
    "SecretKey": "YOUR_SECRET_KEY_HERE",
    "Issuer": "ECommerce",
    "Audience": "ECommerceUsers",
    "ExpirationMinutes": 300
  },
  "StripeSettings": {
    "SecretKey": "YOUR_STRIPE_SECRET_KEY",
    "PublishableKey": "YOUR_STRIPE_PUBLISHABLE_KEY"
  }
}
```

## 🗄️ Database Setup

The application uses two databases:

1. **ECommerceDb**: Main database for products, orders, and business data
2. **ECommerceIdentityDb**: Identity database for user authentication

### Running Migrations

```bash
# Navigate to the Web project
cd ECommerceG02.Web

# Apply migrations
dotnet ef database update
```

## 🔒 Security Features

- JWT-based authentication
- Input validation and sanitization
- Token expiration and validation

## 📝 Notes

- The frontend Angular application is located in the `client` folder
- **Redis is required** for:
  - Basket/shopping cart storage (baskets are stored in Redis, not SQL Server)
  - API response caching (product listings, brands, types are cached to reduce database queries)
- Ensure SQL Server is running before starting the application
- Ensure Redis Server is running before starting the application
- Configure Stripe keys in `appsettings.json` for payment functionality
- Default admin credentials can be found in the database seed data

## 📸 Screenshots
<img width="1884" height="857" alt="image" src="https://github.com/user-attachments/assets/7a2b6597-1f98-417d-bde7-c51be4b070bc" />

<img width="1917" height="1018" alt="image" src="https://github.com/user-attachments/assets/6d3ca518-cefb-4fd0-8ca2-4e8b7cb42764" />


<img width="1917" height="1022" alt="image" src="https://github.com/user-attachments/assets/d24b4a9f-d391-40f8-a586-1b3b6ac6a49b" />

---

**Built with ❤️ using .NET and Angular**
