# Test results

Date: 2026-09-30
Command: `dotnet test ClearHaul.sln --configuration Release`
Exit code: 0

| Project | Passed | Failed | Skipped |
| --- | ---: | ---: | ---: |
| ClearHaul.Domain.Tests | 36 | 0 | 0 |
| ClearHaul.Server.Tests | 6 | 0 | 0 |
| ClearHaul.Client.Tests | 5 | 0 | 0 |
| ClearHaul.Backup.Tests | 4 | 0 | 0 |
| ClearHaul.Security.Tests | 1 | 0 | 0 |
| Total | 52 | 0 | 0 |

The domain tests construct the in-process model. They do not open PostgreSQL, Redis, MinIO, a bank, or a verification service. The 30 blueprint scenarios are covered there, along with catalog, ledger, retention, and permission checks. Live rule evaluation asserts UNABLE TO DETERMINE. Fixture decisions are requested with live evaluation turned off.

The server tests use the in-process test host. They do not prove a listening process on port 5080.

The client tests call `ServerHealthClient` with a stub handler. They do not click the window.

The backup tests use a temporary directory and an in-memory key. A separate restore of the foundation commit succeeded. See BACKUP_LOG.md entry m0-foundation-20260930. That archive does not contain Phase Zero.

The secret scan found no private-key block, Amazon-style access-key prefix, or certificate file in the tree it read. It does not scan ignored build output.

Not run: Docker Compose, PostgreSQL migration `0001` or `0002`, installer, authentication, a live click of the window button, and XRP Ledger.
