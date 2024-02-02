--liquibase formatted sql

/*
 * Create the message table
 *
 * Author: Webster
 * Date: 2024-02-01
 */

-- changeset webster:v1-initial-creation
-- comment: create message table
CREATE TABLE public.message (
    message_id uuid NOT NULL,
    message_content bytea NOT NULL,
    created_date timestamp with time zone NOT NULL,
    sent_date timestamp with time zone,
    status_id smallint NOT NULL DEFAULT 0,
	error text,
    CONSTRAINT message_pk PRIMARY KEY (message_id),
	FOREIGN KEY (status_id) REFERENCES messages_status(status_id)
);
-- rollback DROP TABLE IF EXISTS public.message