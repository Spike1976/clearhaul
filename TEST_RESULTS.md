# Test results

Date: 2026-09-30
Command: `dotnet test ClearHaul.sln --configuration Release`
Exit code: 0

| Project | Passed | Failed | Skipped |
| --- | ---: | ---: | ---: |
| ClearHaul.Domain.Tests | 39 | 0 | 0 |
| ClearHaul.Server.Tests | 12 | 0 | 0 |
| ClearHaul.Client.Tests | 5 | 0 | 0 |
| ClearHaul.Backup.Tests | 4 | 0 | 0 |
| ClearHaul.Security.Tests | 1 | 0 | 0 |
| Total | 61 | 0 | 0 |

Domain tests include the Phase Zero marketplace rules and the new qualification and audit-hash checks. They do not open PostgreSQL.

Server tests use the in-process host. Six cover the original health contract. Six cover the development directory: organization isolation, suspension, signed-out qualification, driver maintenance, worker health, and authentication disabled when the directory flag is off. They do not prove a listening process on port 5080.

The Windows client process started and was stopped after three seconds. The window button was not clicked.

Backup m1-entry-20260930 restored with exit code 0. See BACKUP_LOG.md.

Not run: Docker Compose, PostgreSQL, Redis, MinIO, a marketplace end-to-end test, and a database restore.
