# Compliance engine

`RulesEngine` stores draft rule packages. A package cannot be saved with `ApprovedForLive` true. No method can turn that flag on.

Live evaluation, which the shipment workflow uses, returns:

- Outcome: UNABLE TO DETERMINE
- Decision code: QUALIFIED HAZMAT REVIEW REQUIRED
- The facts that were supplied
- No source citation
- Human confirmation required
- Dispatch blocked

A synthetic fixture can be evaluated only when the caller passes live evaluation as false. The decision is labeled `DETERMINED_BY_FIXTURE`. It is not a regulation and it is not used by shipment dispatch.

The engine does not contain a copied statute, placard table, or compatibility chart. An active shipment keeps the rule-package id it received at publish. A newer package does not replace that id.

Jurisdiction, effective dates, and citation fields exist on the decision and the package. They are empty or fixture-labeled until a reviewed source is loaded. No source has been loaded.
