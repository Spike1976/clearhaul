# ClearHaul Milestone 4 Engineering Order

## Protected Payment, Live Tracking, and Verified Delivery

Product owner: Michael Stokes  
Working product: ClearHaul  
Milestone: M4  
Status: Ordered; implementation begins only after its entry gates pass  
Primary clients: ClearHaul server, Windows operations client, shipper workspace, carrier workspace, driver client, administrator workspace

## 1. Mission

Milestone 4 builds the transaction and movement engine that protects the shipper without allowing earned carrier payment to be held hostage.

ClearHaul must:

1. require the shipper to fund or irrevocably authorize the agreed amount through an approved, licensed marketplace-payment provider;
2. allow the shipper to choose an earlier payment-release milestone when desired;
3. release undisputed earned freight payment automatically when delivery is verified;
4. track the load as a chain of custody rather than as a single moving map dot;
5. detect missing, contradictory, stale, or suspicious movement evidence;
6. confirm that the assigned driver, tractor, trailer, seal, freight record, route, receiver, and destination belong to the same shipment;
7. create evidence-backed, two-way performance records for shippers and carriers; and
8. preserve an append-only audit history of every material instruction, observation, exception, decision, and payment event.

This milestone does not turn ClearHaul into a bank, money transmitter, insurer, broker, or final judge of cargo claims.

## 2. Entry gates

Implementation does not begin until all applicable gates are recorded as passed:

- Milestone 0 foundation tests pass.
- Approved database, object storage, and queue services are running and health-checked.
- The ClearHaul blueprint and master build prompt are present and read completely.
- Required Milestone 2 marketplace contracts are stable.
- Required Milestone 3 equipment-assignment, history, cleaning, and approval contracts are stable.
- Questions CH-0009 through CH-0018 are answered or formally deferred without silently inventing a legal, financial, privacy, or safety rule.
- The first live pilot remains nonhazardous domestic dry-van freight.
- No live money movement is enabled until an approved payment provider, sandbox, agreement, and legal review exist.

Work that does not cross these gates may be limited to interfaces, provider-neutral contracts, disabled adapters, schemas, threat models, test fixtures, and simulations.

## 3. Non-negotiable product rules

### 3.1 Licensed payment partner

ClearHaul does not directly hold customer funds.

The payment layer must use a licensed third-party provider capable of marketplace or platform payments. The integration must sit behind a provider-neutral adapter so ClearHaul can replace a provider without rewriting shipment and ledger rules.

No source code, database row, log, screenshot, test fixture, or backup may contain a live payment credential, bank-account number, card number, or provider secret.

### 3.2 Funding before final award

A shipper may draft and publish freight according to the marketplace rules, but the load cannot reach the final-award state until the required funding status is confirmed by the payment provider.

ClearHaul stores the provider's opaque reference, amount, currency, state, timestamps, and permitted metadata. It does not store payment-card data.

### 3.3 Release timing

The accepted load agreement records exactly one current release policy:

- after verified pickup;
- after an agreed verified in-transit milestone; or
- after verified delivery.

The shipper may voluntarily select an earlier release milestone before it occurs. An early release is explicit, authenticated, logged, and treated as irreversible inside ClearHaul after the provider confirms it.

The default policy is release after verified delivery unless Michael approves another default in writing.

### 3.4 Delivery cannot become a payment hostage

When the contract's verified-delivery conditions are satisfied, ClearHaul sends the release instruction for the undisputed earned amount without requiring another discretionary shipper click.

A shipper cannot use silence, a low rating, personal dissatisfaction, or an unrelated claim to delay the undisputed earned payment.

A legitimate shortage, damage, fraud, identity, or delivery-location exception opens a separately identified exception or dispute workflow. The disputed amount, undisputed amount, evidence, authority, and provider capabilities must be explicit. ClearHaul never fabricates a right to split, reverse, freeze, or release money that the provider agreement and applicable law do not permit.

### 3.5 Ratings do not own the money

Ratings affect marketplace trust, visibility, review frequency, load limits, eligibility, and possible removal. Ratings do not decide whether completed work is paid.

Ratings are two-way:

- the shipper may rate the carrier's communication, timeliness, equipment compliance, document handling, and delivery performance;
- the carrier may rate the shipper's accuracy, readiness, treatment, loading and unloading performance, detention conduct, communication, and payment-related conduct.

Every negative rating requires a reason code. Serious accusations require evidence and moderation. Retaliatory ratings, duplicates, rating extortion, and coordinated manipulation are reportable.

## 4. Shipment chain of custody

Tracking must join the following records under one shipment identifier:

- shipper and facility;
- accepted tender and version;
- carrier authority;
- assigned driver;
- tractor and trailer;
- seal or seal-not-required declaration;
- cargo description and quantity record;
- pickup appointment and geofence;
- route and permitted deviations;
- position observations and data source;
- stop, detention, and exception events;
- delivery appointment and geofence;
- authorized receiver or receiver method;
- proof-of-delivery documents;
- final delivery verification; and
- payment-release event.

No single signal proves successful delivery. GPS alone proves neither custody nor correct cargo. A photograph alone proves neither cleanliness nor delivery. A signature alone does not prove the signer was at the contracted destination.

## 5. Tracking sources and confidence

Each position observation stores:

- shipment identifier;
- source type;
- source account or device identifier;
- observed time;
- received time;
- latitude and longitude;
- reported accuracy;
- speed and heading when available;
- freshness;
- confidence;
- consent and authorization basis;
- integrity or provider reference when available; and
- whether the observation was used in a decision.

Supported source contracts may include:

- ClearHaul driver-client location;
- carrier telematics or ELD integration;
- tractor or trailer tracking provider;
- facility geofence events;
- manual driver check-in; and
- administrator-entered recovery evidence.

The interface must clearly distinguish live, delayed, estimated, manually entered, unavailable, and conflicting data.

Continuous surveillance is not the default. Collection begins no earlier than the authorized load-tracking window and ends after delivery, cancellation, or the approved post-delivery period. Raw high-frequency location retention must be minimized and documented.

## 6. Milestone state machine

The server owns the shipment state. Clients request transitions and display the authoritative result.

Minimum movement states:

1. `AWAITING_FUNDING`
2. `FUNDED`
3. `AWARDED`
4. `EQUIPMENT_PENDING`
5. `READY_FOR_PICKUP`
6. `AT_PICKUP`
7. `PICKED_UP`
8. `IN_TRANSIT`
9. `AT_DELIVERY`
10. `DELIVERY_EVIDENCE_PENDING`
11. `DELIVERED_VERIFIED`
12. `PAYMENT_RELEASE_REQUESTED`
13. `PAYMENT_RELEASED`
14. `COMPLETED`

Exceptional states include:

- `TRACKING_STALE`
- `ROUTE_EXCEPTION`
- `IDENTITY_MISMATCH`
- `EQUIPMENT_MISMATCH`
- `SEAL_EXCEPTION`
- `DELIVERY_EXCEPTION`
- `PAYMENT_EXCEPTION`
- `DISPUTE_OPEN`
- `CANCELLED`

Every transition specifies the actor, permission, prerequisites, evidence, idempotency key, resulting audit events, notification behavior, and recovery path.

## 7. Verified pickup

Pickup verification requires the configured evidence bundle. The provider-neutral foundation must support:

- arrival inside the pickup geofence or an approved location exception;
- assigned driver authentication;
- assigned tractor and trailer confirmation;
- shipper or facility handoff confirmation;
- bill-of-lading capture or approved equivalent;
- cargo quantity acknowledgement;
- seal capture or seal-not-required declaration;
- pickup time; and
- unresolved-exception check.

The precise mandatory bundle must be configurable by load type and professionally reviewed before live hazardous or specialized freight.

## 8. Verified delivery

Delivery verification requires an evidence bundle, not a single button.

The provider-neutral foundation must support:

- arrival inside the delivery geofence or an approved location exception;
- shipment, driver, tractor, trailer, and seal continuity check;
- authorized receiver confirmation using an approved method;
- delivery timestamp;
- proof-of-delivery document;
- quantity and visible-condition acknowledgement;
- exception declaration;
- final location confidence; and
- duplicate-delivery and replay protection.

The receiver method may later include a one-time code, authenticated shipper account, facility integration, QR handoff, or administrator-reviewed exception. The method must not expose private shipment data in a public QR code.

## 9. Route and exception engine

The tracking service must detect and explain:

- stale or missing tracking;
- movement before pickup verification;
- movement by unassigned equipment;
- unexpected trailer substitution;
- seal mismatch;
- material route deviation;
- unexpected prolonged stop;
- likely missed appointment;
- arrival at the wrong geofence;
- delivery evidence submitted away from the destination;
- duplicate or replayed evidence;
- contradictory timestamps;
- location impossible for elapsed time; and
- loss of the authorized tracking source.

An alert is not automatically guilt. Each alert records its source, threshold version, confidence, review state, resolution, and reviewer.

Active fraud thresholds and sensitive detection rules are not published in public documentation.

## 10. Payment ledger

ClearHaul maintains an internal accounting ledger that mirrors, but does not replace, the payment provider.

Minimum records:

- agreed line-haul amount;
- fuel surcharge when applicable;
- accessorials;
- detention;
- approved additions or deductions;
- funded amount;
- undisputed amount;
- disputed amount;
- release policy and version;
- provider transaction references;
- requested, pending, succeeded, failed, reversed, and reconciled events; and
- final carrier remittance statement.

Money uses fixed-precision decimal values with an explicit currency. Floating-point money is prohibited.

Provider webhooks are authenticated, idempotent, replay-resistant, stored safely, and reconciled against provider queries. A browser redirect is never treated as proof that money moved.

## 11. Required user experiences

### Shipper

- See funding status without seeing protected financial credentials.
- Select an allowed release milestone before that milestone occurs.
- See the evidence supporting pickup, transit, and delivery.
- Receive clear exception notices.
- Open a documented claim or report without silently stopping undisputed payment.
- Rate the carrier after the rating window opens.

### Carrier

- See whether the load is funded before final acceptance.
- See the exact release policy and all changes before accepting them.
- Follow funding, release, failure, and reconciliation states.
- Submit authorized evidence and respond to exceptions.
- Receive a clear remittance statement.
- Rate the shipper.

### Driver

- See the assigned shipment, stops, trailer, seal requirements, and tracking state.
- Understand when tracking starts and stops.
- Confirm pickup and delivery evidence offline when necessary.
- See when location or evidence is missing, stale, or rejected.
- Never see bank credentials or unrestricted shipper financial data.

### Administrator

- Review identity, tracking, delivery, rating, payment, and dispute evidence under least privilege.
- Never edit or delete prior audit events.
- Resolve exceptions by adding a new decision event.
- See provider reconciliation differences.
- Suspend marketplace privileges separately from payment rights.

## 12. Notifications

Notification events include:

- funding confirmed or failed;
- final award;
- equipment approved;
- tracking started, stale, restored, or ended;
- pickup arrival and pickup verification;
- route, seal, identity, equipment, or appointment exception;
- delivery arrival;
- delivery evidence requested, submitted, rejected, or verified;
- early release authorized;
- payment release requested, succeeded, or failed;
- rating window opened; and
- report or dispute status changed.

Notifications contain the minimum necessary information and never include payment credentials, private document URLs, or unnecessary location history.

## 13. Security and privacy requirements

- Role and organization authorization is enforced by the server.
- Shipment access is relationship-bound and time-bound.
- Provider secrets use an approved secret store.
- Webhook authenticity is verified before state changes.
- Every external request uses timeouts, bounded retries, and idempotency.
- Documents use malware scanning, hashes, versioning, and access-controlled object storage.
- Sensitive values are redacted from telemetry and logs.
- Location access is explicit, visible, revocable when no active load requires it, and limited to the authorized purpose.
- Support staff access requires a reason and creates an audit event.
- Test data is synthetic.
- Audit corrections append; they never overwrite.

## 14. Provider architecture

Create interfaces before a real provider adapter:

- `IPaymentMarketplaceProvider`
- `IFundingService`
- `IPayoutReleaseService`
- `IPaymentReconciliationService`
- `ITrackingProvider`
- `IGeofenceService`
- `IRouteExceptionService`
- `IDeliveryVerificationService`
- `IRatingService`

Provider-specific objects do not leak into the domain model or public API.

The first implementation uses simulators and official provider sandbox environments only. No live account is opened, no production credential is created, and no real money is moved under this order.

## 15. Required persistence

At minimum, design and migrate records for:

- payment accounts by opaque provider reference;
- funding intents;
- release policies and versions;
- ledger entries;
- provider events;
- reconciliation runs and differences;
- tracking sessions;
- position observations;
- geofences;
- custody events;
- route exceptions;
- pickup and delivery evidence;
- verification decisions;
- ratings, reason codes, evidence, responses, and moderation;
- reports and disputes; and
- notifications.

Retention, indexes, tenant isolation, foreign keys, uniqueness, concurrency, and audit impact require documented review.

## 16. Testing

Required automated tests include:

- unauthorized organization access is denied;
- a load cannot be finally awarded without confirmed funding;
- duplicate funding and release requests are idempotent;
- forged or replayed webhooks cannot change payment state;
- an early-release choice is authenticated and audited;
- verified delivery requests release exactly once;
- a rating cannot freeze earned payment;
- the undisputed and disputed amounts cannot exceed the funded amount;
- provider failure leaves a recoverable, visible state;
- reconciliation finds missing and contradictory events;
- stale GPS is not labeled live;
- wrong geofence cannot silently verify delivery;
- mismatched driver, trailer, tractor, or seal opens an exception;
- duplicate delivery evidence is detected;
- offline evidence sync preserves original observed time and records received time;
- location access ends when the authorized tracking period ends;
- two-way ratings require completed eligible loads;
- negative ratings require reason codes;
- moderation does not rewrite the original rating event; and
- audit-chain verification detects alteration.

Integration tests use PostgreSQL, Redis, MinIO, payment simulators, tracking simulators, and provider sandboxes when approved.

## 17. Demonstration scenario

The milestone demonstration uses synthetic shipment CH-DEMO-0004:

1. A verified shipper posts a nonhazardous dry-van load.
2. A simulated payment provider confirms funding.
3. A verified carrier accepts the load and sees the release policy.
4. The assigned driver, tractor, trailer, and seal are confirmed.
5. Pickup is verified using synthetic location and document evidence.
6. The shipper selects an early release, then the system demonstrates its irreversible audit event in one run.
7. A separate run keeps the default delivery release.
8. Tracking becomes stale, raises an exception, then recovers.
9. A route deviation is explained and reviewed.
10. Delivery at the correct geofence is confirmed by synthetic receiver evidence and proof of delivery.
11. The system requests release exactly once and reconciles the simulated provider result.
12. Both parties submit ratings.
13. An administrator reviews the complete custody, payment, and rating record.

No real person, shipment, location history, payment account, or money appears in the demonstration.

## 18. Definition of done

Milestone 4 is complete only when:

- all entry gates are recorded;
- schemas and forward migrations are applied and tested;
- payment and tracking provider interfaces exist;
- simulators and approved sandbox adapters pass;
- server authorization and validation pass;
- shipper, carrier, driver, and administrator controls work;
- data survives restart;
- pickup and delivery evidence work offline and synchronize safely;
- automatic verified-delivery release is idempotent;
- ratings cannot control earned payment;
- exception and recovery paths are demonstrated;
- audit events are append-only and verifiable;
- security, privacy, threat, API, domain, database, permission, risk, test, status, changelog, handoff, and legal-review documents are updated;
- the Release test suite passes;
- secret and vulnerability scans pass or approved findings are recorded;
- a checkpoint commit exists;
- an encrypted milestone backup is created and restored; and
- limitations and disabled production capabilities are stated plainly.

## 19. Explicit exclusions

Milestone 4 does not authorize:

- ClearHaul custody of customer funds;
- live card or bank processing without provider approval;
- production payment credentials;
- cryptocurrency or stablecoin payment;
- XRP Ledger payment;
- production XRP Ledger anchoring;
- automatic guilt findings from GPS or ratings;
- secret deductions from carrier payment;
- deletion or rewriting of audit history;
- continuous driver tracking outside an authorized shipment window;
- sale of driver location data;
- live hazardous-material transportation;
- autonomous cargo-claim adjudication; or
- claims that the system guarantees theft prevention, cargo condition, legal compliance, or successful delivery.

## 20. Stop conditions

Engineering stops and writes a question for Michael when:

- a payment-provider choice changes fees, liability, custody, onboarding, reserves, chargebacks, or legal status;
- a rule would permit holding earned payment after verified delivery;
- disputed and undisputed payment cannot be separated;
- receiver identity or delivery evidence requirements are unresolved;
- tracking would collect more location data than the authorized shipment purpose requires;
- a rating rule would impose an automatic financial penalty;
- an integration requires production credentials or a live account;
- a legal, insurance, hazmat, employment, privacy, or money-transmission conclusion would otherwise be guessed; or
- previous milestone contracts are not stable enough to support the work.

## 21. Handoff to Milestone 5

Milestone 5 may build advanced disputes, claims, compliance expansion, hazmat foundations, public audit verification, and controlled network scaling only after Milestone 4 has proven the complete synthetic path from funded award through verified delivery, released payment, reconciliation, ratings, backup, and restore.
