--liquibase formatted sql

/*
 * Create the Genre table
 *
 * Author: Webster
 * Date: 2022-12-06
 */

-- changeset webster:v1-initial-creation
-- comment: create genre table
CREATE TABLE public."Genre" (
	"GenreId" integer NOT NULL,
	"Name" varchar(64) NOT NULL,
	CONSTRAINT "Genre_pk" PRIMARY KEY ("GenreId")
);
-- rollback DROP TABLE IF EXISTS public."Genre"