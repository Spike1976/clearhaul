# Domain model

Business aggregates are not specified.

The blueprint and the master build prompt were not supplied. `docs/source/README.md` records that fact. Decision CH-D-0001 says product workflows are not designed from guesswork. This file does not invent aggregates, fields, states, or tables.

No shipment aggregate is defined. No payment state is defined. No database table is defined here. No legal rule is defined here.

Decision CH-D-0003 is a reversible documentation assumption about a possible later pilot boundary. It does not define a freight aggregate, a field, or a rule. No freight workflow exists.

Decision CH-D-0004 names four workspace labels: shipper, carrier, driver testing, and administrator. Those labels are not domain aggregates. They are not built.

No user aggregate and no role aggregate exist. See `PERMISSION_MATRIX.md`.

The audit ledger, object storage, and XRP Ledger anchoring are not modeled here. They are not started. See `ARCHITECTURE.md`.
