# Security model

This file lists the controls the operating order requires and the state of each one in the foundation. A control is implemented only when the repository contains it.

Implemented in this foundation:

- Secrets are not committed. A test scans for private-key blocks, Amazon-style access-key prefixes, and certificate or key files.
- Package versions in project files are explicit. Lock files are generated on restore.
- The health server returns problem responses without exception text.
- Responses set `X-Content-Type-Options`, `X-Frame-Options`, `Referrer-Policy`, `Content-Security-Policy`, and `Cache-Control: no-store`.
- The server removes the `Server` header when it can.
- A rate limit of 60 requests per minute per remote address is installed outside the test environment.
- The Windows health client allows only `http` and `https`.
- Backup payloads use AES-256-GCM. The command-line key file uses Windows DPAPI.
- The health description text does not include connection strings. The server does not open a database connection.

Not implemented:

- Authentication, authorization, sessions, and multifactor authentication
- TLS termination
- Role and shipment permissions
- Encryption at rest for a database
- Malware scanning
- Audit logging of business actions
- Dependency vulnerability gate in continuous integration beyond NuGet audit during restore
- Software bill of materials
- Installer signing
- Production signing-key isolation
- Off-computer backup

The two health routes are unauthenticated on purpose. They return no freight data. That is not an authentication system.
