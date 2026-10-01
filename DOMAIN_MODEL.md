# Domain model

Phase Zero lives in `src/ClearHaul.Domain`. It is an in-process model. It is not exposed on the HTTP server. The product name is not part of a state, a table, or a rule.

## Aggregates

- Actor and organization, with one or more platform roles.
- Load tender, with structured parties, stops, freight, equipment, schedule, money, and compliance facts. Notes cannot replace a required field.
- Shipment, whose state follows `TransitionCatalog`.
- Payment account and balanced ledger. Shipment funds and platform operating funds are different accounts.
- Equipment history book and shipment-scoped access.
- Washout evidence.
- Rule package, citation, and compliance decision.
- Audit log, notification log, evidence chain, signed document link, and retention policy.
- Verification snapshot, bank-change hold, compensation disclosure, and enforcement case.

## Shipment transitions

`TransitionCatalog.All` is the contract. Each entry names the actor, previous state, required data, validation, audit event, notification, reversal, and failure behavior. Failed transitions leave the state unchanged and append a refusal audit event.

## Equipment approval

`EquipmentApprovalStateMachine.Project` maps the shipment state to assignment pending, assigned, review, cleaning requested, substitution requested, approved, or not in equipment review. Rejecting a trailer requests substitution and does not clear the selected carrier.

## Relationship sketch

```text
organization 1---* app_user 1---* user_role
organization 1---* shipment 1---1 load_tender
shipment 1---* shipment_transition
shipment 1---* load_stop
shipment 1---* ledger_entry
shipment 1---* equipment_history_access
equipment_history_entry 0---* amendment
shipment *---1 rule_package
audit_event records actor, shipment, and rule version
```

The SQL form is `db/migrations/0002_phase_zero.sql`. It has not been applied.
