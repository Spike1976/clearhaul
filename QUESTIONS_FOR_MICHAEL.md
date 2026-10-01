# Questions for Michael Stokes

Project: ClearHaul
Product owner: Michael Stokes
File opened: 2026-09-30

How to answer a question:
Type your letter under "Michael's answer:". Add a short note if you want. Save this file.

Active questions are first. Answered questions will be moved to the bottom.

Eight questions are open. The engineering order required seven. One more was added because the blueprint and the master build prompt were not found on this computer. No further questions will be added until one of these is answered, unless there is an emergency.

Work that can be undone is continuing. No legal, financial, hazmat, or production-blockchain choice has been assumed.

QUESTION CH-0008

Status: ANSWER NEEDED
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

Decision recorded:

QUESTION CH-0001

Status: ANSWER NEEDED
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

Decision recorded:

QUESTION CH-0002

Status: ANSWER NEEDED
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

Decision recorded:

QUESTION CH-0003

Status: ANSWER NEEDED
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

Decision recorded:

QUESTION CH-0004

Status: ANSWER NEEDED
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

Decision recorded:

QUESTION CH-0005

Status: ANSWER NEEDED
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

Decision recorded:

QUESTION CH-0006

Status: ANSWER NEEDED
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

Decision recorded:

QUESTION CH-0007

Status: ANSWER NEEDED
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

Decision recorded:
