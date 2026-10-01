# Foundation health contract

Status: authoritative for the foundation checkpoint.
Stage: foundation.
This contract does not describe a freight API. No shipment, payment, identity, or compliance route exists.

## Base address

Local development address: `http://127.0.0.1:5080`

The foundation process listens on loopback HTTP only. TLS is not terminated by this process. That is a known gap, not a completed control.

## GET /health/live

Response: `200` with `Content-Type: application/json`.

```json
{ "status": "Healthy" }
```

No other fields are returned.

## GET /health

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

Rules:

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

## Unknown routes

Any other path returns `404` with `Content-Type: application/problem+json`.

The body contains `title` and `status`. It must not contain a stack trace, an internal source path, or an exception message.

## Headers

Every response includes:

- `X-Content-Type-Options: nosniff`
- `X-Frame-Options: DENY`
- `Referrer-Policy: no-referrer`
- `Content-Security-Policy: default-src 'none'; frame-ancestors 'none'`
- `Cache-Control: no-store`

The `Server` header is removed when the host adds it.

## Rate limit

Outside the `Testing` environment, each remote address is limited to 60 requests per minute. The limit response is `429` with a problem body and no internal detail.

The test host uses the environment name `Testing` so the limit does not make the test suite order-dependent.

## Logging

Request logs may include the method, path, and status code. They must not include request bodies, query secrets, connection strings, or authorization headers.

## What this contract does not include

- Accounts, passwords, or sessions
- Shipment routes
- Payment routes
- Document upload
- Audit persistence
- XRP Ledger calls
- A browser API page
