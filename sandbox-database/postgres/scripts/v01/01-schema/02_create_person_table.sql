--liquibase formatted sql

/*
 * Create the Person table
 *
 * Author: Webster
 * Date: 2022-12-06
 */

-- changeset webster:v1-initial-creation
-- comment: create person table
CREATE TABLE public."Person" (
	"PersonId" uuid NOT NULL,
	"FirstName" varchar(255) NOT NULL,
	"LastName" varchar(255) NOT NULL,
	CONSTRAINT "Person_pk" PRIMARY KEY ("PersonId")
);
-- rollback DROP TABLE IF EXISTS public."Person"