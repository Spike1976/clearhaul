# Environment setup

## What this machine can run

.NET SDK 8.0.422 is installed. From the repository:

```powershell
Set-Location C:\Users\17402\projects\clearhaul
dotnet test ClearHaul.sln --configuration Release
```

The health server project can be started later with:

```powershell
dotnet run --project src\ClearHaul.Server --configuration Release
```

It binds `http://127.0.0.1:5080`.

## What is not installed

Docker Desktop is not installed and was not installed. Compose is prepared and has not been started. PostgreSQL, Redis, and MinIO are not running. The migration files have not been applied. Install steps, if Michael approves an install, are in `docs/setup/DOCKER_DESKTOP.md`.

No Android SDK workflow is set up. No Node shipment application is part of this repository.
