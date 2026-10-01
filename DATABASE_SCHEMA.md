# Database schema

The only migration file is `db/migrations/0001_foundation.sql`.

It creates the PostgreSQL extensions `postgis` and `pgcrypto`, and the empty schemas `app` and `audit`. It creates no business table, no audit table, and no index.

This migration has not been applied. Docker Desktop is not installed, and `docker compose up` was not run. No database exists for this project.

Decision CH-D-0017 approves Docker Compose as the local method. The compose file mounts the migration into a new PostgreSQL data directory's init folder. That mount does nothing until Docker is installed and compose is started with permission.
