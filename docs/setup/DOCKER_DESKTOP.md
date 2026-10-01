# Docker Desktop for local ClearHaul development

Decision CH-D-0017 approves Docker Desktop for local PostgreSQL, MinIO, and Redis. Decision CH-D-0031, recorded 2026-10-01, is Michael Stokes's permission to install Docker Desktop and start the local Compose stack. Installation had not been run when CH-D-0031 was recorded.

Docker Desktop 4.93.0 was installed on 2026-10-01. The engine observed in service was 29.8.1. On that date `docker compose ps` showed postgres, redis, and minio healthy. Do not treat the services as running on a later day until `docker compose ps` shows them healthy again.

## Install, when you choose to

1. Download Docker Desktop for Windows from https://docs.docker.com/desktop/setup/install/windows-install/
2. Run the installer with an account that is allowed to install software.
3. Start Docker Desktop and wait until it says the engine is running.
4. Open a terminal in the ClearHaul repository.
5. Copy `.env.example` to `.env` if a compose file requires it. The sample password `dev-only-not-a-secret` is only for this local Docker database. Do not use it anywhere else.
6. Run `docker compose up -d` only after you have read the compose file.
7. Run `docker compose ps` and confirm each service is healthy before pointing the application at it.

The Docker CLI is not on PATH. Prepend `C:\Program Files\Docker\Docker\resources\bin`. If `docker info` cannot find the pipe, start Docker Desktop and wait. Do not also run `wsl -d docker-desktop`. That attaches the same virtual disk and stops the engine.

Docker Hub removed `minio/minio`. Object storage in `docker-compose.yml` is pinned to `pgsty/silo:RELEASE.2026-09-16T00-00-00Z`. That image keeps the MinIO S3 API, the `MINIO_*` settings, the command `server /data --console-address :9001`, and `http://127.0.0.1:9000/minio/health/live`. Do not switch to a distroless tag. The health check calls `curl`. Do not run `docker compose down -v`.

The foundation server does not connect to those services yet. Starting Docker does not by itself create freight data. The first volume init applied migrations 0001 and 0002. The seed file is not mounted.
