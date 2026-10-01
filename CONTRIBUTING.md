# Contributing

Date: 2026-09-30

The working license for the server and core is AGPL version 3. See LICENSE, OPEN_SOURCE_GOVERNANCE.md, and decision CH-D-0014. A final legal and open-source review is still open.

## Branches

Engineers work on milestone or feature branches. Do not commit directly on main. Main is not a direct work branch.

## Do not add these

Contributions must not include:

- Secrets, including passwords, API keys, seeds, certificates, or connection strings that carry real credentials.
- Personal data.
- Real shipment files.
- A change that disables tests.

Do not weaken or skip a test to make a change pass. Fix the code or correct the test without turning the check off.

Plaintext secrets do not belong in Git, archives, logs, documents, screenshots, demo data, or test fixtures.
