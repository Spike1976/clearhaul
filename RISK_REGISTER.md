# Risk register

| ID | Risk | Current state | What would reduce it |
| --- | --- | --- | --- |
| R1 | The blueprint .docx binary is absent. Markdown copies are in docs/source. | Open. Milestone 2 and Milestone 3 were ordered and were not implemented. | Keep product work behind the entry gates. Do not invent privacy or cost rules. |
| R2 | The Docker engine can stop, and the foundation server is not connected to the local services. | Reduced on 2026-10-01. Desktop 4.93.0 is installed. Compose was healthy and migrations 0001 and 0002 are on the volume. Object storage is pinned to Silo. | Keep the volume. Connect the server only with the health contract. Do not start the docker-desktop WSL distro by hand. |
| R3 | The DPAPI backup key is tied to one Windows user. | Open. | Approve an off-computer key procedure when the external drive path exists. |
| R4 | No off-computer backup destination has been named. | Open. Local backups can still be made. | Michael provides the external-drive path. |
| R5 | The server speaks loopback HTTP. Sign-in is a development directory and is off unless configured. | Open for any use beyond this machine. | Add TLS and a production identity provider before any shared deployment. |
| R6 | Final legal and open-source review has not been done. | Open. The working license is AGPL version 3 and the public repository was requested. | Complete the review named in CH-D-0014. |
| R7 | Hazmat behavior could be activated too early. | Controlled for now. Live evaluation returns UNABLE TO DETERMINE. Live dispatch stays disabled. | Keep live hazmat disabled until qualified review. |
| R8 | Production XRP Ledger anchoring could be enabled by mistake. | Controlled for now. No ledger client exists. | Keep Mainnet credentials out of the repository until a separate written approval. |
| R9 | Milestone 3 could be built before persisted Milestone 2. | Open. Feature work is stopped. CH-0010 through CH-0018 are answered. The longer retention periods and the dispute procedure are unnamed. | Pass the Milestone 2 gate before equipment-history disclosure. |
