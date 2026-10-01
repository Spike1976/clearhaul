# Roadmap

Source: `docs/source/ENGINEERING_OPERATING_ORDER.md`. The blueprint and the master build prompt were not supplied. This list does not invent product behavior.

The operating order's first assignment is the foundation checkpoint. That assignment is not a future milestone. This file does not mark the foundation complete. Server and client status is in `ARCHITECTURE.md`.

The operating order does not number later milestones and does not give dates or completion amounts. The sequence below follows dependencies stated by that order: no product behavior before a blueprint, no persisted product data before the named stores exist, no anchoring before an audit ledger exists, no production anchoring before separate approval, and no publication before a license decision. Every milestone below is not started.

1. Product blueprint and master build prompt
   - Status: not started.
   - The operating order is not the product blueprint. Product workflows stay unspecified until the real documents are supplied.

2. PostgreSQL with PostGIS
   - Status: not started.
   - The foundation health contract reports this store as not configured and forbids a connection. No business table is specified.

3. Redis
   - Status: not started.
   - The foundation health contract reports Redis as not configured and forbids a connection.

4. Object storage
   - Status: not started.
   - The foundation health contract reports object storage as not configured and forbids a connection. The operating order names uploaded documents as a later feature. This milestone does not design that feature.

5. Append-only audit ledger
   - Status: not started.
   - The operating order says the audit ledger is append-only once it exists, and that corrections are new events. No audit table is specified.

6. Freight workflows
   - Status: not started.
   - The first assignment is not product screens. Business aggregates are not specified. No freight workflow works.

7. Payment workflows
   - Status: not started.
   - Payment behavior is not specified. The operating order puts cryptocurrency payments, a ClearHaul token, stablecoin payments, and user wallets out of scope. Releasing funds is not authorized. No payment state is specified.

8. XRP Ledger test-network anchoring
   - Status: not started.
   - When anchoring is built, the operating order limits it to audit anchoring on a test network. It is not connected. It is not a payment system.

9. Production XRP Ledger anchoring
   - Status: not started.
   - Production activation is separately approved in writing by Michael Stokes. Decision CH-D-0019 names him as the only approver. Testnet work is not a production approval. This milestone is not started.

10. Publication
    - Status: not started.
    - Decision CH-D-0014 adopts AGPL version 3 as the working license and still requires a final legal and open-source review. Decision CH-D-0021 publishes the foundation repository.
