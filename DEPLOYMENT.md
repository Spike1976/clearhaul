# Deployment

Nothing in this repository is deployed as a freight service.

The public Git repository is https://github.com/Spike1976/clearhaul. The default branch `main` is the foundation checkpoint and the static website. Phase Zero is on the local branch `milestone/m1-phase-zero` until it is reviewed and merged.

The health process, when started from its project, listens on `http://127.0.0.1:5080` only. It is not a production deployment. TLS, a database, Redis, object storage, and authentication are not configured.

Do not point this build at a real bank, a real verification provider, or a production ledger.
