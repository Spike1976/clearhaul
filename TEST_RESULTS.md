# Test results

Date: 2026-09-30
Command: `dotnet test ClearHaul.sln --configuration Release`
Branch: milestone/m3-entry-gate
Exit code: 0

| Project | Passed | Failed | Skipped |
| --- | ---: | ---: | ---: |
| ClearHaul.Domain.Tests | 39 | 0 | 0 |
| ClearHaul.Server.Tests | 12 | 0 | 0 |
| ClearHaul.Client.Tests | 5 | 0 | 0 |
| ClearHaul.Backup.Tests | 4 | 0 | 0 |
| ClearHaul.Security.Tests | 1 | 0 | 0 |
| Total | 61 | 0 | 0 |

Passed: 61. Failed: 0. Partial: 0. Blocked: the Milestone 3 equipment workflow was not tested because the entry gate failed.

Domain tests include the Phase Zero marketplace rules and the qualification and audit-hash checks. They do not open PostgreSQL.

Server tests use the in-process host. Six cover the original health contract. Six cover the development directory: organization isolation, suspension, signed-out qualification, driver maintenance, worker health, and authentication disabled when the directory flag is off. They do not prove a listening process on port 5080.

The Windows client Release process was started after this suite and stopped after three seconds. The window button was not clicked.

Not run: Docker Compose, PostgreSQL, Redis, MinIO, marketplace end-to-end, equipment-assignment end-to-end, substitution end-to-end, Milestone 3 security and concurrency suites, and a database restore. No Milestone 2 backup exists to restore.
