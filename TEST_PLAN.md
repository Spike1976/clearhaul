# Foundation test plan

Date: 2026-09-30
Milestone: foundation
Repository: C:\Users\17402\projects\clearhaul

This plan follows docs/contracts/foundation-health.md and docs/engineering/AGENT_ASSIGNMENTS_M0.md. It does not add shipment, payment, identity, compliance, or blockchain checks. The blueprint and the master build prompt were not available and are not invented here.

## Evidence

No test result in this file is evidence. A sentence that says what a test proves is the intended check, not an outcome. This file does not record a pass, a fail, or any measured outcome. Results belong in TEST_RESULTS.md. This plan must not write TEST_RESULTS.md. The integrator writes that file after a run.

Commands named below are the commands the assignments require. Naming a command is not a run.

## Server

Project: tests/ClearHaul.Server.Tests (ClearHaul.Server.Tests)

Command named by the server assignment: `dotnet test tests/ClearHaul.Server.Tests/ClearHaul.Server.Tests.csproj --configuration Release`

The test host uses Microsoft.AspNetCore.Mvc.Testing and the environment name Testing. Testing skips the rate limiter so suite order does not depend on the limit of 60 requests per minute. These tests do not prove that limit, and they do not prove a 429 response.

| Planned test | What it proves |
| --- | --- |
| Live route | GET /health/live returns 200 with Content-Type application/json and a body whose only field is status Healthy. |
| Health route | GET /health returns 200 with Content-Type application/json, status Degraded, stage foundation, and version exactly 0.1.0-foundation. The checks are self Healthy, and postgres, redis, and objectStorage NotConfigured, with the descriptions in the health contract. Overall status is Degraded because a check is NotConfigured and none are Unhealthy. The response does not show a live database, Redis, or object-storage connection. |
| Unknown route | A path other than the two health routes returns 404 with Content-Type application/problem+json. The body contains title and status. It does not contain a stack trace, an internal source path, or an exception message. |
| Header check | Each covered response includes X-Content-Type-Options nosniff, X-Frame-Options DENY, Referrer-Policy no-referrer, Content-Security-Policy default-src 'none'; frame-ancestors 'none', and Cache-Control no-store. The Server header is absent. |
| Secret-leak check | The response does not contain the words password, stack trace, or exception. Health descriptions do not contain connection strings, passwords, file paths, exception text, or host names from configuration. |
| Repeated calls | Two calls to GET /health each return the contract health response. This does not prove the rate limit. |

## Client

Project: tests/ClearHaul.Client.Tests (ClearHaul.Client.Tests)

Command named by the client assignment: `dotnet test tests/ClearHaul.Client.Tests/ClearHaul.Client.Tests.csproj --configuration Release`

These tests target ServerHealthClient. That client accepts an HttpMessageHandler, allows only http and https, times out in five seconds, reads at most 65536 bytes, and returns a failed result instead of throwing for network and HTTP failures. The client project does not reference ClearHaul.Server.

| Planned test | What it proves |
| --- | --- |
| Health-client success | A successful health JSON body is returned as success. |
| HTTP failure | A non-success HTTP status becomes a failed result and does not throw. |
| Rejected file URI | A file URI is rejected and is not used as a health request. Only http and https are allowed. |
| Repeated calls | Two successive successful health calls both succeed. This does not prove the server rate limit. |

## Backup

Project: tests/ClearHaul.Backup.Tests (ClearHaul.Backup.Tests)

Command named by the backup assignment: `dotnet test tests/ClearHaul.Backup.Tests/ClearHaul.Backup.Tests.csproj --configuration Release`

These tests call the backup library with an in-memory key. They do not require Windows DPAPI. They are not a restoration of a real project backup.

| Planned test | What it proves |
| --- | --- |
| Round trip | Payload bytes encrypted with AES-256-GCM restore to the original bytes when the in-memory key and the recorded hashes still match. |
| Tampered payload | Restore fails after the ciphertext is changed. |
| Tampered hash | Restore fails after a recorded hash is changed. |
| Retention keeping milestone backups | Retention selection keeps the newest 48 manifests with retentionClass worktree. It never selects a manifest with retentionClass milestone or release. |

## Secret scan

Project: tests/ClearHaul.Security.Tests (ClearHaul.Security.Tests)

Command named by the security assignment: `dotnet test tests/ClearHaul.Security.Tests/ClearHaul.Security.Tests.csproj --configuration Release`

| Planned test | What it proves |
| --- | --- |
| Secret scan for private keys and cloud access-key prefixes | The scan walks parent directories until it finds QUESTIONS_FOR_MICHAEL.md, then scans that repository tree while ignoring bin, obj, and .git. The scan fails if it finds a private-key block or an Amazon-style access-key prefix. The exact placeholder dev-only-not-a-secret is allowed. The scan does not prove that every other secret form is absent. |

## Owned by the integrator

These two checks are owned by the integrator and are not run in this plan.

| Check | Mark |
| --- | --- |
| Solution-wide test. Command: `dotnet test ClearHaul.sln --configuration Release`. The integrator creates the solution file. When run, this is the integrated test of that solution, not a substitute for the project checks above. | Owned by the integrator. Not run in this plan. |
| One real backup restoration. This uses the backup tool on a real archive. It is not the in-memory library round trip. A backup is trusted only after a restore test. This plan does not perform that restoration and does not write BACKUP_LOG.md. | Owned by the integrator. Not run in this plan. |

## Not possible yet

These checks are not possible yet. They are not planned tests, and this file does not record them as passed or failed.

| Check | Why it is not possible yet |
| --- | --- |
| PostgreSQL migration applied | The foundation migration is not applied. The server has no connection setting that opens PostgreSQL, and it must not open one. Docker is not installed, so the database service is not started. |
| Docker health | Compose health checks are not executed. Docker is not installed, and compose is not started. |
| TLS | The foundation process listens on http://127.0.0.1:5080 only. It does not terminate TLS. |
| Authentication | The contract has no accounts, passwords, or sessions. The two health routes are unauthenticated. No other route exists. |
| Installer | No installer is part of this foundation. |
| Audit chain | The domain audit log refuses deletion in memory. PostgreSQL persistence and its append-only triggers have not been applied, so the database chain is not verified. |
| XRP Ledger confirmation | No XRP Ledger code is authorized in this milestone. There is no ledger confirmation to verify. |

## Out of scope for this plan

Rate-limit behavior outside the Testing environment is specified by the health contract and is not a planned test here. A browser shipment interface, a live bank, and a live hazmat decision are outside this plan.

## Phase Zero domain plan

Date: 2026-09-30. These checks run in `ClearHaul.Domain.Tests` against the in-process model. They do not open a database, a bank, or a regulatory service.

The automated scenarios cover unverified posting, inactive authority, expired insurance, unfunded award, hidden equipment history, unassigned trailer history, trailer substitution, washout mismatch, altered washout evidence, append-only history correction, offline packet read, hazmat quantity change, fixture placard change with a live refusal, missing classification, missing clearance for incompatible facts, missing permit data, missing endorsement, declared route restriction, withdrawal after pickup, detention proposal, one-line accessorial dispute, private compensation dispute, audit deletion refusal, suspended bidding, revoked signed link, concurrent edit, duplicate webhook, unavailable verification, bound rule version, and investigator conflict.

A synthetic rule fixture may return a labeled test decision only when the caller sets live evaluation off. Live evaluation returns UNABLE TO DETERMINE and QUALIFIED HAZMAT REVIEW REQUIRED. That fixture is not a regulation.
