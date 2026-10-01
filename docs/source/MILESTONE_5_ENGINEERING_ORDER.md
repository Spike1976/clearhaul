# ClearHaul Milestone 5 Engineering Order

## Trust, Disputes, Compliance, and Public Verification

Product owner: Michael Stokes  
Working product: ClearHaul  
Milestone: M5  
Status: Ordered; implementation begins only after its entry gates pass  
Primary surfaces: ClearHaul server, shipper workspace, carrier workspace, driver client, administrator workspace, public audit verifier

## 1. Mission

Milestone 5 builds the trust and accountability layer that allows ClearHaul to grow without giving any shipper, carrier, driver, administrator, algorithm, or outside service unchecked power.

ClearHaul must:

1. receive and investigate reports against shippers, carriers, drivers, facilities, equipment, and transactions;
2. separate reports, ratings, payment disputes, cargo claims, safety events, compliance holds, and marketplace enforcement;
3. require evidence and reason codes for consequential actions;
4. notify affected parties and provide a fair response and appeal path;
5. detect repeated and coordinated bad behavior without treating an allegation as proof;
6. preserve immutable decision history;
7. provide independent audit-record verification without exposing private freight data;
8. build disabled hazmat and specialized-freight foundations from authoritative sources;
9. enforce clear pilot boundaries; and
10. prove that the system can suspend marketplace privileges without confiscating earned payment or rewriting history.

## 2. Entry gates

Milestone 5 implementation begins only after:

- Milestones 0 through 4 meet their recorded definitions of done.
- PostgreSQL, Redis, and object storage are running and tested.
- The blueprint and master build prompt are present and read completely.
- Payment, tracking, delivery verification, ratings, audit events, backups, and restore tests pass.
- Open questions affecting trailer history, cleaning, evidence, ratings, payment, tracking, and delivery are answered or formally deferred.
- The live pilot remains nonhazardous domestic dry-van freight.
- Qualified legal, privacy, transportation-compliance, payment, and hazmat reviewers are identified for the rules that require them.
- Production XRP Ledger anchoring remains disabled unless Michael Stokes separately authorizes it in writing.

Provider-neutral schemas, interfaces, simulators, test fixtures, and disabled rule packages may be built before a live-service approval. No disabled capability may appear enabled to a user.

## 3. Trust-system principles

### 3.1 Allegation is not proof

A report creates a case. It does not automatically create guilt, a public warning, a rating penalty, payment loss, account suspension, or regulatory referral.

Automated signals may prioritize review. They do not issue final findings by themselves.

### 3.2 Enforcement and payment are separate

ClearHaul may restrict future marketplace activity when authorized by an approved enforcement rule. It may not use that restriction to seize, delay, or reverse earned undisputed payment from a completed load.

Payment exceptions follow the Milestone 4 payment and provider rules. Marketplace enforcement follows this milestone.

### 3.3 Evidence has provenance

Every evidence item records its source, submitter, relationship, observed time, received time, content hash, version, access policy, retention category, malware-scan state, confidence, and dispute status.

Carrier statements, shipper statements, driver statements, photographs, GPS, signatures, uploaded documents, third-party records, and administrator conclusions remain distinguishable.

### 3.4 Decisions are explainable and appealable

Every consequential decision identifies:

- the rule version;
- the facts relied upon;
- the evidence considered;
- conflicting evidence;
- the deciding authority;
- the resulting action;
- the effective period;
- the affected capabilities;
- the notice provided;
- the appeal deadline; and
- the method for review.

Private fraud thresholds, reporter identities when legally protected, security-sensitive details, and unrelated personal information are not exposed in notices.

### 3.5 History is append-only

Reports, responses, findings, sanctions, reversals, and appeals are never silently overwritten. A correction creates a new linked event.

## 4. Case types

The system must distinguish at least:

- service complaint;
- rating challenge;
- payment exception;
- cargo shortage claim;
- cargo damage claim;
- detention dispute;
- identity or impersonation report;
- authority or insurance discrepancy;
- equipment-history dispute;
- cleaning or washout dispute;
- seal exception;
- tracking or route-integrity event;
- pickup or delivery fraud report;
- harassment, coercion, or retaliation report;
- document falsification report;
- platform manipulation or collusion report;
- safety event;
- suspected hazmat misdeclaration;
- privacy complaint;
- security incident; and
- legal or regulatory request.

Each case type has separate permissions, required fields, evidence rules, service levels, retention, notice rules, and available outcomes.

## 5. Reporting workflow

Minimum report states:

1. `DRAFT`
2. `SUBMITTED`
3. `TRIAGE_PENDING`
4. `AWAITING_INFORMATION`
5. `UNDER_REVIEW`
6. `RESPONSE_REQUESTED`
7. `DECISION_PENDING`
8. `DECIDED`
9. `APPEAL_AVAILABLE`
10. `APPEAL_PENDING`
11. `FINAL`
12. `CLOSED_NO_ACTION`
13. `REFERRED`

Emergency containment is a separately recorded action. It does not skip later notice and review unless law or immediate safety requires restricted notice.

Reports support:

- structured reason codes;
- plain-language narrative;
- related shipment, organization, user, vehicle, trailer, facility, document, rating, payment, or event;
- evidence uploads;
- witnesses when appropriate;
- requested resolution;
- reporter confidentiality setting where permitted;
- conflict-of-interest declaration; and
- acknowledgement against knowingly false reports.

## 6. Triage and priority

Triage assigns:

- case type;
- severity;
- urgency;
- immediate-safety concern;
- continuing-harm risk;
- evidence-preservation need;
- required specialist;
- notice restrictions;
- conflict check;
- response deadline; and
- review owner.

Priority guidance:

- **Emergency:** credible immediate danger, active cargo theft, account takeover, active fraud, or serious safety threat.
- **Urgent:** delivery identity mismatch, forged documents, material seal issue, payment-provider anomaly, or repeated active misconduct.
- **Standard:** service, rating, detention, equipment, cleaning, or ordinary performance dispute.
- **Advisory:** incomplete information, policy question, or non-consequential concern.

The classification and every change are audited.

## 7. Evidence integrity

Evidence storage must:

- preserve the original file;
- calculate a content hash;
- create separate derivatives rather than replacing the original;
- restrict access by case role;
- scan uploads;
- enforce file type and size limits;
- preserve uploader and timestamps;
- record downloads and administrative access;
- support legal hold;
- prevent public URLs;
- redact copies without modifying originals; and
- include export manifests.

ClearHaul must display the difference between:

- claimed;
- system-observed;
- third-party reported;
- independently confirmed;
- disputed;
- superseded;
- rejected; and
- administratively determined.

## 8. Response and due process

Except where legally prohibited or immediate safety requires temporary containment, an affected party receives:

- the allegation category;
- the material transaction or event;
- a useful summary of the supporting basis;
- the possible consequences;
- the response deadline;
- a method to submit evidence;
- the identity of the reviewing ClearHaul function;
- the appeal rules; and
- accessibility and support options.

The reviewer must consider the response before a final adverse marketplace decision.

Silence may allow the review to proceed, but it is not automatically an admission.

## 9. Enforcement ladder

Available marketplace actions are intentionally separate:

1. no action;
2. education or policy reminder;
3. information correction request;
4. warning;
5. enhanced verification;
6. limited feature restriction;
7. load-value or activity limit;
8. manual review requirement;
9. temporary suspension;
10. relationship-specific block;
11. permanent marketplace removal;
12. preservation and referral when legally appropriate.

Actions must be proportional, time-bounded when appropriate, and scoped to the relevant risk.

An action against an organization does not silently punish every employee or driver without an approved relationship rule. An action against a driver does not automatically declare the carrier guilty.

## 10. Appeals

Appeals must:

- have a clear filing window;
- accept newly available or previously misunderstood evidence;
- identify alleged factual, procedural, or policy error;
- be reviewed by someone who did not make the original decision when staffing permits;
- record conflicts and recusals;
- preserve the original decision;
- result in affirmance, modification, reversal, remand, or dismissal;
- notify affected parties; and
- update marketplace restrictions without rewriting prior events.

Repeated frivolous appeals may be managed through an approved rule, but access to one meaningful appeal cannot be conditioned on payment.

## 11. Ratings and reputation

Reputation must not collapse distinct conduct into a mysterious single number.

ClearHaul will maintain explainable dimensions, including:

- identity and verification status;
- acceptance reliability;
- cancellation behavior;
- pickup timeliness;
- delivery timeliness;
- tracking reliability;
- document completeness;
- equipment compliance;
- detention conduct;
- communication;
- claim frequency and disposition;
- report history and disposition;
- payment conduct;
- response to problems; and
- recent verified improvement.

The interface must show:

- what period is measured;
- sample size;
- whether data is verified, reported, disputed, or under review;
- material exclusions;
- the rule version; and
- a path to challenge incorrect information.

Unproven reports are not published as confirmed misconduct. Closed-no-action reports do not count as violations.

## 12. Bad-actor detection

Risk signals may identify:

- duplicate identities;
- authority or insurance mismatch;
- coordinated rating manipulation;
- repeated last-minute cancellations;
- repeated delivery-location mismatch;
- document reuse;
- impossible location sequences;
- repeated seal discrepancies;
- chronic detention abuse;
- repeated false cargo descriptions;
- suspicious device or account sharing;
- chargeback or payment anomaly patterns;
- report retaliation;
- collusive bidding;
- washout or cleaning-document reuse; and
- rapid organization re-entry after removal.

Models and rules provide leads. Human review controls consequential enforcement unless a narrowly approved emergency security control applies.

Protected traits and unrelated criminal, medical, family, political, or personal information must not become reputation inputs.

## 13. Cargo claims and payment disputes

The claim system records:

- claimant and responding party;
- shipment and contract version;
- claim type;
- amount claimed;
- undisputed amount;
- notice date;
- evidence;
- inspection or third-party references;
- insurer or provider references;
- response;
- status;
- settlement or external disposition; and
- relationship to payment-provider action.

ClearHaul does not invent cargo-liability law, insurance coverage, chargeback rights, setoff rights, or claim deadlines. Those rules require written legal and insurance review.

The platform must never label itself the court, insurer, surety, broker, or final legal adjudicator.

## 14. Compliance source registry

Every encoded compliance rule must identify:

- jurisdiction;
- issuing authority;
- official source;
- citation;
- effective date;
- retrieval date;
- applicable operation;
- rule owner;
- interpretation status;
- professional reviewer;
- test cases;
- replacement or expiration status; and
- software rule versions using it.

Official primary sources control over blogs, summaries, AI output, vendor marketing, or memory.

Rules that cannot be mapped to an authoritative source stay advisory or disabled.

## 15. Hazmat foundation

Milestone 5 may build the disabled hazmat knowledge and validation foundation. It does not authorize live hazardous-material transportation.

The foundation must support versioned reference data for:

- proper shipping names;
- hazard classes and divisions;
- identification numbers;
- packing groups;
- subsidiary hazards;
- special provisions;
- quantity and unit rules;
- packaging references;
- placarding references;
- segregation references;
- shipping-paper data;
- emergency-response references;
- mode and jurisdiction;
- effective dates; and
- source citations.

The system must distinguish:

- regulatory source text;
- structured source data;
- ClearHaul interpretation;
- shipper declaration;
- calculated result;
- professional review;
- driver acknowledgement; and
- live-operation approval.

No AI-generated answer becomes a hazmat rule.

## 16. Hazmat declaration workflow

The disabled workflow must be designed so the shipper supplies and certifies the shipment facts. ClearHaul assists with completeness and consistency; it does not become the offeror or certify facts it does not know.

Potential inputs include:

- material identity;
- proper shipping name;
- identification number;
- hazard class;
- packing group;
- quantity;
- unit;
- package type and count;
- reportable quantity status;
- marine pollutant status;
- limited or excepted quantity claim;
- emergency contact;
- shipper certification;
- origin, destination, and route jurisdiction;
- equipment type; and
- temperature or handling requirements where applicable.

The engine may return:

- missing data;
- inconsistent data;
- possible classification candidates;
- placarding information;
- segregation information;
- shipping-paper checklist;
- emergency-reference link;
- driver packet requirements;
- confidence and source;
- review-required state; and
- operation-disabled state.

It must not say a load is legally compliant merely because required fields are filled.

## 17. Driver hazmat information package

The disabled driver package design must support:

- exact shipper-entered description;
- quantity and package information;
- placard information and source;
- segregation and placement information;
- shipping-paper location reminder;
- emergency-response information;
- required permits or routing flags when professionally approved;
- PPE or handling information supplied by authoritative sources;
- securement instructions from approved rules or shipper requirements;
- acknowledgements;
- corrections and version history; and
- offline availability.

The system must never improvise emergency instructions.

## 18. Public audit verifier

Build a public, read-only verification tool that can validate a proof package without revealing the private shipment.

The verifier may accept:

- audit format identifier;
- format version;
- batch identifier;
- event or record hash;
- Merkle proof;
- Merkle root;
- event count;
- batch time range;
- optional testnet transaction reference; and
- signed ClearHaul proof manifest when approved.

The verifier reports:

- proof structure valid or invalid;
- hash and Merkle calculation result;
- anchor found, missing, disabled, or unavailable;
- network used;
- confirmation information;
- format support;
- whether the proof matches the supplied record; and
- an explicit limitation that integrity proof does not establish truth, legality, identity, safety, or payment.

No private names, addresses, rates, cargo details, documents, credentials, or location history are published to the ledger or verifier.

## 19. XRP Ledger testnet anchoring

Milestone 5 may implement testnet-only audit anchoring after its tests and threat model are approved.

Permitted testnet memo content is limited to:

- format identifier;
- format version;
- batch identifier;
- Merkle root;
- event count; and
- batch time range.

Signing material:

- never appears in source;
- never appears in ordinary configuration;
- never appears in logs, screenshots, backups, database rows, or agent prompts;
- uses an approved secret boundary;
- supports rotation and revocation; and
- remains separate from user accounts and payments.

Production Mainnet anchoring remains off. Only Michael Stokes may approve it, in writing, under decision CH-D-0019.

## 20. Privacy and controlled disclosure

Case access is relationship-based, role-based, purpose-limited, and audited.

The system must support:

- reporter privacy where permitted;
- respondent access to a useful allegation summary;
- redacted evidence copies;
- legal holds;
- retention and deletion schedules;
- access review;
- administrator reason codes;
- export records;
- post-case access termination; and
- separation between public reputation facts and private case files.

The platform must not create a public accusation wall.

## 21. Required user experiences

### Reporter

- Select the correct report type.
- Understand what evidence is useful.
- Save a draft.
- Submit safely.
- Receive a case identifier.
- Track status and requests.
- Add evidence without altering previous submissions.

### Responding party

- Receive clear notice when permitted.
- Understand the allegation category and possible outcome.
- Submit a response and evidence.
- Identify errors.
- Track decision and appeal status.

### Administrator and reviewer

- Work from a conflict-checked queue.
- See evidence provenance and access restrictions.
- Compare statements, system events, and third-party records.
- Apply only authorized outcomes.
- Record reasons.
- Create notice.
- Preserve history.

### Public verifier user

- Paste or upload a proof package.
- Receive a plain-language valid, invalid, incomplete, unsupported, or unavailable result.
- See exactly what the result proves and does not prove.

## 22. Notifications

Notifications include:

- report submitted;
- evidence requested;
- response requested;
- temporary containment imposed, changed, or removed;
- decision issued;
- appeal opened, decided, or closed;
- marketplace restriction beginning or ending;
- legal hold applied or released;
- compliance source changed;
- hazmat reference update requiring review;
- audit batch created;
- testnet anchor confirmed or failed; and
- verifier format deprecated.

Notifications disclose only the minimum information necessary.

## 23. Required architecture

Create interfaces for:

- `ICaseService`
- `ITriageService`
- `IEvidenceService`
- `IEnforcementService`
- `IAppealService`
- `IReputationService`
- `IRiskSignalService`
- `IClaimService`
- `IComplianceSourceService`
- `IHazmatReferenceService`
- `IHazmatValidationService`
- `IAuditBatchService`
- `IAuditProofService`
- `IAnchorProvider`
- `IPublicVerificationService`

Provider, regulator, and network-specific objects do not leak into the domain model.

## 24. Required persistence

At minimum, design and migrate records for:

- cases;
- case parties and roles;
- allegations and reason codes;
- evidence and redacted derivatives;
- access grants;
- triage decisions;
- responses;
- findings;
- enforcement actions;
- notices;
- appeals;
- conflicts and recusals;
- claims and claimed amounts;
- reputation dimensions and snapshots;
- risk signals and review disposition;
- compliance sources;
- compliance rule packages and versions;
- hazmat reference versions;
- shipper hazmat declarations;
- validation results;
- professional approvals;
- audit batches;
- Merkle proof packages;
- anchor attempts;
- public-verifier requests with privacy-safe telemetry; and
- legal holds and retention actions.

Migrations require backup, upgrade tests, rollback or forward-repair notes, indexes, constraints, tenant-isolation review, retention review, and audit-impact review.

## 25. Required tests

Automated tests include:

- submitting a report does not create an automatic finding;
- unauthorized parties cannot access a case;
- administrator access requires a recorded purpose;
- original evidence cannot be overwritten;
- redaction does not modify the original;
- negative ratings and reports do not freeze earned payment;
- closed-no-action reports do not count as confirmed violations;
- enforcement uses the authorized rule version;
- temporary actions expire or require documented extension;
- a reviewer conflict triggers recusal controls;
- appeal preserves the original decision;
- reversal restores the correct marketplace privileges;
- suspension prevents new marketplace activity without blocking authorized evidence access or earned payment;
- reputation snapshots show sample size, period, and status;
- coordinated manipulation signals require review before enforcement;
- hazmat operation remains disabled;
- unsourced hazmat rules cannot be activated;
- outdated regulatory versions are not treated as current;
- a shipper declaration remains distinguishable from a calculated result;
- public verification works without private freight data;
- altered records fail hash or Merkle verification;
- malformed proofs fail safely;
- testnet failure does not change private audit history;
- production anchoring cannot be activated through ordinary configuration;
- signing material is absent from source, logs, database, backups, and fixtures;
- case and compliance exports include manifests and hashes; and
- backup restoration preserves cases, evidence metadata, rules, and proofs.

## 26. Demonstration scenario

Use synthetic case and shipment records only:

1. A completed demo load has released payment.
2. The shipper submits a documented equipment complaint.
3. The carrier receives notice and provides a response.
4. A reviewer sees a conflicting photograph, timestamp, and system event.
5. The case is decided without changing the completed payment.
6. The carrier appeals using new evidence.
7. A second reviewer reverses part of the decision.
8. Reputation snapshots update transparently.
9. A repeated synthetic document-reuse pattern raises a risk signal but not an automatic sanction.
10. A disabled hazmat declaration demonstrates sourced validation and a live-operation lock.
11. Private audit events are batched into a Merkle tree.
12. A testnet or simulated anchor is created.
13. The public verifier validates the proof package without receiving private shipment information.
14. An encrypted milestone backup is restored and the proof remains verifiable.

## 27. Definition of done

Milestone 5 is complete only when:

- all entry gates are recorded;
- reports, cases, evidence, response, enforcement, and appeal flows work;
- ratings, reports, claims, and payment states remain separate;
- consequential actions require authorized rules and evidence;
- public reputation information is explainable;
- privacy and access boundaries pass;
- disabled hazmat foundations are versioned, sourced, and visibly disabled;
- public audit verification works without private data;
- testnet or simulated anchoring works while production remains technically locked;
- all migrations and integrated tests pass;
- failure and recovery paths are demonstrated;
- mandatory project documents are updated;
- the Release suite passes;
- security, secret, dependency, and privacy scans pass or findings are recorded;
- a checkpoint commit exists;
- an encrypted milestone backup is created and restored; and
- limitations and professional-review requirements are stated plainly.

## 28. Explicit exclusions

Milestone 5 does not authorize:

- public unproven accusations;
- automatic guilt based on an algorithm;
- secret blacklists;
- punishment based on protected or unrelated personal information;
- confiscation or delay of earned payment as marketplace discipline;
- ClearHaul acting as a court, insurer, surety, broker, or regulator;
- unsourced legal or compliance rules;
- live hazardous-material transportation;
- AI-generated hazmat rules;
- driver surveillance outside an authorized load window;
- public shipment data on any blockchain;
- cryptocurrency payment;
- production XRP Ledger anchoring;
- production signing credentials;
- deletion or rewriting of audit events;
- real-world regulatory referral without authorized human review; or
- claims that ClearHaul guarantees compliance, safety, payment, cargo condition, or fraud prevention.

## 29. Stop conditions

Engineering stops and writes a plain-language question when:

- a rule would create a financial penalty or payment hold;
- a report category lacks evidence and notice standards;
- emergency containment authority is unclear;
- an appeal deadline or decision authority would be guessed;
- a public reputation field could expose unproven or private information;
- a legal, insurance, regulatory, privacy, employment, payment, or hazmat conclusion is unresolved;
- an authoritative compliance source cannot be confirmed;
- a hazmat calculation would be activated without qualified review;
- a production credential, Mainnet action, or real regulatory submission would be required;
- a case cannot be handled without exposing unrelated personal data; or
- the prior milestone evidence and audit contracts are unstable.

## 30. Handoff to Milestone 6

Milestone 6 may prepare a controlled nonhazardous pilot, partner onboarding, operational support, service monitoring, incident response, disaster recovery, usability validation, and limited production readiness only after Milestone 5 proves fair enforcement, appealability, privacy, disabled compliance expansion, public integrity verification, and backup restoration.
