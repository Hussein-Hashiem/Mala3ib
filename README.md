# Mala3ib API

A sports-field booking platform backend built with **ASP.NET Core 10**. The system allows players to discover and book football/sports fields, field owners to manage their venues and time slots, and administrators to oversee the whole platform.

> **This is a college project**, developed as a practical application of backend engineering concepts including layered architecture, authentication, and RESTful API design.

---

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Domain Model](#domain-model)
- [API Endpoints & Role Access](#api-endpoints--role-access)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Background Jobs](#background-jobs)
- [Team](#team)

---

## Features

- **JWT Authentication** with refresh token rotation and revocation
- **Role-based Authorization** — three distinct roles: `Player`, `FieldOwner`, `Admin`
- **Email Verification** via OTP on registration
- **Password Reset** via OTP email flow
- **Field Management** — CRUD for sports fields with image uploads
- **Time-Slot System** — field owners define bookable slots; players book them
- **Invitation System** — players invite friends to a slot, or request to join an existing slot
- **Social Follow** — players follow each other
- **Field Reviews** — players rate and review fields
- **Field Owner Dashboard** — owners see their fields, bookings, and invitations in one place
- **Admin Panel** — admins approve/reject field owners and view all bookings
- **Distributed In-Memory Caching**
- **Background Jobs** via Hangfire with a dedicated dashboard at `/Jobs`
- **Global Exception Handler** with RFC 7807 Problem Details responses
- **FluentValidation** auto-wired to all controllers
- **Swagger / OpenAPI** UI at `/swagger`

---

## Architecture

The solution follows a **3-Layer Architecture** separated into three projects:

```
Mala3ib.sln
├── Mala3ib.API      → Presentation layer (Controllers, DI setup, Middleware)
├── Mala3ib.BLL      → Business Logic layer (Services, DTOs, Validators, Mapping)
└── Mala3ib.DAL      → Data Access layer (EF Core, Entities, Repos, Enums)
```

Dependency direction: `API → BLL → DAL`

Key patterns in use:

- **Repository Pattern** — each entity has an interface + implementation under `Repo/`
- **Service Pattern** — business logic lives in `Service/` and is abstracted behind interfaces
- **Result Pattern** — operations return a `Result<T>` wrapping either a value or an `Error`, avoiding exception-driven control flow
- **DTO / Mapping** — Mapster handles all entity-to-DTO conversions, configured in BLL
- **FluentValidation** — every request DTO has a dedicated validator; auto-validation fires before controllers execute

---

## Tech Stack

| Area | Library / Version |
|---|---|
| Runtime | .NET 10 |
| Web Framework | ASP.NET Core 10 Web API |
| ORM | Entity Framework Core 10 |
| Database | SQL Server |
| Auth | ASP.NET Identity + JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| Mapping | Mapster 10 |
| Validation | FluentValidation 10 + `SharpGrip.FluentValidation.AutoValidation.Mvc` |
| Email | MailKit 4 |
| Background Jobs | Hangfire 1.8 (SQL Server storage) |
| API Docs | Swashbuckle (Swagger) 10 |
| Dynamic LINQ | `System.Linq.Dynamic.Core` |

---

## Project Structure

```
Mala3ib.API/
├── Controllers/          # One controller per domain entity
├── Extensions/           # Result → HTTP response helpers
├── Templates/            # HTML email templates (confirmation, password reset)
├── wwwroot/images/       # Uploaded field images (served as static files)
├── DependencyInjection.cs
├── GlobaExceptionHandler.cs
└── Program.cs

Mala3ib.BLL/
├── Authentication/       # IJwtProvider + JwtProvider + JwtOptions
├── Contracts/            # Request/Response DTOs grouped by domain
├── Errors/               # Typed error definitions per domain
├── Helpers/              # EmailBodyBuilder
├── Service/
│   ├── Abstraction/      # Service interfaces
│   └── Implementation/   # Service implementations
├── Settings/             # MailSettings, FileSettings
└── Validations/          # FluentValidation validators (mirroring Contracts structure)

Mala3ib.DAL/
├── Abstraction/          # Result<T>, Error, PaginatedList, DefaultRoles, DefaultUsers
├── Database/
│   ├── ApplicationDbContext.cs
│   └── EntitiesConfigurations/   # IEntityTypeConfiguration per entity
├── Entities/             # Domain entity classes
├── Enums/                # All enum types
└── Repo/
    ├── Abstraction/      # Repository interfaces
    └── Implementation/   # Repository implementations
```

---

## Domain Model

### Entities

| Entity | Description |
|---|---|
| `ApplicationUser` | Extends `IdentityUser` — holds `FirstName`, `LastName`, `Image`, refresh tokens, OTPs, and follow relationships |
| `Player` | A user who books fields and sends invitations; linked 1-to-1 with `ApplicationUser` |
| `FieldOwner` | A user who owns fields; requires admin approval (`Status`) |
| `Admin` | Platform administrator |
| `Field` | A sports field with `Name`, `Location`, `PricePerHour`, soft-delete, images, slots, and reviews |
| `FieldImage` | Images attached to a `Field` |
| `FieldSlot` | A bookable time window on a field (`StartDate`, `EndDate`, `MaxPlayers`, `Price`) |
| `FieldSlotPlayer` | Join table — tracks which players are in a slot |
| `Booking` | A player's confirmed reservation of a `FieldSlot` |
| `Invitation` | A player inviting another player to a slot, or requesting to join |
| `FieldReview` | A rating/review left by a player on a field |
| `Follow` | Directional follow relationship between two `ApplicationUser`s |
| `RefreshToken` | JWT refresh token owned by a user |
| `EmailVerficationOtp` | OTP record for email confirmation or password reset |

### Enums

| Enum | Values |
|---|---|
| `BookingStatus` | `Pending`, `Confirmed`, `Cancelled`, `Rejected` |
| `InvitationStatus` | `Pending`, `Accepted`, `Rejected` |
| `InvitationType` | `Invite`, `Request` |
| `Status` (FieldOwner / Field) | `Pending`, `Approved`, `Rejected` |
| `OtpType` | (email confirmation, password reset) |
| `PaymentMethod` | (defined, not yet wired to a flow) |
| `PaymentStatus` | (defined, not yet wired to a flow) |

> **Soft-delete** is implemented on all major entities (`IsDeleted` flag) — records are never physically removed.

---

## API Endpoints & Role Access

> Legend: **Public** = no auth required · **Auth** = any logged-in user · **Player / FieldOwner / Admin** = role-restricted

### `POST /Auth` — Authentication

| Method | Endpoint | Access |
|---|---|---|
| POST | `/Auth` | Public |
| POST | `/Auth/refresh` | Public |
| PUT | `/Auth/revoke-refresh-token` | Public |
| POST | `/Auth/player-register` | Public |
| POST | `/Auth/field-owner-register` | Public |
| POST | `/Auth/confirm-email` | Public |
| POST | `/Auth/resend-confirmation-email` | Public |
| POST | `/Auth/forget-password` | Public |
| POST | `/Auth/verify-reset-password-otp` | Public |
| POST | `/Auth/reset-password` | Public |

### `GET|PUT|DELETE /Account` — Account Management

| Method | Endpoint | Access |
|---|---|---|
| GET | `/Account/player/{userId}` | Auth |
| GET | `/Account/field-owner` | FieldOwner |
| PUT | `/Account/player` | Player |
| PUT | `/Account/player/change-password` | Player |
| DELETE | `/Account/player` | Player |
| PUT | `/Account/field-owner` | FieldOwner |
| PUT | `/Account/field-owner/change-password` | FieldOwner |
| DELETE | `/Account/field-owner` | FieldOwner |

### `/Field` — Fields

| Method | Endpoint | Access |
|---|---|---|
| GET | `/Field/get-all` | Auth |
| GET | `/Field/get-by-id/{id}` | Auth |
| GET | `/Field/get-by-owner-id/{ownerId}` | Auth |
| POST | `/Field` | FieldOwner |
| PUT | `/Field/{id}` | FieldOwner |
| DELETE | `/Field/{id}` | FieldOwner |

### `/FieldSlot` — Field Time Slots

| Method | Endpoint | Access |
|---|---|---|
| GET | `/FieldSlot/{id}` | Auth |
| GET | `/FieldSlot/field/{fieldId}` | Auth |
| GET | `/FieldSlot/field/{fieldId}/avialable-slots` | Auth |
| POST | `/FieldSlot/{fieldId}` | FieldOwner |
| PUT | `/FieldSlot/{fieldId}/slot/{id}` | FieldOwner |
| DELETE | `/FieldSlot/{fieldId}/slot/{id}` | FieldOwner |

### `/Booking` — Bookings

| Method | Endpoint | Access |
|---|---|---|
| GET | `/Booking/{bookingId}` | Player |
| GET | `/Booking/my` | Player |
| POST | `/Booking/{fieldSlotId}` | Player |
| DELETE | `/Booking/{bookingId}` | Player |

### `/Invitation` — Invitations

| Method | Endpoint | Access |
|---|---|---|
| GET | `/Invitation/received` | Player |
| GET | `/Invitation/sent` | Player |
| POST | `/Invitation/Invite` | Player |
| POST | `/Invitation/Request/{fieldSlotId}` | Player |
| POST | `/Invitation/Accept/{invitationId}` | Player |
| POST | `/Invitation/Reject/{invitationId}` | Player |
| DELETE | `/Invitation/{id}` | Player |

### `/FieldReview` — Reviews

| Method | Endpoint | Access |
|---|---|---|
| GET | `/FieldReview/{reviewId}` | Auth |
| GET | `/FieldReview/{fieldId}/reviews` | Auth |
| POST | `/FieldReview/{fieldId}` | Player |
| PUT | `/FieldReview/{reviewId}` | Player |
| DELETE | `/FieldReview/{reviewId}` | Player or Admin |

### `/Follow` — Social Follow

| Method | Endpoint | Access |
|---|---|---|
| GET | `/Follow/{userId}/followers` | Auth |
| GET | `/Follow/{userId}/following` | Auth |
| POST | `/Follow` | Auth |
| DELETE | `/Follow` | Auth |

### `/FieldOwner` — Field Owner Dashboard

| Method | Endpoint | Access |
|---|---|---|
| GET | `/FieldOwner/fields` | FieldOwner |
| GET | `/FieldOwner/bookings` | FieldOwner |
| GET | `/FieldOwner/invitations` | FieldOwner |

### `/Admin` — Admin Panel

| Method | Endpoint | Access |
|---|---|---|
| GET | `/Admin/bookings` | Admin |
| GET | `/Admin/invitations` | Admin |
| GET | `/Admin/field-owners` | Admin |
| PUT | `/Admin/field-owners/{userId}/status` | Admin |

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (local or remote)

### Clone & Run

```bash
git clone https://github.com/Hussein-Hashiem/Mala3ib.git
cd Mala3ib
```

Copy the secrets template and fill in your values (see [Configuration](#configuration)):

```bash
# Windows (PowerShell)
copy Mala3ib.API\appsettings.json Mala3ib.API\appsettings.Local.json

# macOS / Linux
cp Mala3ib.API/appsettings.json Mala3ib.API/appsettings.Local.json
```

Apply database migrations:

```bash
cd Mala3ib.API
dotnet ef database update --project ../Mala3ib.DAL
```

Run the API:

```bash
dotnet run --project Mala3ib.API
```

The API will be available at `https://localhost:{port}`.
Swagger UI is at `https://localhost:{port}/swagger`.
Hangfire dashboard is at `https://localhost:{port}/Jobs`.

---

## Configuration

`appsettings.json` / `appsettings.Local.json` (gitignored) expects:

```json
{
  "ConnectionStrings": {
    "defaultConnection": "Server=.;Database=Mala3ib;Trusted_Connection=True;TrustServerCertificate=True;",
    "HangfireConnection": "Server=.;Database=Mala3ib_Hangfire;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtOptions": {
    "Key": "<your-secret-key-min-32-chars>",
    "Issuer": "Mala3ib",
    "Audience": "Mala3ib-Users",
    "ExpiryMinutes": 60
  },
  "MailSettings": {
    "Host": "smtp.example.com",
    "Port": 587,
    "DisplayName": "Mala3ib",
    "From": "noreply@mala3ib.com",
    "UserName": "your-smtp-username",
    "Password": "your-smtp-password"
  },
  "FileSettings": {
    "MaxFileSizeInMB": 5,
    "AllowedExtensions": [ ".jpg", ".jpeg", ".png" ]
  }
}
```

> **Never commit secrets.** The project is pre-configured to load `appsettings.Local.json` which is already in `.gitignore`.

---

## Background Jobs

Hangfire is configured with SQL Server storage on the `HangfireConnection` string. The dashboard is exposed at `/Jobs` (no auth guard in development — add authorization middleware before going to production).

Jobs currently used for email delivery and any scheduled tasks can be registered via `IBackgroundJobClient` or `IRecurringJobManager` injected into any service.

---

## Maintainers

When adding a new endpoint:

1. Add or verify `[Authorize]` with the correct `Roles` value from `DefaultRoles`.
2. Update `ROLE_ACCESS_DOCUMENTATION.md` in the same PR.
3. Add a FluentValidation validator for any new request DTO in `Mala3ib.BLL/Validations/`.

---

## Team

| Name | GitHub |
|---|---|
| Youssef Shaaban | [@Youssef-Shaaban](https://github.com/Youssef-Shabaan) |
| Hussein Hashiem | [@Hussein-Hashiem](https://github.com/Hussein-Hashiem) |
