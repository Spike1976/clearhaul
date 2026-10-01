# XRP Ledger anchoring boundary

Status: boundary note only. This is not an implementation.
Date: 2026-09-30
Decision: CH-D-0019 supersedes CH-D-0007
Question: CH-0006, answered
Source: `docs/source/ENGINEERING_OPERATING_ORDER.md`, XRP Ledger section

CH-D-0019 is Michael Stokes's decision. Only he may approve production anchoring, and only in writing. Testnet work does not authorize Mainnet.

## What exists

No anchoring code exists. No anchoring client exists. No signing seed exists. No network connection to the XRP Ledger exists. This note does not connect to any network and does not generate a seed or a key.

## Purpose

Anchoring, if it is built later, is for audit fingerprints only. Cryptocurrency payments, a ClearHaul token, stablecoin payments, and user wallets are out of scope.

## Production activation

Michael Stokes is the only production approver. Production XRP Ledger access, Mainnet credentials, and production anchoring stay disabled until a separate written approval. The test network is not connected in this foundation.

## Future test-network publication

A future test-network design may publish only these items:

- a format identifier
- a format version
- a batch identifier
- a Merkle root
- an event count
- a batch time range

The transaction is not chosen. The memo encoding is not chosen. The fee limit is not chosen. The account addresses are not chosen. This note does not design those parameters.

Signing seeds are never stored in source, ordinary configuration, logs, or database rows.

## Private data

Private freight data must not go in a memo. Document contents are not placed on the XRP Ledger.

## Audit ledger

The audit ledger itself is not implemented. Once an audit ledger exists, it is append-only. Corrections, once they exist, must be new events rather than edits. This note does not define a table or a hash format.
