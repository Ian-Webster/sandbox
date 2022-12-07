--liquibase formatted sql

/*
 * Insert data into the Genre table
 *
 * Author: Webster
 * Date: 2022-12-06
 */

-- changeset webster:v1-initial-creation
-- comment: insert genre data
INSERT INTO public."Genre" VALUES('1','Action');
INSERT INTO public."Genre" VALUES('2','Adventure');
INSERT INTO public."Genre" VALUES('3','Animation');
INSERT INTO public."Genre" VALUES('4','Biography');
INSERT INTO public."Genre" VALUES('5','Comedy');
INSERT INTO public."Genre" VALUES('6','Crime');
INSERT INTO public."Genre" VALUES('7','Drama');
INSERT INTO public."Genre" VALUES('8','Family');
INSERT INTO public."Genre" VALUES('9','Fantasy');
INSERT INTO public."Genre" VALUES('10','History');
INSERT INTO public."Genre" VALUES('11','Horror');
INSERT INTO public."Genre" VALUES('12','Musical');
INSERT INTO public."Genre" VALUES('13','Romance');
INSERT INTO public."Genre" VALUES('14','Sci-Fi');
INSERT INTO public."Genre" VALUES('15','Thriller');
--rollback TRUNCATE TABLE public."Genre" CASCADE;