# Database schema

## Applied

Nothing. Docker Desktop is not installed. Compose has not been started. No database exists.

## Files

`db/migrations/0001_foundation.sql` creates the `postgis` and `pgcrypto` extensions and the empty `app` and `audit` schemas.

`db/migrations/0002_phase_zero.sql` adds organization, user, role, shipment, append-only transition, load tender, stop, simulated ledger, append-only equipment history, history access, append-only audit event, and rule package tables. Triggers refuse updates and deletes on the append-only tables and refuse shipment deletes. `approved_for_live` must stay false. Ledger rows must be marked simulated.

`db/seed/001_synthetic.sql` inserts two synthetic organizations and two synthetic users. It is not mounted by Compose and has not been applied.

Compose mounts `db/migrations` into the PostgreSQL init directory. That mount does nothing until a new database volume is created with Docker, which has not happened.

The in-process domain tests do not open PostgreSQL. A passing domain test is not a migration test.
