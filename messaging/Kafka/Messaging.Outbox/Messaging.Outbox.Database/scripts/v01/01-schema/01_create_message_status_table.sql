--liquibase formatted sql

/*
 * Create the message status table
 *
 * Author: Webster
 * Date: 2024-02-01
 */

-- changeset webster:v1-initial-creation
-- comment: create message status enum
CREATE TYPE message_status_enum AS ENUM (
    'Not set', 
    'Saved', 
    'Not persisted', 
    'Possibly persisted', 
    'Persisted'
);
-- DROP TYPE IF EXISTS message_status_enum;