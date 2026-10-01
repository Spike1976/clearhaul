# Changelog

## Unreleased

- Foundation checkpoint for the public repository. No freight release.
- Working license is AGPL version 3, with final legal review still open.
- Health server, Windows shell, encrypted backup library, and Compose file added.
- Phase Zero domain library added on `milestone/m1-phase-zero`: state machines, permissions, simulated ledger, equipment-history access, and a hazmat gate that does not invent a decision. No shipment HTTP API. Migration `0002` is not applied.
- Milestone 2 was not started. The entry gate failed without Docker. A development identity directory, qualification checks, and an audit hash chain were added on `milestone/m1-entry-repair`.
- Milestone 3 was not started. The entry gate failed because Milestone 2 is not persisted and the tag `clearhaul-m2-marketplace` does not exist. Privacy and cost questions CH-0010 through CH-0018 are open. Source backup `m3-entry-20260930` restored with a verified manifest and contains no database.
- On 2026-10-01 Michael Stokes answered CH-0009 through CH-0018. The answers are CH-D-0031 through CH-D-0040. Docker may be installed. Marketplace and equipment workflows are still not built.
- The local milestone record was combined with GitHub `main` at `61a7c8e`. The public website and the Milestone 4 and Milestone 5 orders are in the same tree. No product workflow was optimized into a finished feature.
- Milestone 6, Controlled Pilot and Production Readiness, was added from GitHub commit `952dfdb`. The pilot has not started.
- On 2026-10-01 Docker Desktop 4.93.0 started the local Compose stack. PostgreSQL applied migrations 0001 and 0002. Redis and object storage became healthy. Object storage uses pinned `pgsty/silo:RELEASE.2026-09-16T00-00-00Z` because Docker Hub removed `minio/minio`. The foundation server is not connected. Marketplace and equipment workflows are still not built.
