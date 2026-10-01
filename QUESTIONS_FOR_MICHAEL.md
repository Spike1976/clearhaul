# Questions for Michael Stokes

Project: ClearHaul
Product owner: Michael Stokes
File opened: 2026-09-30

How to answer a question:
Type your letter under "Michael's answer:". Add a short note if you want. Save this file.

Active questions are first.

## Active questions

None. CH-0009 through CH-0018 were answered in writing on 2026-10-01. The answers are recorded on those questions below.

QUESTION CH-0009

Status: ANSWERED
Priority: BLOCKING
Area: Setup

Plain-language question:
May I install Docker Desktop and start the local Compose stack for PostgreSQL, Redis, and MinIO?

Why this matters:
Milestone 2 cannot start until Milestone 0 is proven. Docker is not installed, so PostgreSQL, Redis, and document storage cannot be shown healthy. Decision CH-D-0017 says Docker Desktop is not installed without a separate permission. Marketplace screens, persisted bids, and a database restore cannot be completed until this is answered.

Recommended choice:
A. Yes. Install Docker Desktop and start the local Compose stack.

Available choices:

A. Yes. Install Docker Desktop and start the local Compose stack.
Effect: The entry gate can be tested against PostgreSQL, Redis, and MinIO. Marketplace implementation can continue after that gate passes.

B. No. Do not install Docker.
Effect: Marketplace work stays blocked. In-process tests can continue. No database is created.

C. Use a database host I will name in the note below. Do not install Docker Desktop.
Effect: Work waits until the host and credentials are provided outside the repository.

Default if Michael does not choose:
Docker is not installed. Milestone 2 does not start. No database is created.

Michael's answer:
A. Yes. Install Docker Desktop and start the local Compose stack.

Decision recorded:
CH-D-0031. Docker Desktop may be installed, and the local Compose stack may be started. Installation had not been run at the time this answer was recorded.

QUESTION CH-0010

Status: ANSWERED
Priority: BLOCKING
Area: Privacy

Plain-language question:
How many prior cargo records should ordinarily be disclosed for an assigned trailer?

Why this matters:
The shipper may see safety-relevant history only after conditional selection and trailer assignment. A count that is too small can hide a relevant prior load. A count that is too large can expose more of the carrier's history than the privacy rule allows. This count is not chosen in the milestone order.

Recommended choice:
Leave the count unset until you write it here.

Available choices:

A. I will write the ordinary number of prior records in the note below.
Effect: Disclosure uses that count after the entry gate passes.

B. Disclose every stored record inside the review window, with no separate count.
Effect: The time or load window in CH-0011 becomes the only limit.

C. Do not disclose prior cargo records until a later written rule.
Effect: Assignment can be designed later, and cargo-history disclosure stays closed.

Default if Michael does not choose:
No disclosure count is in effect. Protected cargo history stays closed.

Michael's answer:
A. Ordinarily disclose the previous three cargo records for the assigned trailer.

Decision recorded:
CH-D-0032. The ordinary disclosure count is three cargo records. CH-0011 limits which three.

QUESTION CH-0011

Status: ANSWERED
Priority: BLOCKING
Area: Privacy

Plain-language question:
Should the disclosure period use a number of loads, a time period, or both?

Why this matters:
The review window decides which history the selected shipper can see. Choosing loads, time, or both changes what is private. The milestone order asks this and does not answer it.

Recommended choice:
Leave the window unset until you write it here.

Available choices:

A. Use a number of loads. I will write the number below.
Effect: Older loads outside that count stay hidden.

B. Use a time period. I will write the period below.
Effect: Records outside that period stay hidden.

C. Use both a load count and a time period. I will write both below.
Effect: A record is disclosed only when it falls inside both limits.

Default if Michael does not choose:
No disclosure window is in effect. Protected cargo history stays closed.

Michael's answer:
C. Use both: the previous three loads within 90 days. Safety incidents and legally required records follow their longer retention rules.

Decision recorded:
CH-D-0033. Ordinary disclosure is the previous three loads that also fall within 90 days. This answer does not name a statute or a longer retention period. Those longer rules stay unstated until a written retention schedule names them.

QUESTION CH-0012

Status: ANSWERED
Priority: BLOCKING
Area: Safety

Plain-language question:
Which cargo categories require an automatic warning to the shipper?

Why this matters:
An automatic warning treats some prior cargo as safety-relevant without a person deciding load by load. The wrong list warns on ordinary freight or stays silent on freight you want called out. The milestone order names a starting taxonomy and does not say which categories warn automatically.

Recommended choice:
Leave automatic warnings off until you name the categories.

Available choices:

A. I will list the categories that must warn automatically.
Effect: Those categories can raise a warning after you name them. Other categories stay factual records.

B. Do not warn automatically. Show the disclosed categories and let the shipper read them.
Effect: The screen shows categories, confidence, and dispute status. It does not add an automatic warning.

C. Warn on every disclosed category.
Effect: Every prior category in the disclosure is marked as a warning.

Default if Michael does not choose:
No category raises an automatic warning.

Michael's answer:
B. Display cargo categories factually. Do not generate automatic safety warnings until a qualified hazmat or food-safety professional approves the warning rules.

Decision recorded:
CH-D-0034. Disclosed categories are shown as facts. No automatic safety warning is generated. Live hazmat remains disabled.

QUESTION CH-0013

Status: ANSWERED
Priority: BLOCKING
Area: Privacy

Plain-language question:
How long may the shipper access the assigned trailer's history after delivery?

Why this matters:
Access that continues after delivery can expose history during a dispute. Access that ends too soon can block a legitimate review. The milestone order asks for the duration and does not set it.

Recommended choice:
Leave the duration unset until you write it here.

Available choices:

A. I will write the duration below.
Effect: Interactive access ends at that time. The approval snapshot remains in the audit record.

B. Access ends at delivery.
Effect: Post-delivery review requires a separate administrative grant.

C. Access remains for an authorized dispute and has no ordinary calendar limit.
Effect: A dispute process must exist before this choice can be enforced.

Default if Michael does not choose:
No post-delivery access period is in effect. Interactive history access stays closed.

Michael's answer:
A. Allow access until 72 hours after delivery. Preserve the approval snapshot permanently. Extend controlled access during an authorized dispute.

Decision recorded:
CH-D-0035. Interactive access lasts until 72 hours after delivery. The approval snapshot is kept. An authorized dispute can extend controlled access. This answer does not define the dispute procedure.

QUESTION CH-0014

Status: ANSWERED
Priority: BLOCKING
Area: Operations

Plain-language question:
Which cleaning requirements may the shipper select?

Why this matters:
The milestone order names candidate requirements, including sweep-out, washout, sanitization, allergen, kosher, halal, pharmaceutical, and customer-specific work. It does not say which of those a shipper may impose on a conditionally selected carrier.

Recommended choice:
Leave shipper selection closed until you name the allowed requirements.

Available choices:

A. The shipper may select any requirement named in the Milestone 3 order.
Effect: The named list becomes the shipper's selectable set after you confirm this choice.

B. I will write a shorter allowed list below.
Effect: Only the written list can be selected.

C. The shipper may state a requirement, and a person must review it before it binds the carrier.
Effect: Nothing in the list binds the carrier automatically.

Default if Michael does not choose:
The shipper cannot select a binding cleaning requirement.

Michael's answer:
C. A shipper can request any cleaning requirement, but it binds the carrier only when the original tender includes it or the carrier accepts it through a written change order.

Decision recorded:
CH-D-0036. A cleaning request may be recorded. It binds the carrier only from the original tender or a written change order.

QUESTION CH-0015

Status: ANSWERED
Priority: BLOCKING
Area: Contract

Plain-language question:
May a shipper require a specific washout facility?

Why this matters:
A named facility changes the carrier's obligation and may change price and schedule. The milestone order allows a required facility type and asks whether a specific facility may be required. It does not answer that.

Recommended choice:
Leave specific-facility requirements unavailable until you answer.

Available choices:

A. Yes. The shipper may name a specific facility.
Effect: The carrier must use that facility when the requirement is accepted.

B. No. The shipper may name a facility type only.
Effect: The carrier chooses a facility of that type.

C. A specific facility requires a separate written agreement for that load.
Effect: The load screen can record the request. It does not bind the carrier by itself.

Default if Michael does not choose:
A shipper cannot require a specific washout facility.

Michael's answer:
C. A specific washout facility requires a separate written agreement for that load.

Decision recorded:
CH-D-0037. Naming a facility does not bind the carrier unless that load has a separate written agreement.

QUESTION CH-0016

Status: ANSWERED
Priority: BLOCKING
Area: Contract

Plain-language question:
Who pays for a washout requested after the carrier is conditionally selected?

Why this matters:
This assigns a cost. The milestone order says cost responsibility must not be decided without an approved rule or agreement. No agreement is on file.

Recommended choice:
Leave the payer unset until you write the rule.

Available choices:

A. The shipper pays.
Effect: The load records the shipper as the payer for a post-selection washout.

B. The carrier pays.
Effect: The load records the carrier as the payer for a post-selection washout.

C. The accepted bid, or a later written agreement, names the payer. Until then, nobody is charged in the software.
Effect: The software stores the request and does not assign a payer.

Default if Michael does not choose:
No payer is assigned. The software does not charge either party.

Michael's answer:
C. The accepted bid or a written change order identifies who pays. The software never assigns a payer on its own.

Decision recorded:
CH-D-0038. Washout cost responsibility comes from the accepted bid or a written change order. The software does not choose a payer.

QUESTION CH-0017

Status: ANSWERED
Priority: BLOCKING
Area: Evidence

Plain-language question:
What evidence is required before a cleaning record is called verified?

Why this matters:
Calling a record verified tells the shipper that someone other than the carrier has confirmed it. A receipt, a photograph, or a carrier statement is not that confirmation unless you say so. The milestone order forbids treating a carrier declaration as independent verification, and it forbids a photograph from proving cleanliness.

Recommended choice:
Leave the verified label unused until you name the evidence.

Available choices:

A. I will write the required evidence below.
Effect: The verified label is available only when that evidence is present and a person confirms it.

B. A carrier upload stays a carrier declaration. An authorized shipper or administrator must confirm it before it is called verified.
Effect: Uploads keep the carrier-declared confidence until that confirmation.

C. Do not use a verified label in this milestone.
Effect: Records show source and confidence. They do not say verified.

Default if Michael does not choose:
No cleaning record is called verified.

Michael's answer:
B. Carrier uploads remain carrier declarations until an authorized shipper, a facility integration, or an administrator confirms them.

Decision recorded:
CH-D-0039. A carrier upload stays a carrier declaration. Confirmation by an authorized shipper, a facility integration, or an administrator is required before the record is called verified. A photograph still does not prove cleanliness.

QUESTION CH-0018

Status: ANSWERED
Priority: BLOCKING
Area: Trust

Plain-language question:
When may an equipment rejection affect the carrier's rating?

Why this matters:
A rating change is a penalty. The milestone order says rejecting one trailer must not cancel the award, suspend the carrier, or create a negative rating by itself. It still asks when a rejection may affect a rating later.

Recommended choice:
Leave rating effects off until you write the rule.

Available choices:

A. A single trailer rejection never affects the rating.
Effect: Rejection stays on the load record. The rating does not change.

B. I will write the conditions below.
Effect: A rating change can be built only for those conditions.

C. Any rating effect requires a separate enforcement review.
Effect: Rejection can open a report. It does not change a rating by itself.

Default if Michael does not choose:
An equipment rejection does not change the carrier's rating.

Michael's answer:
C. Trailer rejection can create a report, but any rating penalty requires review. One rejection never automatically changes the carrier's rating.

Decision recorded:
CH-D-0040. A rejection may create a report. It does not cancel the carrier award. A rating change requires a separate review.

Answered questions are below. They were answered in writing on 2026-09-30.

The blueprint file and the master build prompt were still not in docs\source when these answers were recorded. Their required names are:

- docs\source\Project_ClearHaul_Blueprint.docx
- docs\source\CLEARHAUL_MASTER_BUILD_PROMPT.md

Product workflows stay unbuilt until both files are present and have been read.

Later note, 2026-09-30: Decision CH-D-0022 stored the blueprint chat text as docs\source\Project_ClearHaul_Blueprint.md and the master build prompt as docs\source\CLEARHAUL_MASTER_BUILD_PROMPT.md. The .docx binary is still absent. Milestone 2 and Milestone 3 were ordered after that decision. Neither milestone is implemented. See CH-D-0029 and CH-D-0030.

Later note, 2026-10-01: Michael answered CH-0009 through CH-0018. Those answers are CH-D-0031 through CH-D-0040. The master prompt markdown is already in docs\source. The blueprint .docx is still not in docs\source. Milestone 2 waits for a healthy local Compose stack. Milestone 3 waits for Milestone 2.

## Answered questions

QUESTION CH-0008

Status: ANSWERED
Priority: IMPORTANT
Area: Other

Plain-language question:
Where are the ClearHaul blueprint and the master build prompt? They were not attached to the work session, and they are not in the projects folder.

Why this matters:
The foundation can be built from the engineering order. The real product workflows cannot. If those documents stay missing, shipment, payment, and compliance behavior would be guessed. Guessed behavior will not be built.

Recommended choice:
A. Give the full file path, or place the files in docs/source inside the ClearHaul project.

Available choices:

A. The files are on this computer and the path will be written below.
Effect: The next work session reads them before any product feature is designed.

B. Those documents do not exist yet. The engineering order is the only specification for now.
Effect: Foundation work continues. Product workflows stay unbuilt until a specification is written.

C. The documents were supposed to be attached and should be sent again.
Effect: Foundation work continues. Product workflows wait until the files arrive.

Default if Michael does not choose:
Foundation work continues from the engineering order only. Product workflows are not started. This does not invent a blueprint.

Michael's answer:
C. The blueprint and master build prompt were supposed to be provided. I will place both files in the project's docs\source folder. Do not design product workflows until you have read both completely.

Required filenames:
docs\source\Project_ClearHaul_Blueprint.docx
docs\source\CLEARHAUL_MASTER_BUILD_PROMPT.md

Decision recorded:
The engineering foundation may continue. Product workflows require both source documents. Recorded as CH-D-0013.

QUESTION CH-0001

Status: ANSWERED
Priority: IMPORTANT
Area: Legal

Plain-language question:
Should the ClearHaul server and core use the AGPL version 3 license?

Why this matters:
The license decides what other people may do with the source code. AGPL version 3 requires someone who runs a modified ClearHaul server to offer the source code of their version. A permissive license lets another company keep its changes private. This can be changed before the project is published. It should not be changed after other people have relied on it.

Recommended choice:
A. Use AGPL version 3 for the server and core.

Available choices:

A. Use AGPL version 3 for the server and core.
Effect: A public hosted copy of a modified ClearHaul server must make its source available. Private internal changes still require the operator to offer source to the users of that server.

B. Use the Apache 2.0 license.
Effect: Other people can use and change the code in closed products. They do not have to share their changes.

C. Do not put a license on the project until a later decision.
Effect: Nobody has clear permission to use the code. The project should not be published.

Default if Michael does not choose:
A copy of the AGPL version 3 text is stored as the recommended license and marked provisional. It is not treated as adopted. The project will not be published until this question is answered. The file can be replaced.

Michael's answer:
A. Use AGPL version 3 for the ClearHaul server and core. Keep the license provisional until the project receives a final legal and open-source review before public release.

Decision recorded:
AGPLv3 is the approved working license for the server and core. Integration libraries may later use Apache 2.0 if separately approved. Recorded as CH-D-0014. A later written direction on 2026-09-30 told engineering to put the repository on the public GitHub remote. That direction is CH-D-0021. The final legal and open-source review is still not done.

QUESTION CH-0002

Status: ANSWERED
Priority: IMPORTANT
Area: Hazmat

Plain-language question:
Is the first real pilot limited to ordinary domestic dry-van freight that is not hazardous?

Why this matters:
Hazardous freight has extra legal and safety rules. Those rules must come from a qualified person. They will not be invented in code. A narrow first pilot is safer and smaller.

Recommended choice:
A. Yes. Limit the first pilot to nonhazardous domestic dry-van freight.

Available choices:

A. Yes. The first pilot is nonhazardous domestic dry-van freight only.
Effect: Refrigerated freight, flatbed freight, hazardous freight, and international freight stay out of the first pilot.

B. Include refrigerated freight that is still not hazardous.
Effect: The first pilot grows by one equipment type. Hazardous freight stays out. A later specification must describe temperature data before that workflow is built.

C. Include hazardous freight in the first pilot.
Effect: Work on hazardous-material rules stops until a qualified hazmat professional and an attorney review the rules. Those rules will not be drafted from guesswork.

Default if Michael does not choose:
Documents and future design notes treat the first pilot as nonhazardous domestic dry-van freight. No hazardous-material rules are written. This assumption can be changed before any pilot data is stored.

Michael's answer:
A. Limit the first live pilot to nonhazardous domestic dry-van freight.

The hazmat database, rules engine, schemas, test cases and driver information system should still be developed in parallel. Hazmat operation must not be activated for live loads until it has been reviewed by qualified hazmat and legal professionals.

Decision recorded:
The first live pilot is nonhazardous domestic dry-van freight. Hazmat foundations are developed but remain disabled for live transportation until reviewed and approved. Recorded as CH-D-0015. Hazmat design waits until the blueprint and master build prompt have been read, because those workflows must not be invented.

QUESTION CH-0003

Status: ANSWERED
Priority: IMPORTANT
Area: User Interface

Plain-language question:
Should the first Windows program show four workspaces: shipper, carrier, driver testing, and administrator?

Why this matters:
The first window should not pretend those jobs are finished. The question is only which workspace names appear on the foundation shell.

Recommended choice:
A. Show all four names, with each one marked as not built.

Available choices:

A. Show shipper, carrier, driver testing, and administrator.
Effect: The window lists all four. None of them open a working workspace yet.

B. Show shipper and carrier only.
Effect: Driver testing and administrator names stay off the window until a later decision.

C. Show administrator only.
Effect: The first window is limited to an administrator label. The other workspaces stay off the window.

Default if Michael does not choose:
The window lists all four names and says each one is not built. No workspace workflow is created. The labels can be removed later.

Michael's answer:
A. Show shipper, carrier, driver testing and administrator workspaces.

During the foundation milestone, clearly mark unfinished workspaces as not built. As each milestone is completed, replace the placeholder with the real working workspace. Do not leave fake buttons or pretend unfinished screens work.

Decision recorded:
The Windows client will contain four role-controlled workspaces: shipper, carrier, driver testing and administrator. Recorded as CH-D-0016.

QUESTION CH-0004

Status: ANSWERED
Priority: IMPORTANT
Area: Other

Plain-language question:
May PostgreSQL, MinIO, and Redis run through Docker Desktop on this computer during development?

Why this matters:
The project needs a database, file storage, and a short-term work queue. Docker Desktop is not installed on this computer today. The compose file can be written, but the services cannot be started until Docker is available or another install method is chosen.

Recommended choice:
A. Yes. Use Docker Desktop for local development.

Available choices:

A. Yes. Install and use Docker Desktop for PostgreSQL, MinIO, and Redis.
Effect: Local development matches the compose file. No database is installed directly on Windows.

B. No. Install PostgreSQL, MinIO, and Redis directly on Windows.
Effect: Docker is not used. The setup is different from the compose file and must be documented separately before it is trusted.

C. Leave the database, file storage, and Redis until a later week.
Effect: The server remains a foundation program with no data store. Product data cannot be saved yet.

Default if Michael does not choose:
The compose file is prepared and is not started. Nothing is installed. No database is created.

Michael's answer:
A. Use Docker Desktop for PostgreSQL, MinIO and Redis during local development.

Prepare the Docker Compose environment now. Do not install or change protected system software without permission. Provide plain-language installation instructions if Docker Desktop must be installed manually.

Decision recorded:
Docker Desktop and Docker Compose are the approved local-development method for PostgreSQL, MinIO, Redis and supporting services. Recorded as CH-D-0017. Docker Desktop was not installed by engineering.

QUESTION CH-0005

Status: ANSWERED
Priority: IMPORTANT
Area: Security

Plain-language question:
Where should encrypted copies of the project be stored away from this computer?

Why this matters:
A backup that stays on the same disk can be lost with the disk. An off-computer copy needs a place you choose. A destination will not be guessed.

Recommended choice:
A. Use an encrypted external drive, and write the drive path below when it is plugged in.

Available choices:

A. An encrypted external drive.
Effect: Weekly copies can go to that drive after you provide the path. Until then, only local encrypted backups exist.

B. Another computer or a network drive on the same premises.
Effect: Weekly copies can go there after you provide the path and the access method.

C. An encrypted cloud location.
Effect: No cloud account will be chosen for you. Copies start only after you name the service and the protected location.

Default if Michael does not choose:
No off-computer copy is made. Local encrypted backups are stored at C:\Users\17402\ClearHaul-Backups. That folder is outside the source repository.

Michael's answer:
A. Use an encrypted external drive as the first off-computer backup destination.

Until I provide the external-drive path, continue encrypted rolling backups at:
C:\Users\17402\ClearHaul-Backups

Design the backup system so a second encrypted cloud destination can be added later. Never store plaintext passwords, signing keys, personal information or production secrets in a backup.

Decision recorded:
Local encrypted rolling backups begin immediately. Encrypted external-drive backups begin when Michael provides the drive and path. A second offsite destination remains planned. Recorded as CH-D-0018.

QUESTION CH-0006

Status: ANSWERED
Priority: IMPORTANT
Area: Security

Plain-language question:
Who is allowed to approve turning on the XRP Ledger for production audit anchoring?

Why this matters:
The first release, when it exists, may anchor audit fingerprints to the XRP Ledger test network only. Production anchoring would publish batch fingerprints outside the project. Nobody should be able to turn that on without a named approver. Cryptocurrency payments are not part of this project.

Recommended choice:
A. Only Michael Stokes, in a new written decision.

Available choices:

A. Only Michael Stokes may approve production anchoring, in writing.
Effect: No engineer or agent can turn on production anchoring.

B. Michael Stokes plus one additional named person must both approve it.
Effect: Name the second person in your answer. Until that name is written here, nobody is authorized.

C. Leave production anchoring unauthorized until a later policy.
Effect: Production anchoring stays off. Test-network design can be discussed later, but it is not connected now.

Default if Michael does not choose:
Nobody is authorized. Production anchoring cannot be enabled. No approver is assumed. The test network is not connected in this foundation.

Michael's answer:
A. Only Michael Stokes may approve production XRP Ledger anchoring, and approval must be given in writing.

Testnet integration may be built and tested. Production XRP Ledger access, Mainnet credentials and production anchoring must remain disabled until separately approved.

Decision recorded:
Michael Stokes is the sole production-XRPL approver. Testnet work does not authorize Mainnet activation. Recorded as CH-D-0019.

QUESTION CH-0007

Status: ANSWERED
Priority: ADVISORY
Area: Other

Plain-language question:
Is ClearHaul the working name of the product, or only an internal code name?

Why this matters:
The window title and documents need a name. Renaming later is possible, but it is easier before a public release.

Recommended choice:
A. Use ClearHaul as the working product name.

Available choices:

A. ClearHaul is the working product name.
Effect: The program window, documents, and repository use ClearHaul.

B. ClearHaul is an internal code name. The public product name comes later.
Effect: ClearHaul remains the repository name until you choose the public name. The window can say that the public name is not chosen.

C. Use a different name, written below.
Effect: The window and documents change to that name. The repository folder can be renamed in a separate step.

Default if Michael does not choose:
The window and documents use ClearHaul. The name can be changed before any public release.

Michael's answer:
A. ClearHaul is the working product name.

Keep branding configurable so the public name can be changed later without rewriting the system. Do not hard-code ClearHaul into database rules, API contracts or business logic where configuration is appropriate.

Decision recorded:
ClearHaul is the working product and repository name. The system must remain technically rebrandable. Recorded as CH-D-0020.
