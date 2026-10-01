# Milestone 0 agent assignments

Date: 2026-09-30
Integrator: principal engineer
Repository: C:\Users\17402\projects\clearhaul
Branch: milestone/m0-foundation

The blueprint and master build prompt were not available. Do not invent them. Do not implement shipment, payment, identity, compliance, or blockchain behavior.

No agent commits, pushes, changes Git configuration, or edits ClearHaul.sln. The integrator creates the solution and commits after review.

Shared forbidden files:

- QUESTIONS_FOR_MICHAEL.md
- DECISIONS.md
- CURRENT_STATUS.md
- CURRENT_HANDOFF.md
- TEST_RESULTS.md
- BACKUP_LOG.md
- CHANGELOG.md
- README.md
- Directory.Build.props
- global.json
- nuget.config
- .editorconfig
- .gitattributes
- docs/contracts/foundation-health.md
- docs/engineering/AGENT_ASSIGNMENTS_M0.md
- docs/source/

Security restrictions for every assignment:

- Do not create passwords, API keys, seeds, certificates, or connection strings with real credentials.
- The only allowed development placeholder password is the exact text dev-only-not-a-secret, and only in .env.example.
- Do not add a dependency with a floating version.
- Do not disable tests.
- Do not add a sample freight record or a person's real data.

## Server engineer

Objective: Build the ASP.NET Core foundation server described by docs/contracts/foundation-health.md.

Files owned:

- src/ClearHaul.Server/**
- src/ClearHaul.Contracts/**
- tests/ClearHaul.Server.Tests/**

Files you must not change: every file outside those three trees, including the client, backup tool, compose file, and solution.

Required inputs:

- docs/contracts/foundation-health.md
- Directory.Build.props
- global.json

Required output:

- A Web SDK project that builds with the installed .NET 8 SDK.
- Public partial class Program so tests can host it.
- GET /health/live and GET /health only.
- Safe problem responses for unknown routes and for unhandled exceptions.
- The security headers and rate-limit behavior in the contract.
- Environment name Testing skips the rate limiter.
- launchSettings.json binds http://127.0.0.1:5080 only.
- No Swagger, no Entity Framework, no database client, no Redis client, no object-storage client.
- Contracts project contains the health response records. The server uses those records.
- xUnit tests using Microsoft.AspNetCore.Mvc.Testing for the happy path, an unknown route, a response that contains none of the words password, stack trace, or exception, and two repeated health calls.
- Package versions are explicit. Generate packages.lock.json with dotnet restore --use-lock-file.

Tests required: dotnet test tests/ClearHaul.Server.Tests/ClearHaul.Server.Tests.csproj --configuration Release

Definition of done: that test command exits 0, and the server contains no unused sample weather endpoint.

Documentation required: XML comments are optional. Do not add a new markdown file.

## Windows client engineer

Objective: Build an Avalonia window that tells the truth and can call the foundation health route.

Files owned:

- src/ClearHaul.Client/**
- tests/ClearHaul.Client.Tests/**

Files you must not change: the server, contracts, solution, and every markdown file.

Required inputs:

- docs/contracts/foundation-health.md
- Decision CH-D-0004 in DECISIONS.md, which you may read and must not edit.

Required output:

- An Avalonia desktop application for net8.0.
- Install Avalonia templates only if you generate the project that way. Pin every package version. No wildcard versions.
- Window title ClearHaul.
- One sentence that shipment, payment, and compliance workflows are not built.
- Four text labels, not buttons: Shipper workspace — not built; Carrier workspace — not built; Driver testing workspace — not built; Administrator workspace — not built.
- A sentence that those labels follow temporary decision CH-D-0004.
- A server-address box defaulting to http://127.0.0.1:5080.
- One button, Check server health, which calls ServerHealthClient.
- ServerHealthClient accepts an HttpMessageHandler, allows only http and https, times out in five seconds, reads at most 65536 bytes, and returns a failed result instead of throwing for network and HTTP failures.
- The client does not reference ClearHaul.Server.
- Use theme brushes rather than hard-coded black and white. Set AutomationProperties.Name on the address box, the button, and the result text. Body text is at least 14 points.
- xUnit tests for a successful health JSON body, a non-success HTTP status, an invalid file URI, and two successful calls.
- packages.lock.json for both projects.

Tests required: dotnet test tests/ClearHaul.Client.Tests/ClearHaul.Client.Tests.csproj --configuration Release

Definition of done: that test command exits 0, and the only action button performs a real health request.

Documentation required: none beyond short comments on the scheme check.

## DevOps and database engineer

Objective: Prepare the local dependency definition, the encrypted backup library, and continuous-integration build.

Files owned:

- docker-compose.yml
- .env.example
- infra/**
- db/**
- scripts/**
- tools/ClearHaul.BackupCli/**
- src/ClearHaul.Backup/**
- tests/ClearHaul.Backup.Tests/**
- .github/workflows/ci.yml
- BACKUP_AND_RECOVERY.md
- DATABASE_SCHEMA.md
- RELEASE_CHECKLIST.md

Files you must not change: the server, the client, BACKUP_LOG.md, and the solution.

Required inputs:

- Decision CH-D-0005, CH-D-0006, and CH-D-0012 in DECISIONS.md.
- Docker is not installed. Do not install it. Do not run docker compose up.

Required output:

- Compose services for PostGIS, Redis, and MinIO, with health checks, named volumes, and no published default password except the placeholder below.
- .env.example uses POSTGRES_PASSWORD=dev-only-not-a-secret and MINIO_ROOT_PASSWORD=dev-only-not-a-secret. Say these values are local placeholders and must not be used outside Docker on this machine.
- db/migrations/0001_foundation.sql creates the postgis and pgcrypto extensions and the empty schemas app and audit. It creates no business table. DATABASE_SCHEMA.md describes only that file and says it has not been applied.
- A backup library in src/ClearHaul.Backup with AES-256-GCM payload encryption.
- Manifest fields: backupId, createdAt, projectCommit, branch, includedComponents, excludedComponents, databaseMigrationVersion, encryption status and algorithm, toolVersion, restoreInstructions, verificationResult, file hashes, payload file name, payload ciphertext hash, retentionClass.
- Excluded directory names: bin, obj, .git, node_modules, TestResults, .vs.
- Retention selection keeps the newest 48 manifests with retentionClass worktree and never selects milestone or release. Put this rule in testable library code.
- CLI create wraps the content key with Windows DPAPI into a key file outside the archive. CLI restore accepts only DPAPI key files. Tests call the library with an in-memory key and do not require DPAPI.
- A tampered payload or a tampered hash fails restore.
- scripts/recovery/New-ClearHaulBackup.ps1 stages a copy of a source directory, optionally adds a Git bundle when Git is available, and calls the CLI. It refuses to write inside the source Git repository. The folder is named recovery because a standard .NET gitignore ignores directories named Backup.
- scripts/recovery/Restore-ClearHaulBackup.ps1 calls the CLI and does not delete the source.
- GitHub Actions workflow on windows-latest restores locked packages and runs dotnet test ClearHaul.sln --configuration Release. The solution file is created by the integrator after your work.
- RELEASE_CHECKLIST.md lists the checks and marks each one as not run.
- Do not write BACKUP_LOG.md.

Tests required: dotnet test tests/ClearHaul.Backup.Tests/ClearHaul.Backup.Tests.csproj --configuration Release

Definition of done: the backup tests exit 0. Compose is valid YAML. The migration has not been applied and the document says so.

Documentation required: BACKUP_AND_RECOVERY.md and DATABASE_SCHEMA.md, without claiming a restore already succeeded.

## System architect

Objective: Write the architecture boundary for this foundation without inventing a freight domain.

Files owned:

- ARCHITECTURE.md
- DOMAIN_MODEL.md
- API_SPECIFICATION.md
- PERMISSION_MATRIX.md
- ROADMAP.md

Files you must not change: source code, the health contract, and DECISIONS.md.

Required inputs:

- docs/contracts/foundation-health.md
- DECISIONS.md
- docs/source/README.md

Required output:

- ARCHITECTURE.md states the planned boundaries from the operating order: ASP.NET Core server, Avalonia Windows client, PostgreSQL with PostGIS, object storage, Redis, append-only audit records, and XRP Ledger test-network anchoring later. Mark each item as present, partial, or not started. Only the items that exist in the repository may be marked present.
- DOMAIN_MODEL.md says business aggregates are not specified because the blueprint is missing. Do not invent fields, states, or tables.
- API_SPECIFICATION.md matches the health contract and lists no other operations.
- PERMISSION_MATRIX.md says no users or roles are implemented. The two health routes are unauthenticated. No other route exists.
- ROADMAP.md is an ordered list of future milestones taken from the operating order. Each milestone says not started. Do not assign dates or percentages.

Tests required: none. Do not claim tests passed.

Definition of done: a reader can tell what exists and what does not. No sentence says a product workflow works.

Documentation required: the five files above.

## Security and privacy specialist

Objective: Record the foundation threat model, security gaps, and privacy boundaries without claiming missing controls are done.

Files owned:

- SECURITY_MODEL.md
- THREAT_MODEL.md
- SECURITY.md
- RISK_REGISTER.md
- docs/privacy/DATA_GOVERNANCE.md
- tests/ClearHaul.Security.Tests/**

Files you must not change: application source, compose files, and QUESTIONS_FOR_MICHAEL.md.

Required inputs:

- docs/contracts/foundation-health.md
- DECISIONS.md

Required output:

- SECURITY_MODEL.md lists controls the operating order requires. Mark implemented controls only when a file in the repository implements them. Mark everything else as not implemented.
- THREAT_MODEL.md names foundation threats: secret leakage, unauthenticated future routes, backup key loss, local HTTP without TLS, and a missing off-computer backup. Do not invent a completed mitigation.
- SECURITY.md describes how to report a vulnerability privately to Michael Stokes. No public disclosure address exists yet.
- RISK_REGISTER.md uses rows with id, risk, current state, and what would reduce it. Include the missing blueprint, missing Docker, DPAPI key tied to one Windows user, and no off-computer backup.
- docs/privacy/DATA_GOVERNANCE.md says the foundation stores no personal data and no shipment documents. Future classes are named only as future work.
- tests/ClearHaul.Security.Tests scans the repository, ignoring bin, obj, and .git, and fails on private-key blocks or Amazon-style access-key prefixes. It allows the exact placeholder dev-only-not-a-secret.
- The scan walks parents until it finds QUESTIONS_FOR_MICHAEL.md.

Tests required: dotnet test tests/ClearHaul.Security.Tests/ClearHaul.Security.Tests.csproj --configuration Release

Definition of done: the scan exits 0 against the current tree, and the documents do not call missing controls complete.

Documentation required: the markdown files listed above.

## Transportation compliance specialist

Objective: Record what must be reviewed by qualified people. Do not write legal rules.

Files owned:

- LEGAL_REVIEW_REQUIRED.md
- COMPLIANCE_SOURCE_REGISTER.md

Files you must not change: source code and DECISIONS.md.

Required inputs:

- DECISIONS.md decision CH-D-0003
- docs/source/README.md

Required output:

- A list of subjects that need an attorney, a hazardous-material professional, or another qualified reviewer before rules are encoded. Include carrier qualification, freight documents, hazardous materials, payment release, data retention, and audit retention.
- State that no statute, regulation, or agency rule has been verified for this project.
- Do not cite a section number. Do not paraphrase a legal requirement as if it were established.
- COMPLIANCE_SOURCE_REGISTER.md has columns for subject, source, reviewer, and status. Every status is "not started".

Tests required: none.

Definition of done: both files exist and contain no invented legal rule.

Documentation required: the two files above.

## Documentation and open-source governance specialist

Objective: Write the contributor and governance documents for an unpublished project.

Files owned:

- OPEN_SOURCE_GOVERNANCE.md
- CONTRIBUTING.md
- CODE_OF_CONDUCT.md
- GOVERNANCE.md
- THIRD_PARTY_NOTICES.md
- docs/licensing/PROVISIONAL_LICENSE_STATUS.md

Files you must not change: LICENSE, README.md, and source code.

Required inputs:

- Decision CH-D-0002
- Question CH-0001

Required output:

- OPEN_SOURCE_GOVERNANCE.md says the project is not published and the license is not adopted.
- CONTRIBUTING.md forbids secrets, personal data, real shipment files, and disabling tests. It states that main is not a direct work branch.
- CODE_OF_CONDUCT.md is the Contributor Covenant version 2.1. Download the covenant text from https://www.contributor-covenant.org/version/2/1/code_of_conduct.md and keep its text intact. Add one line above it that says reports go to Michael Stokes because no public email is chosen.
- GOVERNANCE.md says Michael Stokes is the product owner and can replace an engineering assumption by answering QUESTIONS_FOR_MICHAEL.md. Engineers do not adopt legal or financial rules.
- THIRD_PARTY_NOTICES.md says notices are incomplete until the first dependency report is generated. Do not invent package lists.
- The provisional license file points at question CH-0001 and does not say the license is adopted.

Tests required: none.

Definition of done: the files exist and do not grant publication or an adopted license.

Documentation required: the files listed above.

## Not assigned in this milestone

Payment, XRP Ledger, and quality-assurance implementation agents are not assigned separate file trees. The integrator performs the test run and writes TEST_RESULTS.md. No payment code and no XRP Ledger code are authorized.
