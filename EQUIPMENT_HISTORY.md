# Equipment history

History entries are appended. `Correct` adds an amendment that points at the original id and requires a reason and evidence. The original entry object remains.

Confidence values are carrier declared, driver documented, shipper confirmed, facility confirmed, third-party verified, disputed, and superseded by a documented correction. An entry sourced as carrier cannot be stored as third-party verified.

The projection shown to a shipper has trailer id, unit number, equipment type, cargo category, hazmat, food or feed, allergen, refuse or animal-product, spill, contamination, rejected-load, repair, cleaning, confidence, and source. It has no customer, rate, route, or product-identity field.

`EquipmentHistoryAccess.View` allows the view only for an authorized shipper user on that shipment, after a carrier is selected, and only for the assigned trailer, before access expiry. `SearchFleet` always fails. Successful views and exports are appended to the shipment access log and the audit log.

Approval stores a hash of the entry ids that were relied upon. Later amendments do not change that hash. Closing the shipment sets the access expiry. A later shipper view fails closed.
