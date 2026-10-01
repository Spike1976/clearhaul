# Test results

Date: 2026-09-30
Command: `dotnet test ClearHaul.sln --configuration Release --no-restore`
Exit code: 0

| Project | Passed | Failed | Skipped |
| --- | ---: | ---: | ---: |
| ClearHaul.Server.Tests | 6 | 0 | 0 |
| ClearHaul.Client.Tests | 5 | 0 | 0 |
| ClearHaul.Backup.Tests | 4 | 0 | 0 |
| ClearHaul.Security.Tests | 1 | 0 | 0 |
| Total | 16 | 0 | 0 |

The server tests use the in-process test host. They do not prove a listening process on port 5080.

The client tests call `ServerHealthClient` with a stub handler. They do not click the window.

The backup tests use a temporary directory and an in-memory key. They do not by themselves prove a restore of this repository. That restore is recorded in BACKUP_LOG.md when it has been run.

The secret scan found no private-key block, Amazon-style access-key prefix, or certificate file in the tree it read. It does not scan ignored build output.

Not run: Docker Compose, PostgreSQL migration, installer, authentication, audit chain, XRP Ledger, and a live click of the window button.
