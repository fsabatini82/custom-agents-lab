# Employee Portal — Demo Target for Session 2 Lab

Small .NET 9 Web API used as the review target for the custom agents lab.

## Purpose

This codebase contains **intentional** code smells, architectural flaws, and
subtle security issues. It is the target for the `@code-reviewer` and
`@doc-writer` agents built during the workshop.

**Do not use any of this code as a reference for real projects.**

## Run

```bash
dotnet restore
dotnet run
```

Swagger UI at `https://localhost:5001/swagger`.

## Domain

A minimal employee management portal:

- **Employees** — directory with roles and salary info
- **LeaveRequests** — vacation / sick leave workflow
- **Auth** — login endpoint
