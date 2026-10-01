# Backup log

## m0-foundation-20260930

Creation time: 2026-09-30
Project commit: 0bcd526ae17181516be8199f36327b21d508ed4e
Branch: milestone/m0-foundation
Backup identifier: m0-foundation-20260930
Location: C:\Users\17402\ClearHaul-Backups\worktree\m0-foundation-20260930
Key file: C:\Users\17402\ClearHaul-Backups\configuration\m0-foundation-20260930-key.json
Included components: staged worktree and a Git bundle at recovery/repository.bundle
Excluded components: bin, obj, .git, node_modules, TestResults, .vs
Database migration version: 0001_foundation.sql named in the manifest. The migration was not applied to a database.
Encryption: AES-256-GCM. The content key is wrapped with Windows DPAPI for the current user. The key file is outside the archive.
Tool version: 0.1.0-foundation
Create command exit code: 0
Restore command exit code: 0
Restore location: C:\Users\17402\ClearHaul-Backups\restore-test\m0-foundation-20260930
Verification result: verified
README.md SHA-256 from the repository matched the restored README.md.
The restored tree contains recovery/repository.bundle.

This restore did not start Docker, did not create a database, and did not run the application from the restored tree.

No off-computer copy was made. The external-drive path has not been provided.

## m1-entry-20260930

Creation time: 2026-09-30
Project commit: af39543
Branch recorded on the backup: milestone/m1-entry-repair
Backup identifier: m1-entry-20260930
Location: C:\Users\17402\ClearHaul-Backups\worktree\m1-entry-20260930\m1-entry-20260930
Key file: C:\Users\17402\ClearHaul-Backups\configuration\m1-entry-20260930-key.json
Encryption: AES-256-GCM
Create command exit code: 0
Restore command exit code: 0
Restore location: C:\Users\17402\ClearHaul-Backups\restore-test\m1-entry-20260930
Verification result: verified
README.md hashes matched because that file was unchanged. The restored Program.cs hash differs from the later working tree, which is the pre-repair snapshot.
This backup was taken before the identity-foundation edits. It does not contain a database. Docker was not started.

## m3-entry-20260930

Creation time: 2026-09-30
Project commit: 305f7990cddd2ee2883b34b6669e8be84e385320
Branch: milestone/m3-entry-gate
Backup identifier: m3-entry-20260930
Location: C:\Users\17402\ClearHaul-Backups\worktree\m3-entry-20260930
Key file: C:\Users\17402\ClearHaul-Backups\configuration\m3-entry-20260930-key.json
Included components: staged worktree and a Git bundle at recovery/repository.bundle
Excluded components: bin, obj, .git, node_modules, TestResults, .vs
Database migration version: 0002_phase_zero.sql named in the manifest. The migration was not applied to a database.
Encryption: AES-256-GCM. The content key is wrapped with Windows DPAPI for the current user. The key file is outside the archive.
Create command exit code: 0
Restore command exit code: 0
Restore location: C:\Users\17402\ClearHaul-Backups\restore-test\m3-entry-20260930
Verification result: verified
At restore time, CURRENT_STATUS.md and QUESTIONS_FOR_MICHAEL.md from commit 305f799 matched the restored files. The CURRENT_STATUS.md SHA-256 was B6DCBB4E6F9FA04BDA687118FD973812721A21A857561B21EBC326A7751AAE2B. This backup does not include the later note that records the restore.
This backup records the Milestone 3 entry-gate failure. It is not a Milestone 2 marketplace backup and it does not contain a database. Docker was not started. No off-computer copy was made.
