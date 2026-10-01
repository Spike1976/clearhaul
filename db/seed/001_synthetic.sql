-- Synthetic seed. Not mounted by Compose. Not applied.
-- No real person, USDOT number, bank account, or address is included.

INSERT INTO app.organization (id, kind, legal_name, verified, synthetic)
VALUES
    ('00000000-0000-4000-8000-000000000001', 'shipper', 'Synthetic Shipper One', true, true),
    ('00000000-0000-4000-8000-000000000002', 'carrier', 'Synthetic Carrier One', false, true);

INSERT INTO app.app_user (id, organization_id, display_label, mfa_required, synthetic)
VALUES
    ('00000000-0000-4000-8000-000000000011', '00000000-0000-4000-8000-000000000001', 'Synthetic shipper administrator', false, true),
    ('00000000-0000-4000-8000-000000000012', '00000000-0000-4000-8000-000000000002', 'Synthetic dispatcher', false, true);
