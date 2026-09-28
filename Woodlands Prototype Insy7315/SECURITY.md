# Security implementation (Sprint 6 - Role 3)

## Setup (secrets are NOT committed)
Run once from the `Woodlands Prototype Insy7315` folder:

    dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<azure sql connection string>"
    dotnet user-secrets set "Jwt:Key" "<random string, 32+ characters>"

Environment variables work too (`ConnectionStrings__DefaultConnection`, `Jwt__Key`).
The app refuses to start if `Jwt:Key` is missing or shorter than 32 characters.

**Rotate the old credentials.** The Azure SQL password and Supabase keys were
previously committed in `appsettings.json`, so they remain in git history.
Change the SQL password and regenerate the Supabase keys.

## What is implemented
| Requirement | Where |
|---|---|
| Password hashing | ASP.NET Core Identity PBKDF2 hasher (`UserManager.CreateAsync`) |
| Password policy + lockout | `Program.cs` (8+ chars, upper/lower/digit/symbol; 5 failures = 15 min lock) |
| Login / Register (site) | `AccountController` (cookie session via `SignInManager`) |
| JWT auth | `AuthApiController`: `POST /api/auth/login`, `POST /api/auth/register`, `GET /api/auth/me`; `JwtTokenService` |
| RBAC (Admin, Manager, Sales, Customer) | `IdentitySeederRoles`, `[Authorize(Roles=...)]`, policies `StaffOnly` / `AdminOnly` |
| Input validation | DataAnnotations + length limits on view models; `ModelState` checks |
| XSS | Razor output encoding + `InputSanitizer.StripHtml` on free-text input |
| SQL injection | EF Core parameterised queries only; no raw SQL in the project |
| CSRF | `[ValidateAntiForgeryToken]` on POST actions |
| Audit logging | `AuditLogs` table via `AuditLogService` (login, register, logout, admin user create/edit/delete) |

## Demo
1. Call `POST /api/auth/login` with valid credentials, then `GET /api/auth/me` with `Authorization: Bearer <token>`.
2. Call `/api/auth/me` with no token: `401`.
3. Log in as a Customer and open `/Admin/Users`: redirected to Access Denied.
4. Register with a weak password or `<script>` in the name: rejected / stripped.
5. Fail login 5 times: account locks; each attempt appears in `AuditLogs`.

## Known gaps
- Seeded admin password is hard-coded in `DbInitializer` (change after first login or move to config).
- Testimonials, FAQs and products still go through the Node API, which has no source in this repo.
- Migration `AddAuditLog` was written by hand; verify with `dotnet build` and `dotnet ef migrations list`.
- No automated tests yet (Role 5).
