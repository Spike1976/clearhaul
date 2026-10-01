# Payments boundary

Project: ClearHaul
Written: 2026-09-30
Status: boundary note only. This is not a design and not an implementation.

Read with DECISIONS.md and docs/source/ENGINEERING_OPERATING_ORDER.md. If Michael Stokes writes a newer direction, that direction wins.

## What exists

No payment code exists. This foundation has no payment route, payment table, payment provider, wallet, fee, or ledger entry.

## Later scope, not a design

The initial version, when it is eventually specified, is a USD ledger. That sentence is a scope boundary for work that has not been specified. It is not a product decision, and it does not describe how the ledger works.

Out of scope:

- Cryptocurrency payments
- A project token, including a ClearHaul token
- Stablecoins
- User wallets

The ledger connection in the engineering operating order, when it is built, is for audit anchoring only. It is not a payment system. That connection is not built. This note does not design it.

## Named and not designed

Double-entry accounting, funding states, release conditions, accessorial changes, disputes, reconciliation, and a sandbox payment provider are future work. They are not designed here. Naming them does not create rules for them.

Do not invent account names, ledger entries, fees, or release rules. This file invents none.

## Later question

A regulated payment provider cannot be chosen until Michael decides and a qualified reviewer is involved.

Recorded here for later, and not opened in QUESTIONS_FOR_MICHAEL.md:

Which regulated payment provider, if any, may ClearHaul use, and who is the qualified reviewer for that choice?

No provider is selected. No reviewer is named. No default is used. The engineering operating order does not allow a default for an irreversible financial choice.

## Not a product decision

No financial assumption in this file is a product decision. Recorded decisions do not authorize a financial rule. This note does not add one.

Releasing funds stays prohibited without explicit authorization. That restates the operating order. It is not a release rule.
