## 1. Language & Project Setup

- Target **.NET 9** (or latest LTS available).
- Use lean and modern features of C# 12 or later.
- Enable **`<Nullable>enable</Nullable>`** and **`<ImplicitUsings>enable</ImplicitUsings>`**.

---

## 2. Unit Testing (Mandatory)

- Use **xUnit** (no UI, database, or mocks needed).
- Achieve **100 % branch coverage** for the core domain game logic.
- Use well-known unit test best practices.

---

## 3. Coding Conventions

- Follow **Microsoft C# style** with `dotnet format`.
- Use **expression-bodied** members where concise.
- Mark fields `private readonly`.
- Prefer **guard clauses** for validations.
- All public APIs are **XML-documented**.
- Treat warnings as errors (`<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`).
