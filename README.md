[![Unit Tests](https://img.shields.io/badge/Unit_Tests-Passed-brightgreen?style=flat&logo=github)](https://github.com/Omaremad763/AI-Analytics/actions)
[![.NET](https://img.shields.io/badge/.NET_9-512BD4?style=flat&logo=.net&logoColor=white)](https://github.com/Omaremad763/AI-Analytics)
[![Angular](https://img.shields.io/badge/Angular_21-DD0031?style=flat&logo=angular&logoColor=white)](https://github.com/Omaremad763/AI-Analytics)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=flat&logo=postgresql&logoColor=white)](https://github.com/Omaremad763/AI-Analytics)
[![Docker](https://img.shields.io/badge/Docker-2496ED?style=flat&logo=docker&omegaColor=white)](https://github.com/Omaremad763/AI-Analytics)
[![Git Hooks](https://img.shields.io/badge/Git_Hooks-Enabled-blue?style=flat&logo=git&logoColor=white)](https://github.com/Omaremad763/AI-Analytics)
[![Nginx](https://img.shields.io/badge/Nginx-009639?style=flat&logo=nginx&logoColor=white)](https://github.com/Omaremad763/AI-Analytics)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean--Architecture-blueviolet?style=flat)](https://github.com/Omaremad763/AI-Analytics)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat)](https://github.com/Omaremad763/AI-Analytics/blob/main/LICENSE)


---

# AI Analytics Platform

**Enterprise-Grade Financial Data Intelligence System**

A robust, modular full-stack platform designed to transform raw financial data into actionable insights using AI-driven analytics. Built with **.NET 9** and **Angular 21**, this project serves as a showcase for clean architecture, CQRS patterns, and high-performance data processing pipelines.

## 🌟 Engineering Vision

This project was designed to solve the complexity of large-scale data ingestion and analysis. Instead of monolithic structures, I implemented a **layered domain-driven approach** that ensures the system is testable, maintainable, and highly extensible.

---

## 🏗️ System Architecture

The system follows strict **Clean Architecture** principles, enforcing separation of concerns between business logic, infrastructure, and presentation.

### High-Level Design

* **Domain Layer:** Core entities (`FinancialRecord`, `DataBatch`) and business rules.
* **Application Layer:** CQRS implementations using MediatR, DTOs, and application interfaces.
* **Infrastructure Layer:** External integrations (OpenAI), database persistence (EF Core), and file processing services.
* **Presentation Layer:** Modern REST API with global exception handling and middleware.

---

## 🚀 Core Pillars

### 1. Data Ingestion Pipeline (Phase I)

* **Goal:** Efficiently ingest, parse, and validate complex financial spreadsheets.
* **Technology:** Background processing with **Hangfire** to ensure non-blocking UI interactions during file parsing.
* **Flow:** File Upload $\rightarrow$ Validation $\rightarrow$ Background Batch Processing $\rightarrow$ Status Polling.

### 2. Analytics Dashboard (Phase II)

* **Goal:** Real-time visualization of financial trends.
* **Strategy:** Optimized SQL Views for high-performance read operations, providing aggregated data for charts and metrics.
* **Components:** Modular components with reusable chart wrappers.

### 3. AI Insights Engine (Phase III)

* **Goal:** Providing intelligent summarization of financial data.
* **Mechanism:** Asynchronous integration with **OpenAI API** triggered post-ingestion.
* **UX Touch:** Typing animation to simulate a real-time conversational AI experience.

---

## 🛠️ Technology Stack

| Layer | Technologies |
| --- | --- |
| **Backend** | .NET 9, EF Core, MediatR (CQRS), Hangfire, FluentValidation |
| **Database** | PostgreSQL |
| **Frontend** | Angular 21 (Standalone), Tailwind CSS, RxJS |
| **DevOps** | Docker, Docker Compose, GitHub Actions (CI/CD) |

---

## 📁 Project Structure

```text
omaremad763-ai-analytics/
├── Application/    # CQRS, DTOs, & Business Logic
├── Domain/         # Entities & Enums
├── Infrastructure/ # Persistence, External Services & Migrations
├── Presentation/   # API Controllers & Middlewares
└── Front/          # Angular Standalone Components & Pages

```

---

## 🔐 Engineering Standards

* **Robustness:** Global Exception Handling & Logging.
* **Scalability:** CQRS pattern separates Read/Write models.
* **Maintainability:** Dependency Injection via `DependenciesCollector`.
* **Quality:** Automated testing project for critical ingestion workflows.

---

## 🤝 Contributing

This project is part of a long-term engineering journey. Suggestions regarding architecture or performance improvements are highly appreciated.

**Built with passion by [Omar Emad**]([https://www.linkedin.com/in/omar-abusaif/])

---
