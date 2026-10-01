# Current status

Date: 2026-09-30
Current milestone: foundation checkpoint, not a finished product milestone
Current branch: milestone/m0-foundation
Latest commit: recorded after this file is committed
Last completed task: Release test suite passed. Repository restore is the next proof.

## Counts

Complete: 4
Partial: 4
Failed: 0
Blocked: 5
Not started: the freight, payment, identity, audit, and anchoring product

Complete means a test command passed for that slice:

- Server health routes
- Client health client
- Backup library round trip, tamper checks, and retention selection
- Repository secret scan

Partial:

- Avalonia window compiles and lists four workspaces as not built. The button was not clicked in a live window.
- Docker Compose file exists and was not started.
- `db/migrations/0001_foundation.sql` exists and has not been applied.
- AGPL version 3 is the working license. The final legal review is still open.

Blocked:

- Product workflows, until `docs/source/Project_ClearHaul_Blueprint.docx` and `docs/source/CLEARHAUL_MASTER_BUILD_PROMPT.md` are present and read
- Docker Desktop, which is not installed and was not installed
- External-drive backup, until Michael provides the path
- Live hazmat operation, until qualified review
- Production XRP Ledger anchoring, until a separate written approval

## Features that work

- `GET /health/live` and `GET /health` on the test host, with the headers in the foundation contract
- Unknown routes return a problem response without a stack trace
- The Windows health client rejects a file address and reads a stub health response
- Encrypted backup create and restore for a sample directory
- A text scan for private keys and cloud access-key prefixes

## Partial features

- Windows shell
- Compose definition for PostGIS, Redis, and MinIO
- Foundation SQL file
- Public working license with review still open

## Blocked features

- Shipment, payment, compliance, and hazmat workflows
- A running database
- Off-computer backup
- XRP Ledger testnet or Mainnet

## Questions

No question is waiting. Answers are in QUESTIONS_FOR_MICHAEL.md. Decisions CH-D-0013 through CH-D-0021 record them.

## Tests

See TEST_RESULTS.md. Command exit code was 0. Passed 16, failed 0, skipped 0.

## Defects

- Avalonia 12.1.3 does not compile with the installed .NET 8 compiler. The client uses Avalonia 11.3.22.
- The MinIO and PostGIS image tags have not been pulled.
- The window button has not been operated by a person in this session.

## Security

No accounts, no TLS, no audit ledger, and no production secrets. The health routes are unauthenticated and return no freight data. See SECURITY_MODEL.md and RISK_REGISTER.md.

## Compliance

No legal rule is encoded. The first live pilot boundary is nonhazardous domestic dry-van freight. Hazmat code has not been started. See LEGAL_REVIEW_REQUIRED.md.

## Backup

Local destination: `C:\Users\17402\ClearHaul-Backups`
Repository restore: see BACKUP_LOG.md
Off-computer copy: not configured

## Next task

Read the blueprint and the master build prompt after they are placed in `docs/source`. Do not design product workflows before that.

## Resume

```powershell
Set-Location C:\Users\17402\projects\clearhaul
git switch milestone/m0-foundation
dotnet test ClearHaul.sln --configuration Release
```

## Files under active development

The foundation tree in this repository. No second workstream is open.
