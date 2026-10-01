-- Phase Zero schema. This file has not been applied.
-- Docker has not been started. These tables are the relational form of the
-- in-process domain model. They do not contain a legal rule or a payment provider.

CREATE TABLE app.organization (
    id uuid PRIMARY KEY,
    kind text NOT NULL CHECK (kind IN ('shipper', 'carrier', 'facility', 'platform')),
    legal_name text NOT NULL,
    dba text,
    verified boolean NOT NULL DEFAULT false,
    synthetic boolean NOT NULL DEFAULT false,
    created_at timestamptz NOT NULL DEFAULT now()
);

CREATE TABLE app.app_user (
    id uuid PRIMARY KEY,
    organization_id uuid NOT NULL REFERENCES app.organization (id),
    display_label text NOT NULL,
    removed boolean NOT NULL DEFAULT false,
    mfa_required boolean NOT NULL DEFAULT false,
    synthetic boolean NOT NULL DEFAULT false
);

CREATE TABLE app.user_role (
    user_id uuid NOT NULL REFERENCES app.app_user (id),
    role_name text NOT NULL,
    PRIMARY KEY (user_id, role_name)
);

CREATE TABLE app.shipment (
    id uuid PRIMARY KEY,
    shipper_organization_id uuid NOT NULL REFERENCES app.organization (id),
    carrier_organization_id uuid REFERENCES app.organization (id),
    state text NOT NULL,
    version integer NOT NULL DEFAULT 0,
    hazmat_designated boolean NOT NULL DEFAULT false,
    hazmat_exercise boolean NOT NULL DEFAULT false,
    dispatch_blocked boolean NOT NULL DEFAULT false,
    bound_rule_package_id text,
    assigned_trailer_id text,
    assigned_driver_user_id uuid,
    history_access_expires_at timestamptz,
    relied_upon_history_hash text,
    created_at timestamptz NOT NULL DEFAULT now(),
    CHECK (hazmat_exercise = false OR hazmat_designated = true)
);

CREATE TABLE app.shipment_transition (
    id uuid PRIMARY KEY,
    shipment_id uuid NOT NULL REFERENCES app.shipment (id),
    from_state text NOT NULL,
    to_state text NOT NULL,
    actor_user_id uuid NOT NULL,
    organization_id uuid NOT NULL,
    reason text NOT NULL,
    correlation_id text NOT NULL,
    occurred_at timestamptz NOT NULL
);

CREATE TABLE app.load_tender (
    shipment_id uuid PRIMARY KEY REFERENCES app.shipment (id),
    legal_shipper_name text NOT NULL,
    bill_to_party text NOT NULL,
    commodity_description text NOT NULL,
    commodity_category text NOT NULL,
    piece_count integer NOT NULL CHECK (piece_count > 0),
    package_type text NOT NULL,
    weight_grams bigint NOT NULL CHECK (weight_grams > 0),
    trailer_type text NOT NULL,
    hazmat_designated boolean NOT NULL,
    total_amount_cents bigint NOT NULL,
    carrier_payout_cents bigint NOT NULL,
    platform_charge_cents bigint NOT NULL,
    payment_release_condition text NOT NULL,
    CHECK (carrier_payout_cents + platform_charge_cents = total_amount_cents)
);

CREATE TABLE app.load_stop (
    id uuid PRIMARY KEY,
    shipment_id uuid NOT NULL REFERENCES app.shipment (id),
    role text NOT NULL CHECK (role IN ('pickup', 'delivery')),
    line1 text NOT NULL,
    locality text NOT NULL,
    region text NOT NULL,
    postal_code text NOT NULL,
    country text NOT NULL,
    latitude double precision NOT NULL CHECK (latitude BETWEEN -90 AND 90),
    longitude double precision NOT NULL CHECK (longitude BETWEEN -180 AND 180),
    time_zone text NOT NULL,
    address_validation_result text NOT NULL
);

CREATE TABLE app.ledger_entry (
    id uuid PRIMARY KEY,
    event_id text NOT NULL,
    shipment_id uuid NOT NULL REFERENCES app.shipment (id),
    account_name text NOT NULL,
    signed_cents bigint NOT NULL,
    simulated boolean NOT NULL CHECK (simulated = true),
    occurred_at timestamptz NOT NULL,
    UNIQUE (event_id, account_name)
);

CREATE TABLE app.equipment_history_entry (
    id uuid PRIMARY KEY,
    trailer_id text NOT NULL,
    cargo_category text NOT NULL,
    confidence text NOT NULL,
    source_name text NOT NULL,
    recorded_by_user_id uuid NOT NULL,
    organization_id uuid NOT NULL,
    amends_entry_id uuid REFERENCES app.equipment_history_entry (id),
    amendment_reason text,
    evidence_id text,
    recorded_at timestamptz NOT NULL,
    CHECK (confidence <> 'third-party-verified' OR source_name <> 'carrier'),
    CHECK (amends_entry_id IS NULL OR amendment_reason IS NOT NULL)
);

CREATE TABLE app.equipment_history_access (
    id uuid PRIMARY KEY,
    shipment_id uuid NOT NULL REFERENCES app.shipment (id),
    actor_user_id uuid NOT NULL,
    organization_id uuid NOT NULL,
    trailer_id text NOT NULL,
    action_name text NOT NULL CHECK (action_name IN ('view', 'export', 'download')),
    occurred_at timestamptz NOT NULL
);

CREATE TABLE audit.audit_event (
    id uuid PRIMARY KEY,
    actor_user_id uuid NOT NULL,
    organization_id uuid NOT NULL,
    role_name text NOT NULL,
    event_name text NOT NULL,
    original_value text,
    new_value text,
    occurred_at timestamptz NOT NULL,
    source_name text NOT NULL,
    reason text NOT NULL,
    shipment_id uuid,
    equipment_id text,
    document_hash text,
    rule_version text,
    correlation_id text NOT NULL
);

CREATE TABLE app.rule_package (
    id text PRIMARY KEY,
    version_label text NOT NULL,
    effective_on date NOT NULL,
    retired_on date,
    approved_for_live boolean NOT NULL DEFAULT false CHECK (approved_for_live = false),
    synthetic_test_fixture boolean NOT NULL,
    created_at timestamptz NOT NULL DEFAULT now()
);

CREATE OR REPLACE FUNCTION app.reject_mutation()
RETURNS trigger
LANGUAGE plpgsql
AS $$
BEGIN
    RAISE EXCEPTION 'append-only table % cannot be updated or deleted', TG_TABLE_NAME;
END;
$$;

CREATE TRIGGER shipment_transition_append_only
    BEFORE UPDATE OR DELETE ON app.shipment_transition
    FOR EACH ROW EXECUTE FUNCTION app.reject_mutation();

CREATE TRIGGER ledger_entry_append_only
    BEFORE UPDATE OR DELETE ON app.ledger_entry
    FOR EACH ROW EXECUTE FUNCTION app.reject_mutation();

CREATE TRIGGER equipment_history_append_only
    BEFORE UPDATE OR DELETE ON app.equipment_history_entry
    FOR EACH ROW EXECUTE FUNCTION app.reject_mutation();

CREATE TRIGGER audit_event_append_only
    BEFORE UPDATE OR DELETE ON audit.audit_event
    FOR EACH ROW EXECUTE FUNCTION app.reject_mutation();

CREATE TRIGGER shipment_no_delete
    BEFORE DELETE ON app.shipment
    FOR EACH ROW EXECUTE FUNCTION app.reject_mutation();
