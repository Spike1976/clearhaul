# Hazmat engine

Hazmat facts can be stored on a load tender. The shipper confirmation flag starts false until a shipper user sets it. The software does not classify a material from a name, a note, or an image.

Live pilot behavior:

- A hazmat tender that is not marked as an exercise cannot be published.
- An exercise shipment can move through assignment so the gate can be tested. It sets dispatch blocked.
- Driver acknowledgement on a blocked shipment does not enter Ready for pickup.
- Changing the hazmat quantity clears shipper confirmation, bumps the packet revision, and leaves the local packet stale until a later sync.
- Departure of a hazmat shipment is blocked by the live rules result.

Missing classification returns UNABLE TO DETERMINE. Incompatible materials do not receive a clearance, because no compatibility table is loaded. The result is the same qualified-review block.

This is not a live hazmat operation. CH-D-0015 still requires qualified review before any live use.
