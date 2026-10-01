# Project ClearHaul engineering operating order

Captured: 2026-09-30

This file is a structured record of the operating order supplied on 2026-09-30. It is not a verbatim transcript. If Michael Stokes writes a newer direction, the newer direction wins. This file is not the product blueprint. The blueprint and the master build prompt were not included with the order.

## Authority order

1. Current written direction from Michael Stokes
2. This engineering operating order
3. Project ClearHaul blueprint
4. Project ClearHaul master build prompt
5. Recorded decisions in DECISIONS.md
6. Approved architecture documents
7. Existing implementation conventions
8. Reasonable documented engineering assumptions

An old decision is not used after Michael replaces it.

## How the work is run

The implementation lead inspects supplied documents and source before changes, preserves valid work, plans each milestone, uses specialist agents when the environment supports them, keeps questions in QUESTIONS_FOR_MICHAEL.md, maintains rolling backups, and does not call unfinished work complete.

Agents receive an objective, owned files, forbidden files, inputs, outputs, tests, a definition of done, security restrictions, and documentation requirements. Agents do not change shared contracts or database schemas on their own. The integrator reviews agent output, runs the integrated tests, and rejects unverifiable work.

Specialist perspectives named by the order are: system architect, server engineer, Windows client engineer, database engineer, security engineer, audit and XRPL engineer, transportation compliance specialist, payment and ledger specialist, quality assurance engineer, devops and reliability engineer, privacy and data-governance specialist, and documentation and open-source governance specialist.

If an agent did not run, the record must say so. A perspective performed by the integrator is labeled as such.

## Question notepad

Questions that need Michael are written in plain language in QUESTIONS_FOR_MICHAEL.md and opened in Windows Notepad at the start of a session. Unanswered questions are reviewed before new work. At most five active questions stay open unless there is an emergency. The first session exceeded that cap because the order required seven questions and the blueprint was missing.

Each question uses the CH-0000 structure from the order: status, priority, area, plain-language question, why it matters, recommended choice, choices with effects, default, Michael's answer, and decision recorded.

Defaults are not used for an irreversible financial, legal, privacy, or safety-critical choice. Answered questions are copied to DECISIONS.md.

## Mandatory root documents

README.md, CURRENT_STATUS.md, QUESTIONS_FOR_MICHAEL.md, DECISIONS.md, ROADMAP.md, ARCHITECTURE.md, DOMAIN_MODEL.md, DATABASE_SCHEMA.md, API_SPECIFICATION.md, PERMISSION_MATRIX.md, SECURITY_MODEL.md, THREAT_MODEL.md, BACKUP_AND_RECOVERY.md, BACKUP_LOG.md, RISK_REGISTER.md, TEST_PLAN.md, TEST_RESULTS.md, RELEASE_CHECKLIST.md, CHANGELOG.md, LEGAL_REVIEW_REQUIRED.md, COMPLIANCE_SOURCE_REGISTER.md, OPEN_SOURCE_GOVERNANCE.md, and CURRENT_HANDOFF.md.

CURRENT_STATUS.md is updated after meaningful work and uses feature counts instead of guessed percentages.

## Source control

Required branches are main, develop, milestone branches, and short-lived feature branches. main is not a direct work branch. Force-push and history rewrite are prohibited. Stable checkpoints are committed when a migration, workflow, API contract, integration, security control, milestone, or release changes.

## Backups

Git is not the only backup. Backup archives are not committed. The backup root for this computer is C:\Users\17402\ClearHaul-Backups, with worktree, database, documents, configuration, release, and manifests collections.

During active development the order requires a worktree snapshot every 30 minutes, a checkpoint commit, database and document backups every four hours when data changed, configuration backups after approved configuration changes, release backups for tagged builds, a daily encrypted project backup, and a weekly off-computer encrypted backup.

Retention keeps the last 48 half-hour worktree snapshots, 14 four-hour data backups, 30 daily backups, and 12 weekly backups, plus every milestone and release backup. Milestone and release backups are not deleted automatically.

Every backup has a manifest with identifier, time, commit, branch, included and excluded components, migration version, file hashes, encryption status, tool version, restore instructions, and verification result. A backup is trusted only after a restore test. The first foundation session must complete one restore test.

Plaintext secrets do not go in Git, archives, logs, documents, screenshots, demo data, agent prompts, or test fixtures.

## Destructive actions

A destructive action requires a named target, a reason, a verified backup, a record, and Michael's approval when the action could remove meaningful work or data. Force-push, hard reset, dropping a database, deleting production data, rewriting audit history, removing backups, rotating production keys, disabling security controls, removing encryption, releasing funds, publishing private information, and deploying unreviewed hazardous-material rules are prohibited without explicit authorization.

## Security baseline

Secure defaults start with the first commit: no secrets in source, pinned dependencies, vulnerability and secret scanning, static analysis where the toolchain supports it, validation, parameterized data access, least privilege, encryption plans, safe errors, file limits, audit logging, and redaction. Controls that are not actually present are documented as gaps.

## XRP Ledger

The ledger connection, when it is built, is for audit anchoring only. Cryptocurrency payments, a ClearHaul token, stablecoin payments, and user wallets are out of scope. Testnet is required until production activation is separately approved. Nobody is authorized to approve that activation until question CH-0006 is answered. Signing seeds are never stored in source, ordinary configuration, logs, or database rows. Memos may carry only a format identifier, format version, batch identifier, Merkle root, event count, and batch time range.

## Open source

The core is intended to be open source, but it is not published. Public material must not contain secrets, personal information, real shipments, banking data, private investigations, active fraud thresholds, unpatched vulnerability details, or incompatible third-party material. The license is not chosen until CH-0001 is answered. AGPL version 3 is the recommendation recorded for Michael.

## Database, documents, and tests

Schema changes need a forward migration, a tested upgrade path, a rollback or forward-repair note, a backup, and reviews for constraints, indexes, retention, and audit impact. The audit ledger is append-only once it exists. Corrections are new events.

Uploaded documents, once that feature exists, need an identifier, owner, shipment relationship, access policy, content hash, media type, size, upload time, uploader, malware-scan status, version, retention category, and audit events. Storage paths are not shown to users. Document contents are not placed on the XRP Ledger.

A feature is complete only when the implementation exists, visible controls work, data persists, authorization and validation hold, failures are handled, audit events exist for auditable actions, tests pass, documents are updated, a checkpoint and a backup exist, and limitations are stated. The foundation checkpoint has no auditable business action yet, and the status file must say that.

## First assignment

The first assignment is the foundation, not product screens: read the available order, inspect the machine, create the mandatory documents and question file, open Notepad, create the repository, configure Git safely, build the backup system, test one restoration, prepare Docker Compose, create the server, the Windows client shell, and the test projects, then stop and report verified status.

Initial questions required by the order are CH-0001 through CH-0007. CH-0008 records the missing blueprint.
