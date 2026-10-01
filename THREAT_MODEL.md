# Threat model

Foundation threats and their current state:

- Secret leakage through Git or a public repository. A scan covers private-key headers and cloud access-key prefixes. It is not a complete secret scanner.
- A future route added without authentication. No product routes exist. The risk remains for the next milestone.
- Loss of the DPAPI backup key when the Windows user profile is lost. No second key custodian exists.
- Loopback HTTP without TLS. Acceptable only on this development machine. It is not a deployed service.
- No off-computer backup. A disk failure removes the local encrypted copies and the source together if the source disk is the only disk.
- Public GitHub publication before the final license review. Decision CH-D-0021 publishes the foundation. Decision CH-D-0014 still requires that review.
- Hazardous-material rules invented in code. No hazmat rules are encoded. Live hazmat operation is disabled by decision CH-D-0015.
- Production XRP Ledger anchoring without approval. No ledger client exists. Decision CH-D-0019 names Michael Stokes as the only approver.

No threat listed here is closed unless the security model marks the related control implemented.
