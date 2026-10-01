# Project ClearHaul direct freight marketplace blueprint

Date recorded: 2026-09-30
Source: the product assignment read before Phase Zero design. A `.docx` binary was not attached. This markdown is the governing product text.

The platform is a direct shipper-to-carrier freight marketplace. It is not a generic load board, a dispatch dashboard, or a broker marketplace. The software must not describe itself as a broker, carrier, or escrow holder. That legal role is undecided and requires an attorney.

## Mission

Connect verified shippers with verified motor carriers. Make price, charges, deductions, and payment conditions visible to the parties. Protect carrier payment through shipper prefunding by a regulated third party, not by a homemade escrow account. Standardize shipper information. Deliver the load packet to the assigned driver. Limit equipment history to the selected shipment. Use evidence-based reports and a review process before removal. Leave legal duties with the party the law assigns. No statute is encoded in this repository.

## Pilot

The first live pilot is nonhazardous domestic dry-van freight. Hazmat structures may exist and must stay unable to dispatch a live load until qualified hazmat and legal review. Decision CH-D-0015.

## People and organizations

Roles: shipper organization administrator, shipper employee, shipping-facility personnel, carrier organization administrator, dispatcher, safety and compliance personnel, driver, washout or inspection facility, platform investigator, platform compliance administrator, payment and dispute administrator, read-only auditor, and system administrator. A person may hold more than one role. Every action is attributed to one user and one organization.

The Windows shell still shows four unfinished workspace labels. Those labels are not these roles and are not built.

## Shipment states

Draft, validation required, funding pending, funded, open, bidding, carrier conditionally selected, equipment assignment pending, equipment assigned, equipment review, cleaning requested, equipment substitution requested, equipment approved, driver acknowledgment pending, ready for pickup, at pickup, loaded, in transit, at delivery, delivered, document review, payment release pending, paid, disputed, cancelled, and closed.

The shipper selects the carrier before assigned-equipment history is visible. Fleet search, unassigned trailers, previous customers, previous rates, unrelated routes, and product identities are not part of that view. Access is shipment-scoped, logged, and expired when the shipment is closed. The relied-upon snapshot is kept.

Equipment history is append-only. A correction is a new record with a reason, evidence, user, organization, and time. Carrier-declared information is not labeled third-party verified.

## Money

Funding is requested through a sandbox adapter labeled `sandbox-not-a-bank`. Every ledger line in this foundation is simulated. Reserved shipment funds are not unilaterally withdrawable after award. Platform operating funds are a separate ledger account. No percentage fee is invented; the tender supplies the platform charge. No bank, card, or wallet is connected.

## Hazmat and rules

The shipper confirms any hazardous-material facts. The software may not invent a classification, placard, or compatibility result. Without an approved rule package, the live result is UNABLE TO DETERMINE and QUALIFIED HAZMAT REVIEW REQUIRED, and dispatch is blocked. Synthetic fixtures exist only for tests and cannot be marked approved for live use.

A declared permit or route restriction supplied on the tender can block departure. That is the shipper's declared fact, not a copied regulation.

## What this blueprint does not yet authorize

No production payment, no live hazmat activation, no government data pull, no Android application, no shipment HTTP API, and no claim that the marketplace is operational.
