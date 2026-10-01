# Backup and recovery

Local encrypted backups are stored outside the repository at `C:\Users\17402\ClearHaul-Backups`.

The backup tool is `src/ClearHaul.Backup`. It writes a directory that contains `manifest.json` and `payload.bin`. The payload is a zip of the source files, encrypted with AES-256-GCM. The manifest holds the file hashes, the ciphertext hash, the commit, the branch, the migration name, and the retention class.

Directories named `bin`, `obj`, `.git`, `node_modules`, `TestResults`, and `.vs` are excluded.

The command-line tool wraps the content key with Windows DPAPI for the current user. The key file stays outside the archive. Unit tests call the library with an in-memory key and do not use DPAPI.

Restore checks the ciphertext hash, authenticates the payload, and checks every restored file hash. A mismatch throws and does not mark the manifest verified.

Retention keeps the newest 48 `worktree` backups. Backups marked `milestone` or `release` are not selected for deletion. The retention helper has a unit test. It has not deleted any backup on disk.

An external drive is the first off-computer destination under decision CH-D-0018. No drive path has been provided, so no off-computer copy exists. A second encrypted cloud destination is planned and not configured.

Backups must not contain plaintext passwords, signing keys, personal information, or production secrets. This foundation tree has no production secrets. The development placeholder `dev-only-not-a-secret` is only in `.env.example`.

A repository restore is recorded in `BACKUP_LOG.md` only after the integrator runs it. A file existing on disk is not itself evidence of a successful restore.

Create a local backup:

```powershell
.\scripts\recovery\New-ClearHaulBackup.ps1 -Source . -Destination C:\Users\17402\ClearHaul-Backups\worktree -BackupId example -KeyOut C:\Users\17402\ClearHaul-Backups\worktree\example-key.json -Branch develop -Commit abc -Migration 0001_foundation.sql
```

Restore it to an empty folder:

```powershell
.\scripts\recovery\Restore-ClearHaulBackup.ps1 -Manifest C:\Users\17402\ClearHaul-Backups\worktree\example\manifest.json -Destination C:\RestoreHere -Key C:\Users\17402\ClearHaul-Backups\worktree\example-key.json
```

The DPAPI key can be unwrapped only by the same Windows user on this computer. Losing that user profile loses the local backup key. That is a known limit until an off-computer key procedure is approved.
