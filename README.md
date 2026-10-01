# ClearHaul

ClearHaul is a freight-operations project. The code in this repository is a foundation plus an in-process Phase Zero domain model. It is not a working freight marketplace.

The domain model can validate a load tender, move a shipment through tested states, keep a simulated ledger, hide equipment history, and refuse an invented hazmat answer. It does not move freight, hold real funds, call a bank, or write to a blockchain. Shipment behavior is not on the HTTP server.

The authoritative status is [CURRENT_STATUS.md](CURRENT_STATUS.md). Questions for the product owner are in [QUESTIONS_FOR_MICHAEL.md](QUESTIONS_FOR_MICHAEL.md).

## What a developer can run

The .NET 8 SDK is required. From this directory:

```powershell
dotnet test ClearHaul.sln --configuration Release
```

The server listens on `http://127.0.0.1:5080` when that project is started. Docker Compose is not running because Docker Desktop is not installed. Install steps are in [docs/setup/DOCKER_DESKTOP.md](docs/setup/DOCKER_DESKTOP.md).

## License

The working license for the server and core is AGPL version 3. See [LICENSE](LICENSE) and [OPEN_SOURCE_GOVERNANCE.md](OPEN_SOURCE_GOVERNANCE.md). A final legal and open-source review is still open under decision CH-D-0014.

The public repository is https://github.com/Spike1976/clearhaul.

## Project website

The repository also contains the static website that was already on `main`: `index.html`, `styles.css`, `script.js`, `assets/favicon.svg`, and the GitHub Pages workflows.

Preview it locally:

```powershell
python -m http.server 8080
```

Then open `http://localhost:8080`.

GitHub Pages publishes that site from the `main` branch through `.github/workflows/pages.yml`. The site uses no external JavaScript packages, fonts, analytics, or trackers.
