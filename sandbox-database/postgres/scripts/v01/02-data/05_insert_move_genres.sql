--liquibase formatted sql

/*
 * Insert data into the MovieGenre table
 *
 * Author: Webster
 * Date: 2022-12-06
 */

-- changeset webster:v1-initial-creation
-- comment: insert movie genre data
INSERT INTO public."MovieGenre"  VALUES('a2f2ee47-6084-484a-969a-aec304734623',12);
INSERT INTO public."MovieGenre"  VALUES('347f229a-3eec-48f6-96b1-6c11474a0414',4);
INSERT INTO public."MovieGenre"  VALUES('9ceda051-a93e-4b83-9f8f-2c1ffe8a6510',10);
INSERT INTO public."MovieGenre"  VALUES('72c6bfef-90b3-4acb-a2a8-d9491badad03',13);
INSERT INTO public."MovieGenre"  VALUES('983f7645-bbc1-4622-b6d1-3172d6aa8152',3);
INSERT INTO public."MovieGenre"  VALUES('ed18f4dd-4c04-4006-b33c-74f3fc016a26',9);
INSERT INTO public."MovieGenre"  VALUES('525f384c-7e99-4f0f-91d6-73a938bb5e76',14);
INSERT INTO public."MovieGenre"  VALUES('abe39e00-6990-4db4-9790-90d055b2ffca',12);
INSERT INTO public."MovieGenre"  VALUES('0c2be663-cb3a-4381-b723-1345bcbd6b7b',3);
INSERT INTO public."MovieGenre"  VALUES('2db180d7-ad54-4653-8951-21a3efc9b277',1);
INSERT INTO public."MovieGenre"  VALUES('91948c64-bd94-4976-9d0c-43b925212409',12);
INSERT INTO public."MovieGenre"  VALUES('7f839458-c8df-49b7-b643-8e2281250c4b',7);
INSERT INTO public."MovieGenre"  VALUES('e4c97b67-6168-4fe9-84f0-f8cb7b11d674',8);
INSERT INTO public."MovieGenre"  VALUES('76c19392-13ad-44a4-85ef-88288b139634',4);
INSERT INTO public."MovieGenre"  VALUES('9e8757a4-bf3c-4387-acb9-cf1d5b2eb4ba',15);
INSERT INTO public."MovieGenre"  VALUES('70229a51-df5b-4909-838e-f04383387442',6);
INSERT INTO public."MovieGenre"  VALUES('098fda70-78f7-4f07-90f0-4d6ad2961246',5);
INSERT INTO public."MovieGenre"  VALUES('36fa8bdc-de6a-447f-92b5-11cb5d7271e2',10);
INSERT INTO public."MovieGenre"  VALUES('1810faa8-75a0-4cbd-b688-c34ee4706700',11);
INSERT INTO public."MovieGenre"  VALUES('28163f55-d39d-4f15-baeb-429e167d2c6e',11);
INSERT INTO public."MovieGenre"  VALUES('09305239-5ee2-4131-9108-1cc01b609755',6);
INSERT INTO public."MovieGenre"  VALUES('488f8d56-efc6-4bf9-90aa-82c245720ca9',9);
INSERT INTO public."MovieGenre"  VALUES('2b84a535-8f08-4175-addb-3e6908cdfd6b',3);
INSERT INTO public."MovieGenre"  VALUES('01b27e6a-53c9-42dc-bbb0-197ce773899a',14);
INSERT INTO public."MovieGenre"  VALUES('34327101-a216-41e1-9c7f-c32c62756ca5',6);
INSERT INTO public."MovieGenre"  VALUES('aa9a0941-1671-4a0c-89f9-247fe72184f9',12);
INSERT INTO public."MovieGenre"  VALUES('f362fa9a-1d0b-4660-8ae2-715b498bf14e',12);
INSERT INTO public."MovieGenre"  VALUES('954a65f5-3056-4f83-9b3e-89d5348b48c5',15);
INSERT INTO public."MovieGenre"  VALUES('24d818ef-48ea-4a8e-ac52-75de7dae3529',4);
INSERT INTO public."MovieGenre"  VALUES('d0541c33-c39c-4163-afde-89eb77eff898',9);
INSERT INTO public."MovieGenre"  VALUES('4053b290-79aa-488f-90f3-1e687d888d27',15);
INSERT INTO public."MovieGenre"  VALUES('2b0e7580-cdd0-4e13-80ef-548e9d611e81',2);
INSERT INTO public."MovieGenre"  VALUES('e0ee5538-fc83-46d2-8bdf-16609d4ae6dc',11);
INSERT INTO public."MovieGenre"  VALUES('fbf1d087-124b-4107-9322-c205f58ff262',5);
INSERT INTO public."MovieGenre"  VALUES('b057b489-a763-4e94-ad6c-111ddee229aa',5);
INSERT INTO public."MovieGenre"  VALUES('62260611-a3ec-4ddd-b9ae-12e224886b5c',7);
INSERT INTO public."MovieGenre"  VALUES('efc3d10c-2317-45dc-8ce7-a054866dd3a5',6);
INSERT INTO public."MovieGenre"  VALUES('b890481d-9217-43b3-9e4e-9996100ed82b',13);
INSERT INTO public."MovieGenre"  VALUES('6227ee59-6d74-4db5-9076-dd345ebbf5f2',1);
INSERT INTO public."MovieGenre"  VALUES('3acb0b46-e464-4ae5-b006-513e65436d49',14);
INSERT INTO public."MovieGenre"  VALUES('8cb83e29-b07e-4ea1-9dc9-773315277392',15);
INSERT INTO public."MovieGenre"  VALUES('06433fef-5286-4ea0-b998-1fa7a44f9342',8);
INSERT INTO public."MovieGenre"  VALUES('400b92b9-1a79-4d25-a543-dde12a12d723',12);
INSERT INTO public."MovieGenre"  VALUES('c750730e-8235-4230-8ac2-639196e3ef55',14);
INSERT INTO public."MovieGenre"  VALUES('e1427cf9-0419-422d-bea4-4488ed39c423',4);
INSERT INTO public."MovieGenre"  VALUES('10a7abb0-e28c-454b-b771-8e99a948c55f',12);
INSERT INTO public."MovieGenre"  VALUES('e52f199d-ecb3-420f-9115-5f7a98d4edd1',12);
INSERT INTO public."MovieGenre"  VALUES('2f4527a6-6913-4550-8e04-78b07380d3f5',5);
INSERT INTO public."MovieGenre"  VALUES('79e03427-4c26-40af-936e-147f1cb651b7',6);
INSERT INTO public."MovieGenre"  VALUES('f177fa5a-b08d-46e1-97d6-04da42b5d090',15);
--rollback TRUNCATE TABLE public."MovieGenre" CASCADE;