# API specification

The only HTTP operations are the foundation health routes. Phase Zero domain operations are in-process method calls. They are not REST routes and they are not in the OpenAPI file.

OpenAPI file: `docs/openapi/foundation-health.yaml`.

## Base address

Local development address: `http://127.0.0.1:5080`

Loopback HTTP only. TLS is not terminated here.

## GET /health/live

`200` `application/json`: `{ "status": "Healthy" }`.

## GET /health

`200` `application/json` with service checks. PostgreSQL, Redis, and object storage are `NotConfigured`. Overall status is `Degraded`.

## Any other path

`404` `application/problem+json` without a stack trace.

## Domain operations that are not HTTP

Shipment transitions, ledger posts, history views, rule evaluation, and audit appends are methods on `ClearHaul.Domain`. A future versioned REST API has to be added only when those methods are wired to authenticated routes and covered by contract tests. That wiring is not done.
