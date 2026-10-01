# Permission matrix

No users exist. No roles exist.

No accounts, passwords, or sessions are specified. Decision CH-D-0004 names shipper, carrier, driver testing, and administrator as workspace labels that are not built. Those labels are not roles and grant nothing.

Once the server exists, the only specified routes are unauthenticated. No credential is required, and no credential grants authority.

| Route | Method | Once the server exists |
| --- | --- | --- |
| `/health/live` | GET | Unauthenticated |
| `/health` | GET | Unauthenticated |

No other route is authorized.

A path outside those two routes is not an authorized route. The health contract requires `404` for any other path. That response does not grant access.

Nothing in this matrix authorizes shipment, payment, document, audit, database, or XRP Ledger actions. Those capabilities are not started. See `ARCHITECTURE.md`.
