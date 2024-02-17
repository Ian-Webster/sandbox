--liquibase formatted sql

/*
 * Create the message status table
 *
 * Author: Webster
 * Date: 2024-02-01
 */

-- changeset webster:v1-initial-creation
-- comment: create message status
CREATE TABLE messages_status (
    status_id smallint PRIMARY KEY,
    status_name varchar(50) NOT NULL
);
-- rollback DROP TABLE IF EXISTS messages_status;