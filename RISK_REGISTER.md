# Risk register

| ID | Risk | Current state | What would reduce it |
| --- | --- | --- | --- |
| R1 | The blueprint .docx binary is absent. Markdown copies are in docs/source. | Open. Milestone 2 and Milestone 3 were ordered and were not implemented. | Keep product work behind the entry gates. Do not invent privacy or cost rules. |
| R2 | Docker Desktop is not installed, so PostgreSQL, MinIO, and Redis are not running. | Open. Compose is written and was not started. CH-0009 is unanswered. | Install Docker Desktop only after CH-0009 is answered yes, then run compose and a migration test. |
| R3 | The DPAPI backup key is tied to one Windows user. | Open. | Approve an off-computer key procedure when the external drive path exists. |
| R4 | No off-computer backup destination has been named. | Open. Local backups can still be made. | Michael provides the external-drive path. |
| R5 | The server speaks loopback HTTP. Sign-in is a development directory and is off unless configured. | Open for any use beyond this machine. | Add TLS and a production identity provider before any shared deployment. |
| R6 | Final legal and open-source review has not been done. | Open. The working license is AGPL version 3 and the public repository was requested. | Complete the review named in CH-D-0014. |
| R7 | Hazmat behavior could be activated too early. | Controlled for now. Live evaluation returns UNABLE TO DETERMINE. Live dispatch stays disabled. | Keep live hazmat disabled until qualified review. |
| R8 | Production XRP Ledger anchoring could be enabled by mistake. | Controlled for now. No ledger client exists. | Keep Mainnet credentials out of the repository until a separate written approval. |
| R9 | Milestone 3 could be built before persisted Milestone 2 and before the privacy answers. | Open. Feature work is stopped. CH-0010 through CH-0018 are unanswered. | Pass the Milestone 2 gate and receive written answers before equipment-history disclosure. |
