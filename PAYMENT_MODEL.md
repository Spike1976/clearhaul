# Payment model

There is no bank, escrow account, card processor, or wallet in this repository.

`SandboxFundingAdapter` returns `simulated: true` and the provider label `sandbox-not-a-bank`. The ledger refuses a post that is not simulated and refuses a post that does not balance to zero. A repeated webhook event id does not post again.

Funding a shipment moves the sandbox clearing account and `ShipmentReserved` by opposite amounts. Platform operating balance stays zero until a release. Release moves reserved funds to `CarrierPayable` and `PlatformOperating` using the amounts on the tender. The platform charge is an input. No commission percentage is calculated.

After a carrier is selected, a shipper withdrawal request is refused and the payment state becomes manual review. Reserved funds stay in place.

Detention evidence adds a proposed accessorial. It does not accept itself and does not release funds. Disputing one accessorial line changes only that line.

A production funding result is rejected and sent to manual review.
