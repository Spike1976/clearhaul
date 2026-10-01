# Risk register

| ID | Risk | Current state | What would reduce it |
| --- | --- | --- | --- |
| R1 | The blueprint and master build prompt are not in docs/source. | Open. Product workflows are not designed. | Place both required files and read them before workflow design. |
| R2 | Docker Desktop is not installed, so PostgreSQL, MinIO, and Redis are not running. | Open. Compose is written and was not started. | Install Docker Desktop only with permission, then run compose and a migration test. |
| R3 | The DPAPI backup key is tied to one Windows user. | Open. | Approve an off-computer key procedure when the external drive path exists. |
| R4 | No off-computer backup destination has been named. | Open. Local backups can still be made. | Michael provides the external-drive path. |
| R5 | The server speaks loopback HTTP and has no accounts. | Open for any use beyond this machine. | Add TLS and authentication before any shared deployment. |
| R6 | Final legal and open-source review has not been done. | Open. The working license is AGPL version 3 and the public repository was requested. | Complete the review named in CH-D-0014. |
| R7 | Hazmat behavior could be activated too early. | Controlled for now. No hazmat code exists. | Keep live hazmat disabled until qualified review. |
| R8 | Production XRP Ledger anchoring could be enabled by mistake. | Controlled for now. No ledger client exists. | Keep Mainnet credentials out of the repository until a separate written approval. |
