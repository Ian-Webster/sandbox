--liquibase formatted sql

/*
 * Insert data into the Role table
 *
 * Author: Webster
 * Date: 2022-12-06
 */

-- changeset webster:v1-initial-creation
-- comment: insert role data
INSERT INTO public."Role" VALUES('1','Director');
INSERT INTO public."Role" VALUES('2','Producer');
INSERT INTO public."Role" VALUES('3','Cast');
INSERT INTO public."Role" VALUES('4','Crew');
--rollback TRUNCATE TABLE public."Role" CASCADE;