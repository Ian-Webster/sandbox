--liquibase formatted sql

/*
 * Create the Movie table
 *
 * Author: Webster
 * Date: 2022-12-06
 */

-- changeset webster:v1-initial-creation
-- comment: create movie table
CREATE TABLE public."Movie" (
	"MovieId" uuid NOT NULL,
	"Name" varchar(255) NOT NULL,
	CONSTRAINT "Movie_pk" PRIMARY KEY ("MovieId")
);
-- rollback DROP TABLE IF EXISTS public."Movie"