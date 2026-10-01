# Database schema

## Applied

On 2026-10-01 a new local volume `clearhaul_clearhaul-postgres` initialized from `db/migrations`. PostgreSQL reports extensions `pgcrypto`, `plpgsql`, and `postgis`, schemas `app` and `audit`, and these twelve tables: `app.app_user`, `app.equipment_history_access`, `app.equipment_history_entry`, `app.ledger_entry`, `app.load_stop`, `app.load_tender`, `app.organization`, `app.rule_package`, `app.shipment`, `app.shipment_transition`, `app.user_role`, and `audit.audit_event`. The foundation server is not connected to this database. Do not delete the volume.

## Files

`db/migrations/0001_foundation.sql` creates the `postgis` and `pgcrypto` extensions and the empty `app` and `audit` schemas.

`db/migrations/0002_phase_zero.sql` adds organization, user, role, shipment, append-only transition, load tender, stop, simulated ledger, append-only equipment history, history access, append-only audit event, and rule package tables. Triggers refuse updates and deletes on the append-only tables and refuse shipment deletes. `approved_for_live` must stay false. Ledger rows must be marked simulated.

`db/seed/001_synthetic.sql` inserts two synthetic organizations and two synthetic users. It is not mounted by Compose and has not been applied.

Compose mounts `db/migrations` into the PostgreSQL init directory. That mount already ran on the current volume. Init scripts do not run again on the next start.

The in-process domain tests do not open PostgreSQL. A passing domain test is not a migration test.
