# Release checklist

No release has been built. Every item below was not run unless a later note in TEST_RESULTS.md says otherwise.

## 2026-09-30

Milestone 3 is not a release. The entry gate failed. Development-directory permission checks exist in the server tests. The PostgreSQL audit triggers are not applied. No installer exists. The tag `clearhaul-m3-equipment-approval` does not exist.

- Dependency scan: not run
- Secret scan: see TEST_RESULTS.md after the integrator run
- Static analysis: not run as a separate analyzer
- Permission tests: not run, because no users or roles exist
- Database migration test: not run, because no database exists
- Backup and restoration test: see BACKUP_LOG.md after the integrator run
- Installer test: not run, because no installer exists
- Audit-chain verification: not run, because no audit ledger exists
- Known-risk review: see RISK_REGISTER.md
- Final legal and open-source review: not done, decision CH-D-0014
