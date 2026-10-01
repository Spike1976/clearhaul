# Incident response

This is a local foundation, not a hosted production system. There is no on-call rotation and no public incident mailbox.

If source, a backup, or a machine used for this project is exposed:

1. Stop publishing any new secret. Do not commit a key, token, or dump to repair it.
2. Tell Michael Stokes through the same private channel used for this project.
3. Revoke the exposed credential at its issuer.
4. Record the time, the affected path, and what was not exposed. Do not paste the secret into the record.
5. Keep the append-only audit and ledger files. Do not delete them to hide the event.

The health server has no accounts. The domain model has no real personal data. The synthetic seed uses obvious fake labels. The backup key under `C:\Users\17402\ClearHaul-Backups\configuration` stays outside the repository.
