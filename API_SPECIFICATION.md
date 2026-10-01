# API specification

This specification matches `docs/contracts/foundation-health.md`. That contract is authoritative for the foundation checkpoint. This file lists no other operations. It is not a freight API.

The server is not started unless `ARCHITECTURE.md` says a server project file is on disk. The integrator has not verified any server. No product route works.

## Base address

Local development address: `http://127.0.0.1:5080`

The foundation process, when it exists, listens on loopback HTTP only. TLS is not terminated by this process. That is a known gap, not a completed control.

## Operations

Only two operations are specified.

### GET /health/live

Response: `200` with `Content-Type: application/json`.

```json
{ "status": "Healthy" }
```

No other fields are returned.

This route is unauthenticated once the server exists. See `PERMISSION_MATRIX.md`.

### GET /health

Response: `200` with `Content-Type: application/json`.

```json
{
  "status": "Degraded",
  "stage": "foundation",
  "version": "0.1.0-foundation",
  "checks": [
    {
      "name": "self",
      "status": "Healthy",
      "description": "The process handled this request."
    },
    {
      "name": "postgres",
      "status": "NotConfigured",
      "description": "Not implemented in the foundation build."
    },
    {
      "name": "redis",
      "status": "NotConfigured",
      "description": "Not implemented in the foundation build."
    },
    {
      "name": "objectStorage",
      "status": "NotConfigured",
      "description": "Not implemented in the foundation build."
    }
  ]
}
```

Rules from the health contract:

- `self` is `Healthy` when the process handles the request.
- `postgres`, `redis`, and `objectStorage` are `NotConfigured` in this build.
- The server has no connection setting that turns those checks on.
- The server must not open a database, Redis, or object-storage connection.
- Overall `status` is `Healthy` only when every check is `Healthy`.
- Overall `status` is `Degraded` when any check is `Degraded` or `NotConfigured` and none are `Unhealthy`.
- Overall `status` is `Unhealthy` when any check is `Unhealthy`.
- Descriptions must not contain connection strings, passwords, file paths, exception text, or host names from configuration.
- The version value is exactly `0.1.0-foundation`.
- The stage value is exactly `foundation`.

This route is unauthenticated once the server exists. See `PERMISSION_MATRIX.md`.

## Responses that are not operations

Any path other than the two operations above returns `404` with `Content-Type: application/problem+json`.

The body contains `title` and `status`. It must not contain a stack trace, an internal source path, or an exception message.

That `404` is not an authorized operation. No other route is specified.

Outside the `Testing` environment, each remote address is limited to 60 requests per minute. The limit response is `429` with a problem body and no internal detail. The `429` response is not an additional operation.

The test host uses the environment name `Testing` so the limit does not make the test suite order-dependent.

## Headers

Every response includes:

- `X-Content-Type-Options: nosniff`
- `X-Frame-Options: DENY`
- `Referrer-Policy: no-referrer`
- `Content-Security-Policy: default-src 'none'; frame-ancestors 'none'`
- `Cache-Control: no-store`

The `Server` header is removed when the host adds it.

## Logging

Request logs may include the method, path, and status code. They must not include request bodies, query secrets, connection strings, or authorization headers.

## What is not specified

No other operation is listed. In particular, this specification does not include:

- Accounts, passwords, or sessions
- Shipment routes
- Payment routes
- Document upload
- Audit persistence
- XRP Ledger calls
- A browser API page
