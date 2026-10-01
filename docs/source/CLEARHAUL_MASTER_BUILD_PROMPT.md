# ClearHaul master build prompt

Date recorded: 2026-09-30
Source: the product assignment that directed Phase Zero. A separate binary was not attached. This file is the build instruction that was read.

## Build rule

Build the transaction, identity, rules, compliance, payment, and audit engines before decorative interface work. Do not present a fake control as a working feature. Do not claim a feature works unless it is implemented and tested.

## Stack already in this repository

The assignment says to use TypeScript, Next.js, and NestJS unless an existing repository already has a suitable stack. This repository already has .NET 8, ASP.NET Core, Avalonia, PostgreSQL/PostGIS compose files, Redis, and MinIO. Decision CH-D-0023 keeps that stack. A second web stack is not started.

## Phase Zero

Phase Zero is the rules and data foundation: domain model, state machines, permission matrix, audit, regulatory-source model, load-tender schema, retention, errors, notifications, and evidence. Every transition names an actor, previous state, required data, validation, resulting state, audit event, notification, reversal, and failure behavior.

The implementation of that phase is the `ClearHaul.Domain` library and `db/migrations/0002_phase_zero.sql`. The migration has not been applied.

## Continue only as far as the evidence allows

The assignment says to continue into the MVP after Phase Zero. The MVP screens, Android driver app, bank integration, and live hazmat decisions are not implemented. They stay blocked until their external dependencies exist and the tests for those surfaces exist. Do not fill the gap with a simulated success that is labeled as production.

## Definition of done

A feature is done only when the workflow exists, authorization is enforced, data is persisted correctly, validation exists, audit events are generated, errors are handled, tests pass, documentation is updated, and known limits are disclosed. An in-memory domain test does not by itself prove a persisted or user-facing workflow.
