# ClearHaul

ClearHaul is a freight-operations project. This repository is a foundation checkpoint, not a working freight system.

The product blueprint and the master build prompt were not in the workspace when this repository was created. Product workflows are not implemented. Nothing in this repository moves freight, holds funds, checks compliance, or writes to a blockchain.

The authoritative status is [CURRENT_STATUS.md](CURRENT_STATUS.md). Questions for the product owner are in [QUESTIONS_FOR_MICHAEL.md](QUESTIONS_FOR_MICHAEL.md).

## What a developer can run

The .NET 8 SDK is required. From this directory:

```powershell
dotnet test ClearHaul.sln --configuration Release
```

The server, when present, listens on `http://127.0.0.1:5080`. Docker Compose is not running because Docker is not installed.

## License

The working license for the server and core is AGPL version 3. See [LICENSE](LICENSE) and [OPEN_SOURCE_GOVERNANCE.md](OPEN_SOURCE_GOVERNANCE.md). A final legal and open-source review is still open under decision CH-D-0014.

The public repository is https://github.com/Spike1976/clearhaul.
