
Members:
Balan Catalin- Backend
Borsan Iulian- Frontend
Barabas Robert- Tester
Alecsa Alin- DevOps




# 💰 MoneyTracker — Personal Finance Monitoring Application

> A comprehensive personal finance management tool that helps you track income, expenses, budgets, and financial goals in real time.

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Configuration](#configuration)
  - [Running the Application](#running-the-application)
- [Usage](#usage)
  - [Dashboard](#dashboard)
  - [Transactions](#transactions)
  - [Budgets](#budgets)
  - [Reports & Analytics](#reports--analytics)
  - [Alerts & Notifications](#alerts--notifications)
- [API Reference](#api-reference)
- [Database Schema](#database-schema)
- [Testing](#testing)
- [Deployment](#deployment)
- [Contributing](#contributing)
- [License](#license)

---

## Overview

**MoneyTracker** is a full-stack personal finance monitoring application designed to give users complete visibility over their financial health. Whether you're tracking daily coffee expenses or planning long-term savings goals, MoneyTracker provides intuitive tools to record transactions, set budgets, visualize spending patterns, and receive smart alerts when limits are approached.

The application is built with a focus on:
- **Privacy** — all financial data is stored locally or in your own cloud instance
- **Accuracy** — double-entry accounting principles ensure data integrity
- **Insight** — rich analytics and charts surface trends and anomalies automatically

---

## Features

### Core Features
- **Transaction Management** — add, edit, delete, and categorize income and expense transactions
- **Multi-Account Support** — manage bank accounts, cash wallets, credit cards, and savings accounts in one place
- **Budget Planning** — set monthly or custom-period budgets per category and track progress in real time
- **Recurring Transactions** — schedule automatic entries for rent, subscriptions, salaries, and bills
- **Currency Support** — multi-currency tracking with live exchange rate conversion
- **Financial Goals** — define savings goals with target amounts and deadlines; track contribution progress
- **Reports & Charts** — interactive pie charts, bar graphs, and line charts for spending and income analysis
- **CSV / PDF Export** — export transaction history and monthly reports for tax or accounting purposes
- **Smart Alerts** — receive push or email notifications when:
  - a budget category exceeds 80% of its limit
  - an unusual transaction amount is detected
  - a recurring payment is due in 3 days
  - a savings goal milestone is reached

### Advanced Features
- **Tag System** — attach custom tags to transactions for cross-category filtering
- **Split Transactions** — split a single payment across multiple categories
- **Net Worth Tracker** — aggregate all assets and liabilities for an at-a-glance net worth view
- **Dark Mode** — full dark/light theme support
- **Offline Mode** — all core features work offline; data syncs when connectivity is restored
- **Two-Factor Authentication (2FA)** — protect sensitive financial data with TOTP-based 2FA

---

## Tech Stack

| Layer        | Technology                          |
|--------------|-------------------------------------|
| Frontend     | React 18, TypeScript, TailwindCSS   |
| State Mgmt   | Zustand                             |
| Charts       | Recharts                            |
| Backend      | Node.js, Express.js                 |
| Database     | PostgreSQL (primary), Redis (cache) |
| Auth         | JWT + bcrypt, TOTP (speakeasy)      |
| ORM          | Prisma                              |
| API Style    | REST + WebSocket (real-time alerts) |
| Email        | Nodemailer + SendGrid               |
| Testing      | Jest, React Testing Library, Supertest |
| DevOps       | Docker, Docker Compose, GitHub Actions |
| Deployment   | AWS EC2 / Railway / Render          |

---

## Architecture

```
moneytracker/
├── client/                     # React frontend
│   ├── src/
│   │   ├── components/         # Reusable UI components
│   │   ├── pages/              # Route-level page components
│   │   ├── hooks/              # Custom React hooks
│   │   ├── store/              # Zustand state slices
│   │   ├── services/           # API call wrappers
│   │   └── utils/              # Formatters, helpers
│   └── public/
├── server/                     # Express backend
│   ├── src/
│   │   ├── controllers/        # Route handler logic
│   │   ├── middlewares/        # Auth, validation, error handling
│   │   ├── models/             # Prisma model helpers
│   │   ├── routes/             # API route definitions
│   │   ├── services/           # Business logic layer
│   │   ├── jobs/               # Cron jobs (recurring txns, alerts)
│   │   └── utils/              # Currency conversion, email helpers
│   └── prisma/
│       ├── schema.prisma       # Database schema
│       └── migrations/
├── docker-compose.yml
└── .env.example
```

---

## Getting Started

### Prerequisites

Make sure you have the following installed:

- **Node.js** >= 18.x
- **npm** >= 9.x or **yarn** >= 1.22
- **PostgreSQL** >= 14
- **Redis** >= 7
- **Docker** & **Docker Compose** (optional, for containerized setup)

### Installation

```bash
Clone the repository

cd moneytracker

# Install backend dependencies
cd server
npm install

# Install frontend dependencies
cd ../client
npm install
```

### Configuration

Copy the example environment files and fill in your values:

```bash
# Backend
cp server/.env.example server/.env

# Frontend
cp client/.env.example client/.env
```

**`server/.env`**
```env
# Application
PORT=5000
NODE_ENV=development

# Database
DATABASE_URL=postgresql://user:password@localhost:5432/moneytracker

# Redis
REDIS_URL=redis://localhost:6379

# Auth
JWT_SECRET=your_super_secret_jwt_key
JWT_EXPIRES_IN=7d

# Email (SendGrid)
SENDGRID_API_KEY=your_sendgrid_api_key
EMAIL_FROM=noreply@moneytracker.app

# Exchange Rates API
EXCHANGE_RATE_API_KEY=your_api_key
```

**`client/.env`**
```env
VITE_API_BASE_URL=http://localhost:5000/api
VITE_WS_URL=ws://localhost:5000
```

### Running the Application

**Option A — Local Development**

```bash
# Start PostgreSQL and Redis (if not already running)
# Then run migrations
cd server
npx prisma migrate dev
npx prisma db seed

# Start the backend (from /server)
npm run dev

# Start the frontend (from /client, in a new terminal)
npm run dev
```

The app will be available at `http://localhost:5173`.

**Option B — Docker Compose**

```bash
docker-compose up --build
```

This starts PostgreSQL, Redis, the API server, and the React dev server together.

---

## Usage

### Dashboard

The dashboard is your financial command center. On first load you will see:

- **Net Balance** — total income minus total expenses for the current month
- **Recent Transactions** — last 5 transactions across all accounts
- **Budget Summary** — a horizontal progress bar per active budget category
- **Goals Widget** — progress rings for each active savings goal
- **Spending Trend** — a 30-day line chart comparing this month vs last month

### Transactions

Navigate to **Transactions > Add** to log a new transaction:

1. Select **Type**: Income or Expense
2. Enter **Amount** and choose **Currency**
3. Pick an **Account** (e.g., Main Bank, Cash)
4. Choose a **Category** (e.g., Food, Transport, Salary)
5. Add an optional **Note** and **Tags**
6. Submit — the dashboard updates immediately

To bulk import transactions, go to **Transactions > Import** and upload a CSV file following the provided template.

### Budgets

1. Go to **Budgets > New Budget**
2. Choose a **Category** and set a **Limit Amount**
3. Define the **Period** (monthly, weekly, or custom date range)
4. Save — the budget now appears in the dashboard widget and will trigger alerts automatically

### Reports & Analytics

Under the **Reports** section you can generate:

- **Monthly Summary** — income vs expenses bar chart with category breakdown
- **Category Drill-Down** — pie chart showing where money went within a selected period
- **Cash Flow Statement** — day-by-day running balance for any account
- **Year-over-Year Comparison** — overlay two years to spot seasonal patterns

All charts support hover tooltips and can be exported as PNG or included in a PDF report.

### Alerts & Notifications

Go to **Settings > Notifications** to configure:

- Delivery channel: in-app only, email, or both
- Budget threshold: default 80%, configurable per category
- Upcoming recurring payment warning: 1, 3, or 7 days in advance
- Goal milestone alerts: 25%, 50%, 75%, 100% of target reached

---

## API Reference

All endpoints are prefixed with `/api/v1`.

| Method | Endpoint                       | Description                          | Auth |
|--------|-------------------------------|--------------------------------------|------|
| POST   | `/auth/register`               | Create a new user account            | No   |
| POST   | `/auth/login`                  | Authenticate and receive JWT         | No   |
| GET    | `/transactions`                | List transactions (filterable)       | Yes  |
| POST   | `/transactions`                | Create a new transaction             | Yes  |
| PUT    | `/transactions/:id`            | Update a transaction                 | Yes  |
| DELETE | `/transactions/:id`            | Delete a transaction                 | Yes  |
| GET    | `/budgets`                     | List all budgets                     | Yes  |
| POST   | `/budgets`                     | Create a new budget                  | Yes  |
| GET    | `/reports/monthly`             | Monthly income/expense summary       | Yes  |
| GET    | `/accounts`                    | List all accounts                    | Yes  |
| POST   | `/accounts`                    | Create a new account                 | Yes  |
| GET    | `/goals`                       | List savings goals                   | Yes  |
| POST   | `/goals`                       | Create a savings goal                | Yes  |
| PATCH  | `/goals/:id/contribute`        | Add a contribution to a goal         | Yes  |

**Example — Create a transaction:**
```bash
curl -X POST http://localhost:5000/api/v1/transactions \
  -H "Authorization: Bearer <your_jwt>" \
  -H "Content-Type: application/json" \
  -d '{
    "type": "expense",
    "amount": 45.99,
    "currency": "RON",
    "accountId": "acc_123",
    "categoryId": "cat_food",
    "note": "Supermarket Kaufland",
    "tags": ["groceries", "weekly-shop"],
    "date": "2026-04-28"
  }'
```

---

## Database Schema

Key tables in the PostgreSQL database:

- **users** — id, email, password_hash, currency_preference, created_at
- **accounts** — id, user_id, name, type (bank/cash/credit), balance, currency
- **categories** — id, user_id, name, color, icon, type (income/expense)
- **transactions** — id, user_id, account_id, category_id, amount, currency, note, date, is_recurring
- **budgets** — id, user_id, category_id, limit_amount, period_start, period_end
- **goals** — id, user_id, name, target_amount, current_amount, deadline
- **recurring_rules** — id, user_id, frequency (daily/weekly/monthly), next_due, transaction_template
- **tags** — id, user_id, name; linked via **transaction_tags** join table

---

## Testing

```bash
# Run all backend tests
cd server
npm test

# Run frontend unit tests
cd client
npm test

# Run end-to-end tests (requires the app to be running)
npm run test:e2e
```

Test coverage targets: **>80%** for services and controllers.

---

## Deployment

A production Docker image can be built and pushed with:

```bash
docker build -t moneytracker-api ./server
docker build -t moneytracker-client ./client
```

A sample `docker-compose.prod.yml` is included for single-server deployments. For cloud deployments, GitHub Actions workflows in `.github/workflows/` handle CI testing and automatic deployment to Railway or Render on every push to `main`.

---
## License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

---

> Built with care to help people take control of their finances, one transaction at a time.
