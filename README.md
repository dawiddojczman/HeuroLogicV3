# HeuroLogicV3

**A web-based decision support system implementing the Analytic Hierarchy Process (AHP).**

HeuroLogicV3 is a full-stack ASP.NET Core and Blazor Server application designed to support multi-criteria decision-making using Thomas L. Saaty's Analytic Hierarchy Process.

The application allows users to define decision problems, compare criteria and alternatives, evaluate the consistency of their judgments, and generate weighted rankings.

Originally developed as part of my Master's thesis in Applied Computer Science at Bydgoszcz University of Science and Technology, the project has subsequently been revisited to improve its mathematical implementation, architecture, validation, and automated testing.

## Technology Stack

| Area | Technology |
|---|---|
| Backend | C#, ASP.NET Core 8 |
| Frontend | Blazor Server, Razor Components |
| Database | PostgreSQL |
| Data access | Entity Framework Core 8 |
| Authentication | ASP.NET Core Identity |
| Testing | NUnit, EF Core InMemory |
| UI | HTML, CSS, Bootstrap |
| Algorithm | Analytic Hierarchy Process (AHP) |

## Key Features

- Create decision problems with multiple criteria and alternatives.
- Perform pairwise comparisons using Saaty's 1–9 preference scale and reciprocal judgments.
- Generate positive reciprocal comparison matrices.
- Calculate priority vectors using the principal eigenvector method with power iteration.
- Determine the maximum eigenvalue, Consistency Index (CI), and Consistency Ratio (CR).
- Validate judgment consistency before saving decisions.
- Calculate global priorities and produce a weighted ranking of alternatives.
- Store decision results and original comparison matrices in PostgreSQL.
- Review previous decisions using individual user accounts.
- Use a responsive web interface on desktop and mobile devices.

## How AHP Works

The Analytic Hierarchy Process is a structured multi-criteria decision-making method developed by Thomas L. Saaty.

HeuroLogicV3 implements the following workflow:

1. **Define the decision problem.** Create a decision title and specify the criteria and alternatives.
2. **Compare criteria.** Evaluate the relative importance of each pair of criteria using Saaty's preference scale.
3. **Compare alternatives.** Perform separate pairwise comparisons of the alternatives for each criterion.
4. **Calculate priorities.** Construct reciprocal matrices and calculate normalized priority vectors.
5. **Validate consistency.** Calculate CI and CR to determine whether the judgments meet the selected consistency threshold.
6. **Generate the ranking.** Combine criteria weights with the local priorities of alternatives to calculate final scores.

For a pairwise comparison matrix A, the priority vector w is obtained from the principal eigenvector relationship:

**Aw = λmax × w**

For matrices of dimension n greater than two:

**CI = (λmax − n) / (n − 1)**

**CR = CI / RI**

The application uses the commonly adopted CR threshold of 0.10. For matrices of size 2×2, the consistency ratio is defined as zero.

This threshold is a consistency guideline, not a guarantee that the decision is objectively correct.

## Solution Architecture

The solution consists of four projects.

| Project | Responsibility |
|---|---|
| `HeuroLogicV3.Core` | AHP computation engine and domain models |
| `HeuroLogicV3.Database` | PostgreSQL persistence, EF Core entities, Identity and migrations |
| `HeuroLogicV3` | Blazor Server user interface, authentication and application configuration |
| `HeuroLogicV3.Tests` | Numerical algorithm tests and database smoke tests |

The AHP calculation engine is independent of the database layer, making the mathematical logic reusable and easier to test.

## Getting Started

### Prerequisites

- .NET 8 SDK
- PostgreSQL
- Visual Studio 2022 or a compatible .NET development environment

### Clone the Repository

Clone the repository from GitHub:

`git clone https://github.com/dawiddojczman/HeuroLogicV3.git`

Open `HeuroLogicV3.sln` in Visual Studio.

### Configure PostgreSQL

Create a PostgreSQL database and configure the connection string locally using ASP.NET Core User Secrets.

Run the following command from the solution root, replacing the placeholder values:

`dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=HeuroLogicV3;Username=YOUR_USER;Password=YOUR_PASSWORD" --project ./HeuroLogicV3/HeuroLogicV3.csproj`

Do not commit database passwords or other sensitive information to the repository.

### Apply Database Migrations

After configuring PostgreSQL, execute the existing EF Core migrations:

`dotnet ef database update --project ./HeuroLogicV3.Database --startup-project ./HeuroLogicV3`

Use an EF Core 8-compatible version of the `dotnet-ef` tool.

For existing databases, create a backup before applying migrations.

### Build and Run

Restore dependencies:

`dotnet restore`

Build the solution:

`dotnet build HeuroLogicV3.sln`

Run the automated tests:

`dotnet test HeuroLogicV3.sln`

Launch the application:

`dotnet run --project ./HeuroLogicV3/HeuroLogicV3.csproj`

Open the local address displayed in the terminal, register an account, and navigate to **Make Decision**.

## Automated Testing

The NUnit test suite verifies key mathematical properties of the AHP implementation, including:

- Priority calculations for consistent pairwise comparison matrices.
- Principal eigenvector weights for known numerical examples.
- Consistency measurements for contradictory judgments.
- Weighted ranking calculations.
- Validation of invalid or non-reciprocal matrices.
- Basic Entity Framework Core persistence operations.

The project also uses EF Core InMemory for database smoke tests. These tests do not replace integration tests against a real PostgreSQL instance.

## Project Background

HeuroLogicV3 originated as a Master's thesis project in Applied Computer Science at Bydgoszcz University of Science and Technology, Poland.

The project combines full-stack .NET development with numerical methods and multi-criteria decision analysis.

Following the academic implementation, the application has been revisited to introduce more complete pairwise comparisons, consistency validation, improved separation of responsibilities, and enhanced testing.

## Project Status

HeuroLogicV3 is an academic and portfolio project undergoing modernization.

Core AHP calculations, authentication, database persistence, and decision history have been exercised in local tests. Further improvements are planned in integration testing, UI/UX, security hardening, and long-term platform support.

The application is not presented as a production-certified decision support system.

## Author

**Dawid Dojczman**

M.Eng. Applied Computer Science

Politechnika Bydgoska im. Jana i Jędrzeja Śniadeckich w Bydgoszczy

.NET Developer

[GitHub](https://github.com/dawiddojczman)