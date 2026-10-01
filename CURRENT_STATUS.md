# Current status

Date: 2026-10-01
Current milestone: Milestone 3 entry gate. Equipment assignment is not started. CH-0009 through CH-0018 are answered.
Current branch: milestone/m3-entry-gate
Published repository: https://github.com/Spike1976/clearhaul
Last completed task: Docker Desktop 4.93.0 is installed. Local Compose is healthy. The Milestone 3 entry gate still fails because Milestone 2 is not built.

## Entry gate

Milestone 3 requires Milestones 0, 1, and 2 to pass before any equipment-assignment feature work. This gate failed.

| Check | Result |
| --- | --- |
| Clean checkout builds | Passed. Release suite: 61 passed, 0 failed, 0 skipped. |
| Docker Compose starts | Passed on 2026-10-01. Engine 29.8.1. `docker compose up -d` exit 0. A later shell still saw the engine. |
| PostgreSQL healthy | Passed. `clearhaul-postgres-1` is healthy. Extensions `pgcrypto` and `postgis` exist. Schemas `app` and `audit` exist. Twelve tables from migrations 0001 and 0002 exist. The seed file was not applied. |
| Redis healthy | Passed. `clearhaul-redis-1` is healthy and `redis-cli ping` returned PONG. |
| MinIO healthy | Passed for the object-storage service. Docker Hub no longer serves `minio/minio`. The service uses `pgsty/silo:RELEASE.2026-09-16T00-00-00Z`. `http://127.0.0.1:9000/minio/health/live` returned 200. |
| Windows client starts | Passed. The Release process stayed running for three seconds and was then stopped. The health button was not clicked. |
| Authentication works | Partial. A development directory can sign a fictional user in during tests. It is off unless `Foundation:DevDirectory` is true. There is no production identity provider. |
| Organization isolation works | Passed in the test host. A shipper cannot read another shipper. |
| Shipper and carrier verification | Partial. Qualification is calculated in memory from stored test records. It is not a live external check and it is not in PostgreSQL. |
| Driver and equipment records | Partial. The test host can store a driver or equipment label on the caller's organization. Tractor, trailer, authorization, and cargo-history entities are not persisted. |
| Standardized load tenders | Partial. The in-process domain can validate a tender. There is no tender HTTP API and no database row. |
| Marketplace search uses persisted data | Failed. No marketplace search exists. No database is connected. |
| Carrier bidding works | Failed. No bid store and no bid API exist. |
| Conditional carrier selection works | Partial in memory only. `SelectCarrier` is a domain method. The server does not expose it. |
| Funding sandbox works | Partial in memory only. The ledger is labeled sandbox-not-a-bank. There is no funding HTTP callback. |
| Selected load enters EQUIPMENT_ASSIGNMENT_PENDING | Failed as a persisted workflow. The domain state machine has the state. No load is stored. |
| Permission tests pass | Partial. Development-directory and in-process permission checks pass. The Milestone 2 permission suite was not written. |
| Concurrency tests pass | Failed. The Milestone 2 and Milestone 3 concurrency suites were not written. |
| Audit events are append-only | Partial. New events form a hash chain in memory. Migration 0002 is applied on the local volume. Its append-only triggers were not exercised with an update or delete in this session. |
| Milestone 2 backup restores | Failed. No Milestone 2 backup exists. Source backups m0-foundation-20260930 and m1-entry-20260930 restored earlier and contain no database. |
| Tag clearhaul-m2-marketplace exists | Failed. `git tag -l` returned no tags. |
| CURRENT_STATUS.md reflects reality | This file. |

Marketplace implementation has not started. Decision CH-D-0029. Equipment-assignment implementation has not started. Decision CH-D-0030. CH-0009 through CH-0018 are answered. The local database does not by itself make Milestone 2 pass.

## Counts

The counts below are the 22 Milestone 3 entry checks in the table. The gate fails if any required check fails.

Passed: 8. The Release build, Docker Compose, PostgreSQL, Redis, object storage, the Windows client process, organization isolation in the test host, and this status file.
Failed: 6. Persisted marketplace search, bidding, persisted equipment-assignment state, the concurrency suite, the Milestone 2 backup, and the Milestone 2 tag.
Partial: 8. Authentication, verification, driver and equipment labels, load tenders, in-memory carrier selection, in-memory funding, permission checks, and the in-memory audit chain.
Blocked: Milestone 3, until every failed and partial check required by the order passes.

Release suite on this branch: 61 passed, 0 failed, 0 skipped. See TEST_RESULTS.md.

Source backup m3-entry-20260930 was created from commit 305f799 and restored with exit code 0. The manifest result is verified. CURRENT_STATUS.md hashes matched. The backup contains no database. That backup does not satisfy the Milestone 2 backup check.

## Questions

CH-0009 through CH-0018 were answered by Michael Stokes on 2026-10-01. The record is CH-D-0031 through CH-D-0040.

Docker Desktop may be installed. The ordinary history disclosure is the previous three loads within 90 days. Categories are shown as facts, with no automatic safety warning. Interactive history access lasts 72 hours after delivery, and the approval snapshot is kept. A cleaning request binds the carrier only from the original tender or a written change order. A named washout facility needs a separate written agreement for that load. The accepted bid or a written change order names the payer. Carrier cleaning uploads stay carrier declarations until an authorized shipper, a facility integration, or an administrator confirms them. A trailer rejection may create a report and does not by itself change the carrier's rating.

No question is waiting. The longer retention period for safety incidents and legally required records is not named. The dispute procedure that can extend history access is not defined. Live hazmat warning rules still need a qualified reviewer.

## Tests

See TEST_RESULTS.md.

## Defects

- The Milestone 3 entry gate cannot pass until Milestone 2 is persisted and tagged.
- The foundation server still reports postgres, redis, and objectStorage as NotConfigured. It is not connected to the healthy Compose services.
- `docker-compose.yml` pins `pgsty/silo:RELEASE.2026-09-16T00-00-00Z` because Docker Hub removed `minio/minio`. Do not run `docker compose down -v`.
- The development authentication directory is for tests and local development. It is not a production sign-in system.
- No equipment-assignment, protected-history, washout, or approval API exists.
- Avalonia 12.1.3 still does not compile on this SDK. The client remains on Avalonia 11.3.22.

## Next task

Build Milestone 2 against the local database. Milestone 3 stays unstarted until Milestone 2 passes and the tag `clearhaul-m2-marketplace` exists. Do not point the foundation health contract at Compose until `HealthEndpointTests` and `docs/contracts/foundation-health.md` change together.

The blueprint file `docs\source\Project_ClearHaul_Blueprint.docx` is still absent. `docs\source\CLEARHAUL_MASTER_BUILD_PROMPT.md` is present. `docs\source\MILESTONE_4_ENGINEERING_ORDER.md`, `docs\source\MILESTONE_5_ENGINEERING_ORDER.md`, and `docs\source\MILESTONE_6_ENGINEERING_ORDER.md` are present and not implemented. Michael asked for this record to be backed up on GitHub.

## Resume

```powershell
Set-Location C:\Users\17402\projects\clearhaul
git switch milestone/m3-entry-gate
dotnet test ClearHaul.sln --configuration Release
```
