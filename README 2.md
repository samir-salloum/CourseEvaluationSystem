# Course Evaluation System


ASP.NET Core MVC + EF Core. Studenter kan lämna kursutvärderingar (betyg 1–5 + valfri kommentar). Admin ser sammanställningar och kan filtrera per kurs och datum.

## Quick start
1) Skapa DB och kör appen:
   - `dotnet ef database update` (eller bara starta – migrering + seed sker automatiskt).
   - Starta i **Debug**.

2) Logga in
   - **Admin:** `admin@demo.se` / `Admin123!`
   - **Student:** `student@demo.se` / `Student123!`

3) Flöden
   - Student: Kurser → *Details* → *Lämna utvärdering* → *Thanks*.
   - Admin: */Admin* → filtrera per kurs eller datum.

## Tests
- Kör `dotnet test`.
- InMemory-DB, xUnit. Tester täcker: rating-range, kommentar-krav vid ≤2, maxlength 1000, dubblettspärr, CreatedAt, admin-filter m.m.
- (TDD) Se commit-historik: `test: failing ...` → `feat: make test pass`. Skärmbilder finns i `docs/screenshots`.

## Teknik
- EF Core (ORM, LINQ), migrations, seed (demo-data + Identity-roller).
- MVC-pattern, separerat service-lager (`IEvaluationService`).

## Kravmappning (ur kursmålen)
- (5) Tekniska instruktioner/validering följs.
- (6) Debugging/Test Explorer (bilder) + tester.
- (7) Git/GitHub med versionshistorik (se commits).
- (8) Tester före implementation (TDD) – se commit-paret test→feat.
- (11–16) MVC, modeller/relationsdatabas, Razor Views, Controllers→C#-metoder, LINQ/EF.

---

## Features

- Authentication with roles (Student / Admin)
- Students can submit evaluations (rating + comment)
- Admin can view aggregated results per course
- Filtering options for Admin:
  - Filter by course
  - Filter by date range (`CreatedAt`)
- Average rating calculation per course
- Student comments displayed per course
- Unit tests (xUnit + EF Core InMemory)

---

## Getting Started

### 1. Clone the repository
```bash
git clone https://github.com/yourusername/CourseEvaluationSystem.git
cd CourseEvaluationSystem
