# ClearHaul decisions

Michael's decisions belong in this file after they are answered in QUESTIONS_FOR_MICHAEL.md.

Records CH-D-0001 through CH-D-0012 were engineering assumptions. Michael's later decisions CH-D-0013 through CH-D-0021 replace them where the later record says so. If two records conflict, the later record wins. None of these records authorize a hazardous-material rule, a production key, or production XRP Ledger anchoring.

DECISION CH-D-0001

Date: 2026-09-30
Decided by: Engineering lead, temporary assumption
Question reference: CH-0008
Decision: The blueprint and master build prompt were not available. Foundation work follows the engineering operating order only. Product workflows are not designed from guesswork.
Reason: Building invented shipment, payment, or compliance behavior would create a false product.
Affected modules: repository scope, architecture documents, server, client
Requires later professional review: No
Supersedes:
Implementation status: In effect

DECISION CH-D-0002

Date: 2026-09-30
Decided by: Engineering lead, temporary assumption
Question reference: CH-0001
Decision: The AGPL version 3 text may be stored as a provisional recommended license. It is not adopted. The project is not approved for publication.
Reason: The operating order requires a recommendation and forbids a silent final license choice.
Affected modules: LICENSE, OPEN_SOURCE_GOVERNANCE.md
Requires later professional review: Yes. License choice needs Michael's decision before publication.
Supersedes:
Implementation status: In effect

DECISION CH-D-0003

Date: 2026-09-30
Decided by: Engineering lead, temporary assumption
Question reference: CH-0002
Decision: Notes treat the first pilot boundary as nonhazardous domestic dry-van freight. No hazardous-material rules are written or encoded.
Reason: A narrow boundary is reversible. Invented hazardous-material rules are not.
Affected modules: compliance documents, future workflow scope
Requires later professional review: Yes, before any hazardous-material feature is specified.
Supersedes:
Implementation status: In effect for documentation only. No freight workflow exists.

DECISION CH-D-0004

Date: 2026-09-30
Decided by: Engineering lead, temporary assumption
Question reference: CH-0003
Decision: The Windows shell lists shipper, carrier, driver testing, and administrator workspaces as not built.
Reason: The operating order names those four workspaces. Labels can be removed. No workspace function is included.
Affected modules: ClearHaul.Client
Requires later professional review: No
Supersedes:
Implementation status: In effect when the client shell is present

DECISION CH-D-0005

Date: 2026-09-30
Decided by: Engineering lead, temporary assumption
Question reference: CH-0004
Decision: Docker Compose files may be written. They are not started. Docker is not installed, and no database is created.
Reason: A compose file is reversible. Installing services or creating a database without an answer is not required for the foundation checkpoint.
Affected modules: docker-compose.yml, database
Requires later professional review: No
Supersedes:
Implementation status: In effect

DECISION CH-D-0006

Date: 2026-09-30
Decided by: Engineering lead, temporary assumption
Question reference: CH-0005
Decision: Local encrypted backups go to C:\Users\17402\ClearHaul-Backups, outside the Git repository. No off-computer copy is made.
Reason: An off-computer destination was not named. Guessing a cloud account is prohibited.
Affected modules: backup tool, BACKUP_AND_RECOVERY.md, BACKUP_LOG.md
Requires later professional review: No
Supersedes:
Implementation status: In effect

DECISION CH-D-0007

Date: 2026-09-30
Decided by: Engineering lead, temporary assumption
Question reference: CH-0006
Decision: Nobody is authorized to activate production XRP Ledger anchoring. The foundation does not connect to the XRP Ledger. No signing seed exists in the project.
Reason: The operating order forbids a temporary approver for this decision.
Affected modules: audit anchoring, configuration, documentation
Requires later professional review: Yes, before any production anchoring design is activated.
Supersedes:
Implementation status: In effect

DECISION CH-D-0008

Date: 2026-09-30
Decided by: Engineering lead, temporary assumption
Question reference: CH-0007
Decision: Use ClearHaul as the working name in the window title and documents.
Reason: The name is easy to change before publication.
Affected modules: client shell, documentation
Requires later professional review: No
Supersedes:
Implementation status: In effect

DECISION CH-D-0009

Date: 2026-09-30
Decided by: Engineering lead
Question reference:
Decision: The repository lives at C:\Users\17402\projects\clearhaul. Git work uses the branches main, develop, and milestone/m0-foundation. Work is committed on milestone/m0-foundation, then merged. main is not edited directly. Git identity is not configured on this computer, and Git configuration must not be changed. Commits use the process environment author "ClearHaul Engineering <clearhaul-engineering@local>".
Reason: The home directory must not become a Git repository. A commit author is required for a checkpoint and must not depend on changing Git settings.
Affected modules: repository
Requires later professional review: No
Supersedes:
Implementation status: In effect

DECISION CH-D-0010

Date: 2026-09-30
Decided by: Engineering lead
Question reference:
Decision: The foundation targets the installed .NET 8 SDK, version 8.0.422. A newer SDK is not installed and is not downloaded during this checkpoint.
Reason: The build must use a compiler that is already present and can be verified.
Affected modules: global.json, all .NET projects
Requires later professional review: No
Supersedes:
Implementation status: In effect

DECISION CH-D-0011

Date: 2026-09-30
Decided by: Engineering lead
Question reference:
Decision: The foundation server exposes only GET /health/live and GET /health. It does not connect to PostgreSQL, Redis, or object storage. Those checks are reported as NotConfigured. The overall health status is Degraded. Unknown routes return a problem response without a stack trace. There is no Swagger page. The contract is docs/contracts/foundation-health.md.
Reason: Untested database connectivity would look like a working system. Docker and PostgreSQL were not available to prove a connection.
Affected modules: ClearHaul.Server, ClearHaul.Contracts, API specification
Requires later professional review: No
Supersedes:
Implementation status: In effect when the server is present

DECISION CH-D-0012

Date: 2026-09-30
Decided by: Engineering lead
Question reference:
Decision: Local backup archives are encrypted with AES-256-GCM. The content key is wrapped with Windows DPAPI for the current user and stored outside the repository. Milestone and release backups are not deleted by the rolling retention rule.
Reason: Git is not the only backup. Plaintext secrets must not be introduced. Off-computer encryption remains blocked by CH-0005.
Affected modules: ClearHaul.Backup, scripts/recovery
Requires later professional review: No
Supersedes:
Implementation status: In effect when the backup tool is present

DECISION CH-D-0013

Date: 2026-09-30
Decided by: Michael Stokes
Question reference: CH-0008
Decision: The engineering foundation may continue. Product workflows require both source documents. The required files are docs/source/Project_ClearHaul_Blueprint.docx and docs/source/CLEARHAUL_MASTER_BUILD_PROMPT.md. Product workflows are not designed until both have been read completely.
Reason: The blueprint and master build prompt were supposed to be provided. They were not in docs/source when this decision was recorded.
Affected modules: product scope, future workflows
Requires later professional review: No
Supersedes: CH-D-0001
Implementation status: In effect. Both source files were absent at the time of recording.

DECISION CH-D-0014

Date: 2026-09-30
Decided by: Michael Stokes
Question reference: CH-0001
Decision: AGPL version 3 is the approved working license for the ClearHaul server and core. The license stays provisional until a final legal and open-source review before a finished public release. Integration libraries may later use Apache 2.0 only if that choice is separately approved.
Reason: Michael selected the recommended license and kept a final review in front of a finished public release.
Affected modules: LICENSE, OPEN_SOURCE_GOVERNANCE.md, docs/licensing/PROVISIONAL_LICENSE_STATUS.md
Requires later professional review: Yes. Final legal and open-source review is still required.
Supersedes: CH-D-0002
Implementation status: In effect for the working license. Final review is not done.

DECISION CH-D-0015

Date: 2026-09-30
Decided by: Michael Stokes
Question reference: CH-0002
Decision: The first live pilot is nonhazardous domestic dry-van freight. Hazmat foundations are to be developed in parallel and must remain disabled for live transportation until qualified hazmat and legal professionals review and approve them.
Reason: Michael limited live freight and still asked for disabled hazmat groundwork.
Affected modules: future compliance and hazmat modules
Requires later professional review: Yes, before any live hazmat operation.
Supersedes: CH-D-0003
Implementation status: Pilot boundary is in effect. Hazmat design has not started. It waits until CH-D-0013 source documents have been read, so the hazmat model is not invented.

DECISION CH-D-0016

Date: 2026-09-30
Decided by: Michael Stokes
Question reference: CH-0003
Decision: The Windows client will contain four role-controlled workspaces: shipper, carrier, driver testing, and administrator. During the foundation milestone each unfinished workspace is marked not built. A workspace is replaced by a working screen only when that milestone is actually complete. Fake buttons are not allowed.
Reason: Michael confirmed the four workspaces and required honest unfinished labels.
Affected modules: ClearHaul.Client
Requires later professional review: No
Supersedes: CH-D-0004
Implementation status: In effect

DECISION CH-D-0017

Date: 2026-09-30
Decided by: Michael Stokes
Question reference: CH-0004
Decision: Docker Desktop and Docker Compose are the approved local-development method for PostgreSQL, MinIO, Redis, and supporting services. Compose is prepared now. Docker Desktop is not installed or changed without a separate permission. Plain-language install steps belong in docs/setup/DOCKER_DESKTOP.md.
Reason: Michael approved Docker for local development and forbade an unpermitted system install.
Affected modules: docker-compose.yml, docs/setup/DOCKER_DESKTOP.md
Requires later professional review: No
Supersedes: CH-D-0005
Implementation status: In effect. Docker Desktop is not installed. Compose has not been started.

DECISION CH-D-0018

Date: 2026-09-30
Decided by: Michael Stokes
Question reference: CH-0005
Decision: Local encrypted rolling backups are stored at C:\Users\17402\ClearHaul-Backups. Encrypted external-drive backups start when Michael provides the drive and path. The backup design must allow a second encrypted cloud destination later. Backups must not contain plaintext passwords, signing keys, personal information, or production secrets.
Reason: Michael chose an external drive and kept local backups until that path exists.
Affected modules: backup tool, BACKUP_AND_RECOVERY.md, BACKUP_LOG.md
Requires later professional review: No
Supersedes: CH-D-0006
Implementation status: Local destination is in effect. External-drive and cloud copies are not configured.

DECISION CH-D-0019

Date: 2026-09-30
Decided by: Michael Stokes
Question reference: CH-0006
Decision: Only Michael Stokes may approve production XRP Ledger anchoring, and the approval must be in writing. Testnet integration may be built and tested later. Production XRP Ledger access, Mainnet credentials, and production anchoring stay disabled until a separate written approval.
Reason: Michael named himself as the only production approver and separated testnet work from Mainnet.
Affected modules: future audit anchoring, configuration
Requires later professional review: Yes, before production activation.
Supersedes: CH-D-0007
Implementation status: In effect. No XRP Ledger connection is implemented in the foundation.

DECISION CH-D-0020

Date: 2026-09-30
Decided by: Michael Stokes
Question reference: CH-0007
Decision: ClearHaul is the working product and repository name. Branding stays configurable. The name is not hard-coded into database rules, API contracts, or business logic where configuration is the right place.
Reason: Michael confirmed the working name and required a later public rename to be possible.
Affected modules: client shell, configuration, future schema and API contracts
Requires later professional review: No
Supersedes: CH-D-0008
Implementation status: In effect

DECISION CH-D-0021

Date: 2026-09-30
Decided by: Michael Stokes
Question reference: CH-0001
Decision: The project tree is published to the existing GitHub repository https://github.com/Spike1976/clearhaul. That repository is public. The push includes the foundation work and excludes secrets. This direction does not complete the final legal and open-source review required by CH-D-0014.
Reason: Michael gave a later written direction to add the project to that repository.
Affected modules: git remote, public repository
Requires later professional review: Yes. CH-D-0014 final review remains open.
Supersedes:
Implementation status: In effect when the push succeeds.

DECISION CH-D-0022

Date: 2026-09-30
Decided by: Engineering lead, from the product assignment
Question reference: CH-0008
Decision: The blueprint and master build prompt were supplied as assignment text and stored as docs/source/Project_ClearHaul_Blueprint.md and docs/source/CLEARHAUL_MASTER_BUILD_PROMPT.md. A .docx binary was not attached. Phase Zero may be designed from those files. The markdown is not a legal opinion.
Reason: The product assignment said to read the blueprint and build. The required names were not present as binaries.
Affected modules: product scope, domain library, docs/source
Requires later professional review: No, for the act of recording the text. Yes, before live freight or payment claims.
Supersedes: the "files absent" limit in CH-D-0013
Implementation status: In effect

DECISION CH-D-0023

Date: 2026-09-30
Decided by: Engineering lead
Question reference:
Decision: Keep the existing .NET 8, ASP.NET Core, Avalonia, PostgreSQL, Redis, and MinIO direction. Do not start a parallel Next.js or NestJS application. The blueprint allows the existing stack to remain.
Reason: Replacing the tested foundation would discard working health, backup, and website code.
Affected modules: repository structure
Requires later professional review: No
Supersedes:
Implementation status: In effect

DECISION CH-D-0024

Date: 2026-09-30
Decided by: Engineering lead, from the blueprint
Question reference: CH-0003
Decision: The domain permission model uses the thirteen roles named in the blueprint. The four Windows workspace labels remain unfinished shell labels under CH-D-0016. They are not a substitute for the thirteen roles.
Reason: The blueprint is the later product document. The shell still must not show fake controls.
Affected modules: ClearHaul.Domain, ClearHaul.Client
Requires later professional review: No
Supersedes:
Implementation status: In effect for the domain library. The shell is unchanged.

DECISION CH-D-0025

Date: 2026-09-30
Decided by: Engineering lead
Question reference: CH-0002
Decision: The hazmat gate stores facts and returns UNABLE TO DETERMINE with QUALIFIED HAZMAT REVIEW REQUIRED on every live evaluation. No statute text is encoded. A synthetic fixture can be evaluated only when live mode is off. Live hazmat dispatch stays disabled.
Reason: CH-D-0015 asks for parallel hazmat groundwork without live activation, and the blueprint forbids an invented classification.
Affected modules: ClearHaul.Domain rules and hazmat gate
Requires later professional review: Yes, before any live hazmat operation.
Supersedes:
Implementation status: In effect

DECISION CH-D-0026

Date: 2026-09-30
Decided by: Engineering lead
Question reference:
Decision: Payment in Phase Zero is a sandbox adapter labeled sandbox-not-a-bank and a simulated double-entry ledger. Production funding results are rejected. Reserved funds cannot be withdrawn by the shipper after award. No commission percentage is calculated.
Reason: The blueprint forbids a homemade escrow account and forbids pretending a simulation is production.
Affected modules: ClearHaul.Domain payments
Requires later professional review: Yes, before any real money movement. The operator's legal role is also undecided.
Supersedes:
Implementation status: In effect

DECISION CH-D-0027

Date: 2026-09-30
Decided by: Engineering lead
Question reference: CH-0004
Decision: db/migrations/0002_phase_zero.sql is the Phase Zero schema. It is not applied. Compose is not started. Docker is not installed.
Reason: A schema file can be reviewed. Creating a database without Docker and without a migration test would overstate readiness.
Affected modules: database, docker-compose.yml
Requires later professional review: No
Supersedes:
Implementation status: In effect

DECISION CH-D-0028

Date: 2026-09-30
Decided by: Engineering lead
Question reference:
Decision: Phase Zero work stays on branch milestone/m1-phase-zero. It is not merged to main and it is not pushed unless Michael asks.
Reason: main is public and already contains the foundation checkpoint. This branch needs review before it becomes the default branch.
Affected modules: git
Requires later professional review: No
Supersedes:
Implementation status: In effect until a later merge is requested
