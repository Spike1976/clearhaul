# Current status

Date: 2026-09-30
Current milestone: Phase Zero domain foundation. Not an MVP and not a live marketplace.
Current branch: milestone/m1-phase-zero
Published repository: https://github.com/Spike1976/clearhaul
Published main commit before this branch: 2ee5a787d5aae873eac58eccb2f12243bae31a78
Last completed task: Phase Zero domain library implemented and covered by the Release suite.

## Counts

Complete: 5
Partial: 8
Failed: 0
Blocked: 6

Complete means a test command passed for that slice:

- Server health routes
- Client health client
- Backup library round trip, tamper checks, and retention selection
- Repository secret scan
- In-process shipment, payment, permission, equipment-history, and hazmat-gate model

Partial:

- Avalonia window compiles and lists four workspaces as not built. The button was not clicked.
- Docker Compose file exists and was not started. It now mounts the migrations directory.
- `db/migrations/0001_foundation.sql` and `0002_phase_zero.sql` exist and have not been applied.
- AGPL version 3 is the working license. Final legal review is still open.
- Sandbox funding adapter and simulated ledger. No bank.
- Hazmat facts and a rules engine that refuses a live answer. No approved rule package.
- Equipment history, washout checks, and enforcement records. No file storage and no investigator console.
- Driver packet token with an offline read. No Android app.

Blocked:

- Shipment HTTP API and the shipper, carrier, driver, and administrator screens
- Docker Desktop, which is not installed and was not installed
- A running database and the migration tests
- External-drive backup, until Michael provides the path
- Live hazmat operation, until qualified review
- Production payment and production XRP Ledger anchoring

## Features that work

- `GET /health/live` and `GET /health` on the test host
- Unknown routes return a problem response without a stack trace
- The Windows health client rejects a file address and reads a stub health response
- Encrypted backup create and restore for a sample directory
- A text scan for private keys and cloud access-key prefixes
- Domain transitions for the nonhazardous dry-van path, including refused unverified posting, refused unfunded award, hidden equipment history, simulated balanced release, and refused audit deletion

## Questions

No question is waiting in the notepad. The legal role of the operator is undecided and is recorded in LEGAL_REVIEW_REQUIRED.md. It blocks live money movement and any public claim that the platform is a broker or an escrow holder. It does not block the in-process model.

## Tests

See TEST_RESULTS.md. Command exit code was 0. Passed 52, failed 0, skipped 0.

## Defects

- Avalonia 12.1.3 does not compile with the installed .NET 8 compiler. The client uses Avalonia 11.3.22.
- The MinIO and PostGIS image tags have not been pulled.
- The window button has not been operated by a person in this session.
- The backup m0-foundation-20260930 does not contain this Phase Zero branch.
- SQL append-only triggers are untested because PostgreSQL is not running.

## Security

Health routes are unauthenticated and return no freight data. Domain permissions exist in memory only. There is no login, no TLS, and no persisted audit ledger. See SECURITY_MODEL.md.

## Compliance

No statute is encoded. Live hazmat dispatch is disabled. See HAZMAT_ENGINE.md and LEGAL_REVIEW_REQUIRED.md.

## Backup

Local destination: `C:\Users\17402\ClearHaul-Backups`
Repository restore: succeeded for backup m0-foundation-20260930. That restore does not include Phase Zero.
Off-computer copy: not configured

## Next task

Wire the domain model to authenticated persistence after Docker is available, or build the first real shipper tender screen against the domain library without adding fake controls. Do not start Compose and do not merge this branch to main until that work is reviewed.

## Resume

```powershell
Set-Location C:\Users\17402\projects\clearhaul
git switch milestone/m1-phase-zero
dotnet test ClearHaul.sln --configuration Release
```

## Files under active development

`src/ClearHaul.Domain`, `tests/ClearHaul.Domain.Tests`, and `db/migrations/0002_phase_zero.sql`.
