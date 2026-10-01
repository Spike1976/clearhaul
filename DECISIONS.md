# ClearHaul decisions

Michael's decisions belong in this file after they are answered in QUESTIONS_FOR_MICHAEL.md.

The records below are engineering assumptions made on 2026-09-30 because the foundation could not wait. They are reversible. They are not Michael's product decisions. None of them authorize a legal conclusion, a financial rule, a hazardous-material rule, a production key, or a production XRP Ledger connection.

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
Affected modules: ClearHaul.Backup, scripts/backup
Requires later professional review: No
Supersedes:
Implementation status: In effect when the backup tool is present
