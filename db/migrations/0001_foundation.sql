-- Foundation migration. This file has not been applied.
-- It creates extensions and empty schemas only. It creates no business table.

CREATE EXTENSION IF NOT EXISTS postgis;
CREATE EXTENSION IF NOT EXISTS pgcrypto;

CREATE SCHEMA IF NOT EXISTS app;
CREATE SCHEMA IF NOT EXISTS audit;

COMMENT ON SCHEMA app IS 'Future application data. No business tables in the foundation migration.';
COMMENT ON SCHEMA audit IS 'Future append-only audit ledger. No audit tables in the foundation migration.';
