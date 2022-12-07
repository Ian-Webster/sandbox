--liquibase formatted sql

/*
 * Create the MovieGenre table
 *
 * Author: Webster
 * Date: 2022-12-06
 */

-- changeset webster:v1-initial-creation
-- comment: create movie genre table
CREATE TABLE public."MovieGenre" (
	"MovieId" uuid NOT NULL,
	"GenreId" integer NOT NULL,
	CONSTRAINT "MovieGenre_pk" PRIMARY KEY ("MovieId", "GenreId"),
	CONSTRAINT "MovieGenre_Movie_fk" FOREIGN KEY ("MovieId") REFERENCES public."Movie"("MovieId"),
	CONSTRAINT "MovieGenre_Genre_fk" FOREIGN KEY ("GenreId") REFERENCES public."Genre"("GenreId")
);
-- rollback DROP TABLE IF EXISTS public."MovieGenre"