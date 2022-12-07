--liquibase formatted sql

/*
 * Create the MovieParticipant table
 *
 * Author: Webster
 * Date: 2022-12-06
 */

-- changeset webster:v1-initial-creation
-- comment: create movie participant table
CREATE TABLE public."MovieParticipant" (
	"MovieId" uuid NOT NULL,
	"PersonId" uuid NOT NULL,
	"RoleId" integer NOT NULL,
    CONSTRAINT "MovieParticipant_pk" PRIMARY KEY ("MovieId", "PersonId", "RoleId"),
	CONSTRAINT "MovieParticipant_Movie_fk" FOREIGN KEY ("MovieId") REFERENCES public."Movie"("MovieId"),
	CONSTRAINT "MovieParticipant_Person_fk" FOREIGN KEY ("PersonId") REFERENCES public."Person"("PersonId"),
	CONSTRAINT "MovieParticipant_Role_fk" FOREIGN KEY ("RoleId") REFERENCES public."Role"("RoleId")
);
-- rollback DROP TABLE IF EXISTS public."MovieParticipant"