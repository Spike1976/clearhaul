# ClearHaul Milestone 6 Engineering Order

## Controlled Pilot and Production Readiness

Product owner: Michael Stokes  
Working product: ClearHaul  
Milestone: M6  
Status: Ordered; no real shipment or real-money pilot begins until every applicable launch gate is approved  
Pilot boundary: United States domestic, nonhazardous, dry-van freight only

## 1. Mission

Milestone 6 prepares, operates, measures, and closes a deliberately small ClearHaul pilot.

The milestone must prove that verified shippers, carriers, dispatchers, drivers, administrators, payment partners, and support personnel can safely complete the full ClearHaul workflow under controlled real-world conditions.

The pilot must demonstrate:

1. verified participant onboarding;
2. standardized shipper tenders;
3. funded loads through an approved licensed payment provider;
4. carrier bidding and acceptance;
5. driver and equipment assignment;
6. controlled trailer-history review and cleaning decisions;
7. live shipment tracking and chain of custody;
8. pickup and delivery verification;
9. automatic release of undisputed earned payment;
10. ratings, reports, disputes, and appeals;
11. operational monitoring and support;
12. security, privacy, incident, backup, and disaster-recovery controls; and
13. an evidence-based go, hold, revise, or stop decision.

This milestone is not a public nationwide launch.

## 2. Entry gates

Pilot preparation may begin only after:

- Milestones 0 through 5 satisfy their recorded definitions of done.
- All required source documents have been read.
- All blocking product-owner decisions are recorded.
- PostgreSQL, Redis, object storage, queues, backups, monitoring, and restore procedures pass.
- Shipper, carrier, driver, administrator, payment, tracking, evidence, reporting, appeal, and audit workflows pass integrated tests.
- A licensed payment provider approves the intended marketplace flow.
- Payment-provider sandbox reconciliation passes.
- Legal review approves the pilot contracts, terms, privacy notice, consent language, payment flow, dispute boundaries, and participant roles.
- Transportation-compliance review approves the nonhazardous domestic dry-van pilot boundary.
- Insurance review confirms required participant coverage and ClearHaul's coverage plan.
- Security and privacy reviews approve the pilot.
- A production-support owner and emergency contact process are named.
- Production infrastructure and credentials are separated from development.
- A complete production-like backup is restored successfully.
- Michael Stokes gives a written pilot-launch approval naming the permitted dates, participants, load limits, geographic scope, and maximum financial exposure.

Failure of one gate keeps the real pilot off. Screens and documentation must not imply that a blocked capability is live.

## 3. Pilot boundaries

The first pilot is limited to:

- United States domestic freight;
- nonhazardous cargo;
- standard dry-van equipment;
- approved shipper organizations;
- approved motor carriers with verified authority and insurance;
- approved drivers assigned by those carriers;
- approved pickup and delivery facilities;
- approved lanes or geographic regions;
- approved payment methods and maximum load value;
- an approved number of simultaneous loads;
- staffed support hours;
- an approved pilot start and end date; and
- explicitly accepted pilot terms.

Excluded from the pilot:

- hazardous materials;
- international or cross-border freight;
- refrigerated, temperature-controlled, tanker, bulk, flatbed, oversize, household-goods, passenger, or specialized freight;
- cryptocurrency or stablecoin payments;
- cash handling;
- factoring unless separately designed and approved;
- production XRP Ledger anchoring without separate written approval;
- unverified participants;
- unsupported mobile or desktop environments;
- uncontrolled public enrollment; and
- loads above the approved financial or operational limit.

## 4. Pilot participant model

Create pilot cohorts for:

- shipper administrators;
- shipper operations users;
- carrier administrators;
- carrier dispatchers;
- drivers;
- ClearHaul administrators;
- support reviewers;
- payment operations;
- security and incident response;
- compliance observers; and
- pilot evaluators.

Every participant receives:

- identity verification;
- organization verification;
- role assignment;
- least-privilege access;
- training;
- written pilot limitations;
- privacy and tracking notice;
- support instructions;
- emergency instructions;
- test-device and software requirements;
- acknowledgment of responsibilities; and
- removal and offboarding procedure.

No participant shares an account. Temporary accounts expire.

## 5. Shipper onboarding

Shipper onboarding verifies and records:

- legal business identity;
- physical and mailing addresses;
- authorized representatives;
- tax and payment-provider onboarding status;
- approved facilities;
- contacts and escalation paths;
- cargo categories permitted in the pilot;
- tender authority;
- payment authority;
- expected volume;
- maximum load value;
- contract acceptance;
- privacy acknowledgement; and
- training completion.

The pilot must prevent a shipper from posting freight outside its approved scope.

## 6. Carrier onboarding

Carrier onboarding verifies and records:

- legal business identity;
- USDOT and MC authority where applicable;
- operating status;
- insurance information and verified coverage;
- authorized representatives;
- payment-provider onboarding;
- approved equipment types;
- approved drivers;
- safety and compliance review status;
- service area;
- load-value limit;
- contract acceptance;
- privacy and tracking acknowledgement; and
- training completion.

Verification is refreshed before expiration and when authoritative data changes.

ClearHaul does not describe a carrier as safe merely because authority and insurance checks pass.

## 7. Driver readiness

Before receiving a pilot load, a driver must have:

- a carrier-authorized assignment;
- verified account and device;
- current supported client version;
- required permissions;
- clear location-tracking disclosure;
- offline load-packet capability;
- pickup and delivery evidence capability;
- notification checks;
- emergency support information;
- training completion; and
- a successful practice run using synthetic data.

The driver client must clearly display:

- active load;
- assigned tractor and trailer;
- stops and appointment windows;
- contacts;
- tracking status and source;
- required evidence;
- detention controls;
- exceptions;
- payment-related information the driver is authorized to see;
- offline state; and
- safe, large, readable controls.

The interface must not encourage interaction while the vehicle is moving.

## 8. Pilot load admission

Every proposed pilot load passes an admission check before publication and again before award.

Admission checks include:

- pilot dates;
- participant eligibility;
- cargo boundary;
- equipment boundary;
- origin and destination boundary;
- load value;
- payment-provider readiness;
- authority and insurance status;
- driver and equipment readiness;
- facility support;
- prohibited commodity and hazmat screening;
- required data completeness;
- route and tracking support;
- support coverage; and
- system health.

Any failed mandatory check blocks admission and provides a plain-language reason.

An administrator cannot bypass a mandatory pilot boundary through an ordinary interface.

## 9. Release management

Production changes use:

- versioned builds;
- signed release artifacts where supported;
- reproducible build instructions;
- dependency and secret scans;
- database migration plans;
- backup before migration;
- tested rollback or forward repair;
- staged deployment;
- smoke tests;
- change approval;
- release notes;
- known limitations;
- support notification; and
- post-deployment verification.

Emergency fixes still require review, testing proportional to risk, an audit record, and a later full retrospective.

Direct unrecorded production edits are prohibited.

## 10. Environment separation

Maintain separate:

- development;
- automated test;
- provider sandbox;
- pilot staging;
- pilot production; and
- public website environments.

Production data is not copied into development.

Secrets, databases, object storage, queues, payment references, tracking integrations, audit keys, logs, domains, and access policies are environment-specific.

The pilot environment uses production-grade security even though its scale is small.

## 11. Observability

Monitoring must cover:

- API availability and latency;
- authentication and authorization failures;
- database, cache, queue, and object-storage health;
- payment funding, release, webhook, and reconciliation states;
- tracking freshness and data-source health;
- notification delivery;
- document upload and scanning;
- pickup and delivery verification;
- exception and dispute queues;
- audit-chain verification;
- backup completion and restore status;
- client versions and crash reports;
- security events;
- privacy-sensitive administrative access; and
- capacity and cost.

Dashboards distinguish healthy, degraded, unavailable, delayed, and unknown.

No dashboard displays unnecessary private shipment, financial, identity, or location information.

## 12. Service objectives

Before launch, record measurable pilot targets for:

- service availability;
- page and API response time;
- funding confirmation;
- tracking freshness;
- pickup and delivery evidence processing;
- payment-release instruction;
- notification delivery;
- support acknowledgement;
- urgent case response;
- backup completion;
- recovery time objective;
- recovery point objective; and
- audit-proof availability.

Targets are pilot goals, not deceptive guarantees.

Every missed target creates a reviewable event and contributes to the pilot decision.

## 13. Support operations

Support must have:

- staffed schedule;
- primary and backup contacts;
- severity levels;
- response targets;
- participant identity-verification procedure;
- approved administrative tools;
- access reason codes;
- escalation matrix;
- payment-provider escalation;
- tracking-provider escalation;
- privacy escalation;
- security escalation;
- legal and compliance escalation;
- outage communication templates;
- safe evidence collection;
- case handoff;
- shift handoff; and
- post-incident review.

Support never asks users to send passwords, one-time codes, payment credentials, signing keys, or unnecessary private records.

## 14. Incident response

Incident categories include:

- account compromise;
- unauthorized access;
- data exposure;
- lost or stolen device;
- payment mismatch;
- duplicate or incorrect release request;
- provider outage;
- tracking outage;
- wrong-driver or wrong-equipment assignment;
- wrong-location delivery attempt;
- evidence corruption;
- malware upload;
- service outage;
- failed migration;
- failed backup or restore;
- audit-chain failure;
- suspected cargo theft;
- safety emergency; and
- suspected prohibited or hazardous cargo.

Every incident has:

- identifier;
- commander;
- severity;
- start and detection times;
- affected services and participants;
- containment;
- evidence preservation;
- communications;
- regulatory and contractual assessment;
- recovery;
- root-cause analysis;
- corrective actions;
- owners and deadlines; and
- closure approval.

Safety emergencies are directed to the appropriate emergency services and carrier or facility contacts. ClearHaul does not pretend software support replaces emergency response.

## 15. Kill switches and containment

Authorized operators must be able to separately disable:

- new account onboarding;
- new load posting;
- bidding;
- final award;
- payment funding requests;
- voluntary early release;
- automatic release requests when provider integrity is uncertain;
- new tracking sessions;
- document uploads;
- notifications;
- public verification;
- testnet anchoring; and
- specific compromised integrations.

A kill switch:

- requires authorization;
- records the reason;
- preserves existing data;
- does not rewrite prior events;
- shows an honest user-facing status;
- has a recovery checklist; and
- does not silently seize earned money.

Disabling a payment request does not create a legal right to retain funds. Payment operations must coordinate with the licensed provider and approved incident process.

## 16. Business continuity

Document and test:

- loss of the primary server;
- database failure;
- Redis failure;
- object-storage failure;
- queue backlog;
- payment-provider outage;
- tracking-provider outage;
- notification-provider outage;
- DNS or certificate failure;
- administrator lockout;
- region or hosting outage;
- corrupted deployment;
- unavailable support lead;
- ransomware or destructive attack; and
- loss of the development computer.

Degraded operation must identify what users can safely do and what is blocked.

Offline driver evidence preserves original observed time and later received time. It never fabricates continuous tracking.

## 17. Backup and disaster recovery

Pilot backups include:

- source and build manifests;
- database;
- object metadata and approved documents;
- configuration without plaintext secrets;
- audit records;
- compliance source versions;
- payment and reconciliation references;
- cases and evidence metadata;
- release artifacts; and
- recovery instructions.

Required recovery tests:

- point-in-time database recovery;
- object and metadata reconciliation;
- clean-environment service restoration;
- payment-ledger reconciliation after recovery;
- audit-chain verification;
- participant access restoration;
- expired-secret and rotated-key handling; and
- documented recovery within approved objectives.

A backup is not trusted until restored.

## 18. Security validation

Before pilot launch:

- threat model is current;
- dependency, secret, and static scans pass or findings are accepted in writing;
- authentication and session controls pass;
- organization isolation passes;
- authorization tests cover every role;
- administrative access is tested;
- webhook security passes;
- rate limits and abuse controls pass;
- file-upload defenses pass;
- logging redaction passes;
- backup encryption passes;
- client update integrity passes;
- incident exercises pass;
- production secrets are inventoried; and
- no default, shared, or sample credentials remain.

An independent security review is strongly recommended before any expansion beyond the controlled pilot.

## 19. Privacy validation

Before launch, verify:

- data inventory;
- purpose and legal basis documentation;
- participant notices;
- driver tracking consent and visibility;
- tracking start and stop behavior;
- minimum necessary collection;
- retention schedules;
- deletion and legal-hold behavior;
- access and export process;
- administrative access logging;
- vendor data use and deletion terms;
- incident notification process;
- redaction;
- public verifier privacy; and
- production telemetry.

Driver location is not sold, used for unrelated surveillance, or retained indefinitely.

## 20. Payment operations

The pilot payment runbook covers:

- shipper provider onboarding;
- carrier payout onboarding;
- funding confirmation;
- award gate;
- release-policy visibility;
- voluntary early release;
- verified-delivery automatic release;
- failed release;
- provider outage;
- webhook delay;
- duplicate event;
- reconciliation difference;
- disputed and undisputed amount;
- refund or reversal authority;
- chargeback handling;
- support boundaries; and
- daily reconciliation signoff.

ClearHaul does not manually move money outside the approved provider workflow.

No rating, marketplace suspension, or support decision silently changes the payment ledger.

## 21. Pilot communication

Participants receive plain-language:

- pilot purpose;
- included and excluded services;
- system limitations;
- support hours;
- emergency contacts;
- expected response times;
- tracking behavior;
- payment release rules;
- evidence requirements;
- rating and reporting rules;
- privacy practices;
- downtime process;
- participant responsibilities;
- feedback method;
- suspension and withdrawal process; and
- notice that the pilot may be stopped.

Marketing must not call the pilot nationwide, fully automated, guaranteed, regulator-approved, bank-like, broker-free as a legal conclusion, or safe for hazmat.

## 22. Pilot execution phases

### Phase A: Internal rehearsal

- Synthetic participants and loads
- Full workflow rehearsal
- Failure injection
- Support drills
- Recovery test
- No real shipment or money

### Phase B: Shadow operation

- Approved real operational scenarios may be observed
- ClearHaul does not control the actual transaction
- Compare system predictions and records with established processes
- Resolve gaps before live control

### Phase C: Limited live loads

- Written launch approval
- Small named cohort
- Low simultaneous-load limit
- Low financial-exposure limit
- Staffed monitoring
- Daily review
- Immediate stop authority

### Phase D: Stabilization

- No scope expansion
- Fix verified defects
- Analyze support, payment, tracking, usability, security, and privacy results
- Repeat recovery and reconciliation

### Phase E: Closeout

- Complete every pilot load
- Reconcile every payment
- Close or transfer every case
- Terminate unnecessary access
- Preserve required evidence
- Produce final report
- Decide go, hold, revise, or stop

## 23. Pilot metrics

Measure counts and distributions, not vanity percentages alone:

- invited and successfully onboarded participants;
- verification failures and reasons;
- admitted and rejected loads;
- bids and awards;
- load cancellations and causes;
- pickup and delivery verification success;
- manual interventions;
- tracking gaps and duration;
- route and identity exceptions;
- document failures;
- payment-funding and release timing;
- reconciliation differences;
- detention events;
- reports, claims, disputes, and appeals;
- rating participation and challenges;
- support contacts by category;
- incidents by severity;
- client crashes;
- service-objective misses;
- privacy and security events;
- backup and recovery results;
- user task-completion results; and
- participant feedback.

Metrics exclude unrelated driver surveillance and misleading aggregate scores.

## 24. Usability validation

Test critical workflows with actual representatives of:

- shipper operations;
- small carriers;
- owner-operators;
- company drivers;
- dispatchers;
- administrators; and
- support personnel.

Validate:

- understandable language;
- readable controls in a truck cab;
- keyboard and screen-reader access;
- color and contrast;
- error recovery;
- offline behavior;
- low-connectivity behavior;
- notification clarity;
- payment understanding;
- tracking transparency;
- evidence capture;
- report and appeal usability; and
- prevention of dangerous interaction while driving.

Usability findings are defects or documented risks, not decorative feedback.

## 25. Pilot daily controls

During limited live operation:

- review platform health;
- review admitted loads;
- review funding and release states;
- reconcile provider records;
- review stale tracking and exceptions;
- review unresolved pickup and delivery evidence;
- review support queue;
- review reports and urgent cases;
- verify backup completion;
- review administrative access;
- review security alerts;
- record decisions and known problems; and
- confirm whether the pilot remains inside its limits.

The daily signoff identifies the responsible person and time.

## 26. Automatic stop conditions

The pilot stops admitting new loads when:

- payment state cannot be trusted;
- automatic release may issue incorrectly;
- shipment organization isolation fails;
- participant identity cannot be trusted;
- material private data is exposed;
- tracking or delivery verification creates an unacceptable wrong-location risk;
- backup or recovery capability is lost beyond the approved window;
- audit history integrity fails;
- a critical security vulnerability is active;
- support coverage is unavailable;
- prohibited or hazardous freight enters the pilot;
- required insurance or authority expires;
- the pilot exceeds its approved financial, load, geographic, or participant limit; or
- Michael Stokes or the named incident authority orders a stop.

Existing loads move into the safest approved continuity plan. A stop is not permission to abandon freight, evidence, communication, or earned payment obligations.

## 27. Required tests

Automated and operational tests include:

- unapproved users cannot join the pilot;
- out-of-scope loads are rejected;
- expired authority or insurance blocks new award;
- system health can block admission without corrupting existing loads;
- environment data and credentials remain separated;
- a failed deployment can be recovered;
- every kill switch affects only its approved capability;
- payment reconciliation survives restart and restore;
- location tracking starts and stops correctly;
- driver offline evidence synchronizes without inventing data;
- support access requires identity and reason;
- simulated account compromise is contained;
- simulated payment-provider outage follows the runbook;
- simulated tracking outage shows an honest state;
- a wrong-location delivery attempt cannot auto-complete;
- backup restore meets the approved recovery objective;
- restored audit proofs still verify;
- pilot limits cannot be bypassed through an ordinary administrator screen;
- participant offboarding removes unnecessary access;
- no hazmat load can be admitted; and
- production XRPL anchoring remains disabled.

## 28. Demonstration and pilot rehearsal

Before live approval, conduct a complete synthetic rehearsal:

1. Onboard one shipper, one carrier, two drivers, two facilities, support, and an administrator.
2. Admit one eligible load and reject four out-of-scope loads for different reasons.
3. Fund, bid, award, assign equipment, approve history, and dispatch.
4. Lose connectivity and recover offline evidence.
5. Raise and resolve a tracking exception.
6. Attempt a wrong-location delivery and block it.
7. Verify delivery correctly and release payment once.
8. Reconcile the payment provider.
9. Submit a report, response, decision, and appeal.
10. Trigger separate payment, tracking, upload, and onboarding kill switches.
11. Restore from backup into a clean environment.
12. Verify the complete audit chain and public proof package.
13. Offboard the synthetic participants.
14. Produce the same evidence package required for pilot closeout.

## 29. Definition of done

Milestone 6 is complete only when:

- every entry gate and approval is recorded;
- pilot boundaries are technically enforced;
- participant onboarding and offboarding work;
- production-like deployment and rollback work;
- monitoring, alerting, support, and incident runbooks are exercised;
- payment, tracking, delivery, rating, reporting, appeal, and audit flows pass;
- kill switches are tested;
- privacy and security validation pass;
- backup and disaster recovery pass;
- usability findings are resolved or accepted in writing;
- all pilot loads and payments are reconciled;
- all participant access is reviewed;
- metrics and incidents are reported honestly;
- mandatory project documents are updated;
- a checkpoint commit and tagged pilot build exist;
- an encrypted milestone backup is restored;
- a final pilot report is approved; and
- Michael records a go, hold, revise, or stop decision.

Passing technical tests alone does not authorize public expansion.

## 30. Explicit exclusions

Milestone 6 does not authorize:

- nationwide public launch;
- uncontrolled enrollment;
- live hazmat operation;
- specialized equipment;
- international freight;
- ClearHaul custody of funds;
- payment outside the licensed provider;
- cryptocurrency or stablecoin payment;
- production XRP Ledger anchoring without separate written approval;
- sale or unrelated use of location data;
- removal of human support;
- automatic legal or insurance determinations;
- hiding known failures from participants;
- calling a pilot result a guarantee; or
- expansion before pilot closeout.

## 31. Stop conditions for engineering

Engineering stops and records a question when:

- pilot scope, participant count, load count, geography, dates, or financial limit is not approved;
- a live provider contract or credential is required;
- a legal, insurance, tax, payment, employment, privacy, transportation, or regulatory conclusion would be guessed;
- support or incident authority is unnamed;
- a live load would exceed tested capabilities;
- a production migration lacks a verified backup and recovery path;
- a participant asks to bypass identity, funding, equipment, tracking, delivery, or evidence controls;
- an undisputed earned payment would be held as leverage;
- a safety or privacy risk cannot be contained; or
- the system cannot tell users the truth about its current state.

## 32. Handoff to Milestone 7

Milestone 7 may plan regional expansion, additional lanes, larger financial limits, more participants, mobile-platform expansion, approved integrations, and advanced equipment types only after the controlled pilot closes successfully and Michael gives a new written scope approval.
