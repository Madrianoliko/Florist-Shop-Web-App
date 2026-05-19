# 🌸 Florist Shop Web App

A full-stack florist shop web application built with **Blazor Web App (.NET 9)** and **PostgreSQL**. Covers three core business flows — bouquet shop, event decoration enquiries, and prop rental — plus a complete admin panel.

> **Portfolio project.** Based on the architecture and design of [klimatynakwiaty.pl](https://klimatynakwiaty.pl), a real florist shop built for a client. The fictional business is *Petal & Bloom*, Bristol UK.
>
> Original app repo: [Florist-Shop-App](https://github.com/Madrianoliko/Florist-Shop-App)

---

## ✨ Features

### Public pages
| Page | Route | Description |
|---|---|---|
| Home | `/` | Landing page — hero, offer overview, about us, reviews, contact |
| Bouquet Shop | `/shop` | Catalogue of bouquet templates grouped by category |
| Order Form | `/shop/order` | Place a bouquet order (template pre-selection, delivery method, note) |
| Prop Rental | `/rental` | Catalogue of rentable props (vases, arches, lanterns…) |
| Reservation Form | `/rental/reservation` | Reserve selected props for a date range |
| Decorations | `/decorations` | Event decoration service overview |
| Decoration Enquiry | `/decorations/inquiry` | Send a decoration enquiry (event type, date, venue, budget) |
| Gallery | `/gallery` | Photo gallery of past work |

### Admin panel (`/admin` — login required)
- **Orders** — view & update order status, payment status
- **Rental reservations** — view & manage reservation status
- **Decoration enquiries** — view & respond to enquiries
- **Bouquet catalogue** — add / edit / remove bouquet templates, upload photos
- **Rental catalogue** — add / edit / remove rental items, upload photos
- **Gallery** — upload / delete gallery photos
- **Dictionary** — manage bouquet & rental categories (label, sort order)

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 9 — Blazor Web App |
| Render mode | Static SSR + Interactive Server (SignalR) |
| ORM | Entity Framework Core 9 + Npgsql |
| Database | PostgreSQL 16 |
| Auth | ASP.NET Core Identity |
| CSS | Bootstrap 5 + custom design system (CSS variables, `kw-*` classes) |
| Fonts | Playfair Display, Cormorant Garamond (Google Fonts) |
| Deployment | Railway (web service + managed PostgreSQL) |
| Containerisation | Docker / Docker Compose (local dev) |
| Tests | xUnit |

---

## 🏗️ Architecture

**Modular monolith** — one web project with an internal domain-module structure. Each module owns its models, EF Core configuration, services, and Razor pages. Adding a new module doesn't require touching `AppDbContext`.

```
FloristShop.sln
├── src/
│   └── FloristShop.Web/
│       ├── Components/          # Shared layout & pages (Home, Error, Auth)
│       │   └── Layout/          # NavMenu, MainLayout
│       ├── Modules/
│       │   ├── Shop/            # Bouquet catalogue & orders
│       │   ├── Rental/          # Prop catalogue & reservations
│       │   ├── Decoration/      # Event decoration enquiries
│       │   ├── Gallery/         # Photo gallery
│       │   ├── Dictionary/      # Reference data (categories)
│       │   └── Admin/           # Admin panel (cross-cutting)
│       └── Infrastructure/
│           ├── Data/            # AppDbContext, EntityBase, Migrations
│           ├── Identity/        # AppUser, IdentitySeed
│           └── FileUploadService.cs
└── tests/
    └── FloristShop.Tests/
```

**Key patterns:**
- `EntityBase` — base class with `Id` (Guid), `CreatedAt`, `UpdatedAt`
- `IEntityTypeConfiguration<T>` per entity — EF config lives next to the model
- `ApplyConfigurationsFromAssembly` — zero-touch `AppDbContext` when adding entities
- Snake-case column names via `EFCore.NamingConventions`
- Seed data: dictionary entries always; dev bouquet/rental fixtures only in Development

---

## 🚀 Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for local PostgreSQL)

### 1. Clone

```bash
git clone https://github.com/Madrianoliko/Florist-Shop-Web-App.git
cd Florist-Shop-Web-App
```

### 2. Start PostgreSQL via Docker Compose

```bash
docker-compose up -d db
```

This starts a PostgreSQL 16 container on `localhost:5432` with database `floristshop`.

### 3. Configure secrets (optional)

The default `appsettings.Development.json` already points at the Docker database and seeds a local admin account. To override, use user secrets:

```bash
cd src/FloristShop.Web
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=floristshop;Username=app;Password=yourpassword"
```

### 4. Run

```bash
dotnet run --project src/FloristShop.Web
```

EF Core migrations run automatically on startup and seed dictionary data + dev fixtures. Open `https://localhost:5001`.

**Default admin login (dev):**
```
Email:    admin@floristshop.local
Password: Test1234!
```

---

## ⚙️ Environment Variables

| Variable | Description | Example |
|---|---|---|
| `DATABASE_URL` | Full PostgreSQL URL (Railway format) | `postgresql://user:pass@host:5432/db` |
| `PGHOST` | PostgreSQL host (fallback) | `containers-us-west-1.railway.app` |
| `PGPORT` | PostgreSQL port | `6543` |
| `PGDATABASE` | Database name | `railway` |
| `PGUSER` | Database user | `postgres` |
| `PGPASSWORD` | Database password | `secret` |
| `PORT` | HTTP port for the web server | `8080` |
| `ASPNETCORE_ENVIRONMENT` | `Production` or `Development` | `Production` |

Admin accounts are seeded from `AdminUsers` config (array of `{ Email, FullName, Password }`). In production, pass them as Railway Variables in JSON form or via individual secrets.

---

## 🐳 Docker

Build and run the full stack (app + database):

```bash
docker-compose up --build
```

The app will be available at `http://localhost:8080`.

---

## ☁️ Deployment (Railway)

1. Create a new Railway project → **Deploy from GitHub repo**
2. Add a **PostgreSQL** plugin — Railway injects `DATABASE_URL` automatically
3. Set `ASPNETCORE_ENVIRONMENT=Production`
4. Add your admin user(s) via the `AdminUsers` variable:
   ```json
   [{"Email":"you@example.com","FullName":"Admin","Password":"StrongPass1!"}]
   ```
5. Deploy — migrations run on startup, the app is live within seconds

---

## 📁 Database Schema (overview)

```
AspNetUsers / AspNetRoles          ← ASP.NET Core Identity
bouquet_templates                  ← Shop module
orders / order_items               ← Shop module
rental_items                       ← Rental module
rental_reservations                ← Rental module
rental_reservation_items           ← Rental module
decoration_inquiries               ← Decoration module
gallery_photos                     ← Gallery module
dictionary_entries                 ← Dictionary module (reference data)
sequence_counters                  ← Human-readable order/reservation numbers
```

All tables use snake_case column names. Every domain entity extends `EntityBase` (UUID primary key, `created_at`, `updated_at`).

---

## 🔗 Related

- **Live site (original Polish app):** [klimatynakwiaty.pl](https://klimatynakwiaty.pl)
- **Original repo:** [Florist-Shop-App](https://github.com/Madrianoliko/Florist-Shop-App)
- **Portfolio:** [adrianmalik.pl](https://adrianmalik.pl) *(coming soon)*

---

## 👤 Author

**Adrian Malik** — [adrianmalik1001@gmail.com](mailto:adrianmalik1001@gmail.com) · [GitHub](https://github.com/Madrianoliko)

---

*Built as a portfolio showcase. The business data (Petal & Bloom, Bristol) is fictional.*
