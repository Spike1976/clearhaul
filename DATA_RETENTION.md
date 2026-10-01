# Data retention

No retention period is approved. `RetentionPolicy.Unset` stores a null duration and the disposition `tombstone-only-until-legal-review`.

`RetentionActions.RequestPhysicalDelete` fails for every class: shipment operations, equipment-history snapshot, audit, payment ledger, and driver packet.

The domain audit log and the SQL triggers refuse ordinary deletion. A refused delete attempt appends another audit event. Closing a shipment expires shipper access to equipment history. It does not delete the history or the relied-upon hash.

An attorney still has to set any real retention period. Nothing in the backup tool is a legal retention rule.
