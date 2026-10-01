# Docker Desktop for local ClearHaul development

Decision CH-D-0017 approves Docker Desktop for local PostgreSQL, MinIO, and Redis. Decision CH-D-0031, recorded 2026-10-01, is Michael Stokes's permission to install Docker Desktop and start the local Compose stack. Installation had not been run when CH-D-0031 was recorded.

Docker was not installed when the foundation was prepared. The compose file can be reviewed before Docker is installed. Do not treat the database as running until `docker compose ps` shows the services healthy.

## Install, when you choose to

1. Download Docker Desktop for Windows from https://docs.docker.com/desktop/setup/install/windows-install/
2. Run the installer with an account that is allowed to install software.
3. Start Docker Desktop and wait until it says the engine is running.
4. Open a terminal in the ClearHaul repository.
5. Copy `.env.example` to `.env` if a compose file requires it. The sample password `dev-only-not-a-secret` is only for this local Docker database. Do not use it anywhere else.
6. Run `docker compose up -d` only after you have read the compose file.
7. Run `docker compose ps` and confirm each service is healthy before pointing the application at it.

The foundation server does not connect to those services yet. Starting Docker does not by itself create freight data.
