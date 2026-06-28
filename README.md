[![Unit Tests](https://img.shields.io/badge/Unit_Tests-Passed-brightgreen?style=flat&logo=github)](https://github.com/Omaremad763/AI-Analytics/actions)
[![.NET](https://img.shields.io/badge/.NET_9-512BD4?style=flat&logo=.net&logoColor=white)](https://github.com/Omaremad763/AI-Analytics)
[![Angular](https://img.shields.io/badge/Angular_21-DD0031?style=flat&logo=angular&logoColor=white)](https://github.com/Omaremad763/AI-Analytics)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=flat&logo=postgresql&logoColor=white)](https://github.com/Omaremad763/AI-Analytics)
[![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat&logo=docker&omegaColor=white)](https://github.com/Omaremad763/AI-Analytics)
[![Git Hooks](https://img.shields.io/badge/Git_Hooks-Enabled-blue?style=flat&logo=git&logoColor=white)](https://github.com/Omaremad763/AI-Analytics)
[![Nginx](https://img.shields.io/badge/Nginx-009639?style=flat&logo=nginx&logoColor=white)](https://github.com/Omaremad763/AI-Analytics)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean--Architecture-blueviolet?style=flat)](https://github.com/Omaremad763/AI-Analytics)
![GitHub Actions](https://img.shields.io/badge/GitHub%20Actions-2088FF?style=for-the-badge&logo=github-actions&logoColor=white)
![Groq AI](https://img.shields.io/badge/Groq%20AI-F55036?style=for-the-badge&logo=probot&logoColor=white)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat)](https://github.com/Omaremad763/AI-Analytics/blob/main/LICENSE)

# 📊 AI Analytics Platform

> AI-powered Data Analytics System for processing financial Excel/CSV files, generating insights, and visualizing results through an interactive dashboard.

---

# 🚀 Overview

AI Analytics Platform is a full-stack system designed to:

* Upload and process financial datasets (Excel / CSV)
* Parse and store structured financial records
* Generate analytics and KPIs
* Produce AI-generated summaries per dataset batch
* Visualize insights via an interactive dashboard

The system is built using **Clean Architecture + CQRS + Background Processing**.

---

# 🧱 System Architecture

```text
Presentation Layer (ASP.NET Core API)
        │
Application Layer (CQRS + Services + DTOs)
        │
Domain Layer (Entities + Business Rules)
        │
Infrastructure Layer (DB + External Services + File Processing)
```

---

# 📁 Project Structure

```text
AI_Analytics.sln
│
├── Presentation
│   ├── Controllers
│   ├── Middleware
│   └── Program.cs
│
├── Application
│   ├── CQRS
│   ├── DTOs
│   ├── Contracts
│   └── AutoMapper
│
├── Domain
│   ├── Entities
│   └── Enums
│
├── Infrastructure
│   ├── Persistence (EF Core)
│   ├── Services
│   ├── Repositories
│   ├── Migrations
│   └── Excel Parser
│
├── Front (Angular)
│   ├── Pages
│   ├── Core
│   ├── Shared
│   └── Services
│
├── AnalyticsTestProject
└── docker-compose.yml
```
# 🖼️ Screenshots
## Upload Excel file 

![Dashboard](docs/images/Upload.png)

---

## Dashboard

![Dashboard](docs/images/dashboard.png)

---

## Ainsights

![Dashboard](docs/images/Ainsights.png)

---

# 🎥 Demo

Coming Soon...
---

# ⚙️ Core Modules

## 📥 1. Data Ingestion Module

### Purpose

Handles file upload and background processing of datasets.

### Backend Components

* `DataBatch` Entity → tracks uploaded file state
* `IDataBatchRepository`
* `UploadFileCommand`
* `ProcessFileCommand`
* `ExcelParserService`
* Background processing (Hangfire Job)

### API Endpoints

```
POST /api/uploads/upload
GET  /api/uploads/status/{id}
```

### Flow

```text
Upload File → Create Batch → Background Parsing → Store Financial Records → Update Status
```

---

## 📊 2. Analytics Module

### Purpose

Transforms raw financial data into structured metrics and charts.

### Domain

* `FinancialRecord`

### Application Layer

* `GetFinancialMetricsQuery`
* `GetTrendDataQuery`
* `GetCategoryDistributionQuery`

### DTOs

* `MetricCardDto`
* `ChartDataDto`

### API Endpoints

```
GET /api/analytics/metrics
GET /api/analytics/charts
```

### Output Types

* KPI Cards
* Line Chart (Trends)
* Pie Chart (Distribution)

---

## 🤖 3. AI Insights Module

### Purpose

Generates AI-powered summaries per uploaded dataset batch.

### Domain

* `BatchSummary`

### Application Layer

* `GenerateAiSummaryCommand`
* `GetBatchSummaryQuery`

### Infrastructure

* `OpenAIService` (API Wrapper)

### API Endpoint

```
GET /api/analytics/ai-summary/{batchId}
```

### Output

* Natural language financial summary
* Batch-level insights

---

# 🖥️ Frontend (Angular)

## Pages

* Upload File Page
* Analytics Dashboard
* AI Summary Page

## Components

* `FileUploadComponent`
* `StatsCardComponent`
* `ChartWrapperComponent`
* `AiSummaryCardComponent`

## Behavior Flow

```text
Upload File → Receive BatchId → Polling Status → Load Dashboard → Render Charts → Fetch AI Summary
```

---

# 🧠 Domain Model

## Entities

* `DataBatch`

  * FileName
  * Status
  * UploadDate

* `FinancialRecord`

  * Parsed dataset rows

* `BatchSummary`

  * AI-generated summary text

## Enums

* `BatchStatusEnum`
* `FinancialRecordsEnum`

---

# 🔄 Data Flow

```text
1. User uploads file (Excel/CSV)
2. API creates DataBatch
3. Background job parses file
4. FinancialRecords stored in DB
5. Analytics queries aggregate data
6. AI service generates summary
7. Frontend displays:
   - KPIs
   - Charts
   - AI Insights
```

---

# 🐳 Deployment

## Docker Setup

* ASP.NET Core API
* Angular Frontend
* Database (via Docker Compose)

```bash
docker-compose up --build
```

---

# ⚙️ Infrastructure

* EF Core (Database Layer)
* Hangfire (Background Jobs)
* AutoMapper (DTO Mapping)
* OpenAI API Integration
* SQL Views (Analytics Optimization)

---

# 🧪 Testing

* AnalyticsTestProject included
* CQRS unit testing (Data ingestion flow)

---

# 🚀 Key Design Decisions

* CQRS used for separation of reads/writes
* Background jobs for file processing
* Batch-based processing model
* Analytics optimized via SQL Views / aggregation layer
* Modular service contracts for scalability
* AI layer decoupled from analytics engine

---

# 📌 Future Improvements

* Real-time updates (SignalR)
* Advanced AI predictions
* Streaming ingestion pipeline
* Multi-tenant support
* Role-based authentication
* Export reports (PDF / Excel)
* Performance optimization with caching layer

---

# 📄 License

MIT License

---

# 👨‍💻 Author

**Omar Emad**

Software Engineer | Full Stack Development

[![LinkedIn](https://img.shields.io/badge/LinkedIn-blue?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/omar-abusaif/)
> https://www.linkedin.com/in/omar-abusaif/

Online Resume

> (https://omar-emad.vercel.app
### 📈 GitHub Stats
![GitHub Stats](https://github-readme-stats.vercel.app/api?username=Omaremad763&show_icons=true&theme=radical&count_private=true)

![Top Langs](https://github-readme-stats.vercel.app/api/top-langs/?username=Omaremad763&layout=compact&theme=radical)



