--liquibase formatted sql

/*
 * Insert static data into the message status table
 *
 * Author: Webster
 * Date: 2024-02-01
 */

-- changeset webster:v1-initial-creation
-- comment: insert data into the message status
INSERT INTO messages_status (status_id, status_name) 
VALUES 
    (0, 'NotSet'),
    (1, 'Saved'),
    (2, 'NotPersisted'),
    (3, 'PossiblyPersisted'),
    (4, 'Persisted');

-- ROLLBACK DELETE FROM messages_status WHERE status_id IN (0, 1, 2, 3, 4);