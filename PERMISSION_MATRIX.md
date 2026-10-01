# Permission matrix

Domain permissions are enforced by `PermissionMatrix` in the in-process model. They are not HTTP routes. The health routes remain unauthenticated and return no freight data.

Sensitive domain actions require `MfaSatisfied` on the actor. The flag is an input. No authenticator is integrated. A false flag refuses the action.

| Role | Grants |
| --- | --- |
| Shipper organization administrator | Submit tender, publish, select carrier, review and approve assigned equipment, request payment release, dispute, read own organization |
| Shipper employee | Submit tender, publish, select carrier, review and approve assigned equipment, dispute |
| Shipping-facility personnel | Washout evidence, delivery evidence |
| Carrier organization administrator | Bid, assign equipment, washout evidence, dispute, read own organization |
| Dispatcher | Bid, assign equipment, washout evidence |
| Safety and compliance personnel | Washout evidence, read own organization |
| Driver | Acknowledge packet, delivery evidence, washout evidence, dispute |
| Washout or inspection facility | Washout evidence |
| Platform investigator | Investigate, read audit. MFA required |
| Platform compliance administrator | Draft a rule package, review identity, read audit. MFA required for draft and identity |
| Payment and dispute administrator | Record sandbox funding and release, dispute, read audit. MFA required for funding and release |
| Read-only auditor | Read audit, read organization record |
| System administrator | Review identity, read audit, record enforcement. MFA required. No payment release and no equipment-history view |

No role can delete an audit event, search a carrier fleet, approve a rule package for live hazmat, post a non-simulated ledger line, or withdraw reserved funds after award.

Equipment history also requires all of the following: the shipper role that may review equipment, the same organization as the shipment, the user id authorized on that shipment, a selected carrier, the assigned trailer id, and an unexpired access window. A view, export, or download is logged.

| Route | Method | Access |
| --- | --- | --- |
| `/health/live` | GET | Unauthenticated |
| `/health` | GET | Unauthenticated |

No shipment route is published.
