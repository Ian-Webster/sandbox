--liquibase formatted sql

/*
 * Create the Role table
 *
 * Author: Webster
 * Date: 2022-12-06
 */

-- changeset webster:v1-initial-creation
-- comment: create role table
CREATE TABLE public."Role" (
	"RoleId" integer NOT NULL,
	"Name" varchar(128) NOT NULL,
	CONSTRAINT "Role_pk" PRIMARY KEY ("RoleId")
);
-- rollback DROP TABLE IF EXISTS public."Role"