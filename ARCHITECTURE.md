# Architecture

Project ClearHaul. Decision CH-D-0020 uses ClearHaul as the working name and requires branding to stay configurable.

This document records planned boundaries from the engineering operating order and the foundation decisions. It does not report a working freight system. No product workflow works. No feature is complete.

The blueprint and the master build prompt were not in docs/source when this foundation was prepared. Decision CH-D-0013 says product workflows wait until both files are read. Status values are `present`, `partial`, or `not started`. An item is `present` only when it exists in the repository and this document is not using that word for an unverified project file. The integrator verifies builds and tests. This document does not.

Observed when this file was written: `src/ClearHaul.Client/ClearHaul.Client.csproj` was on disk. No server project file was on disk. PostgreSQL, Redis, object storage, the audit ledger, the XRP Ledger, payments, and freight workflows were not started.

## Planned boundaries

### ASP.NET Core server

Status: partial.

`src/ClearHaul.Server` exists. Release tests cover `GET /health/live` and `GET /health` on the test host. The process listens on `http://127.0.0.1:5080` when launched from its project file. It does not connect to a database, Redis, or object storage. TLS is not terminated by this process. That is a known gap.

### Avalonia Windows client

Status: partial.

`src/ClearHaul.Client` builds with Avalonia 11.3.22. Avalonia 12 does not compile on the installed .NET 8 compiler. Release tests cover the health client. The four workspaces are labels only. The window button was not clicked in a live session.

The operating order's first assignment includes a Windows client shell. Decision CH-D-0016 says the shell lists shipper, carrier, driver testing, and administrator workspaces as not built. Those labels are not product features. No workspace workflow works.

### PostgreSQL with PostGIS

Status: not started.

The operating order requires a database before schema changes can be trusted. The foundation health contract reports `postgres` as `NotConfigured` and forbids a database connection. Decision CH-D-0017 approves Docker Compose and forbids installing Docker Desktop without permission. Docker is not installed. No database is created. A Compose file or a SQL file, if one is on disk, does not start this boundary. No business table is specified.

### Object storage

Status: not started.

The foundation health contract reports `objectStorage` as `NotConfigured` and forbids an object-storage connection. The operating order names uploaded documents as a later feature. That feature is not designed here and does not work. Document contents are not placed on the XRP Ledger.

### Redis

Status: not started.

The foundation health contract reports `redis` as `NotConfigured` and forbids a Redis connection. Decision CH-D-0017 says Compose is the local method. Compose was not started. No Redis service is running as part of this project.

### Append-only audit ledger

Status: not started.

The operating order says the audit ledger is append-only once it exists, and that corrections are new events. No audit ledger exists. No audit table is specified. The foundation checkpoint has no auditable business action.

### XRP Ledger

Status: not started.

The operating order limits a future ledger connection to audit anchoring. Test-network use is required until production activation is separately approved. Decision CH-D-0019 names Michael Stokes as the only person who may approve production anchoring, in writing. Testnet work is allowed later and is not implemented. The foundation does not connect to the XRP Ledger. No signing seed exists in the project. Cryptocurrency payments, a ClearHaul token, stablecoin payments, and user wallets are out of scope.

Memos, if anchoring is built later, may carry only a format identifier, format version, batch identifier, Merkle root, event count, and batch time range. That list is a constraint from the operating order. It is not an implementation and it does not choose a transaction, encoding, fee, or account.

### Payments

Status: not started.

No payment workflow is specified. No payment route, payment state, or release rule is defined. Releasing funds is not authorized. The XRP Ledger boundary above is not a payment system.

### Freight workflows

Status: not started.

No freight workflow is specified. Business aggregates are not specified. See `DOMAIN_MODEL.md`. Decision CH-D-0015 limits the first live pilot to nonhazardous domestic dry-van freight and keeps hazmat disabled until professional review. It does not encode a freight rule. Nothing in this repository moves freight, holds funds, checks compliance, or writes to a blockchain.

## What a reader can rely on

`API_SPECIFICATION.md` lists the only specified operations, and only as a contract. `PERMISSION_MATRIX.md` says no users or roles exist. `ROADMAP.md` lists later milestones, each not started. `docs/contracts/foundation-health.md` remains the health contract. This file does not replace it.
