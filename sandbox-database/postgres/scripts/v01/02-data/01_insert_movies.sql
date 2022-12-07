--liquibase formatted sql

/*
 * Insert data into the Movie table
 *
 * Author: Webster
 * Date: 2022-12-06
 */

-- changeset webster:v1-initial-creation
-- comment: insert movie data
INSERT INTO public."Movie"  VALUES('a2f2ee47-6084-484a-969a-aec304734623','Terminator 2');
INSERT INTO public."Movie"  VALUES('347f229a-3eec-48f6-96b1-6c11474a0414','Ice Age 1');
INSERT INTO public."Movie"  VALUES('9ceda051-a93e-4b83-9f8f-2c1ffe8a6510','Transformers');
INSERT INTO public."Movie"  VALUES('72c6bfef-90b3-4acb-a2a8-d9491badad03','Avengers Infinity War');
INSERT INTO public."Movie"  VALUES('983f7645-bbc1-4622-b6d1-3172d6aa8152','The Ring');
INSERT INTO public."Movie"  VALUES('ed18f4dd-4c04-4006-b33c-74f3fc016a26','Rambo III');
INSERT INTO public."Movie"  VALUES('525f384c-7e99-4f0f-91d6-73a938bb5e76','Chocolat');
INSERT INTO public."Movie"  VALUES('abe39e00-6990-4db4-9790-90d055b2ffca','Fantastic Beasts and Where To Find Them');
INSERT INTO public."Movie"  VALUES('0c2be663-cb3a-4381-b723-1345bcbd6b7b','The Dark Knight');
INSERT INTO public."Movie"  VALUES('2db180d7-ad54-4653-8951-21a3efc9b277','Avatar 1');
INSERT INTO public."Movie"  VALUES('91948c64-bd94-4976-9d0c-43b925212409','Wonder Woman');
INSERT INTO public."Movie"  VALUES('7f839458-c8df-49b7-b643-8e2281250c4b','Shrek');
INSERT INTO public."Movie"  VALUES('e4c97b67-6168-4fe9-84f0-f8cb7b11d674','American Pie');
INSERT INTO public."Movie"  VALUES('76c19392-13ad-44a4-85ef-88288b139634','Transformers 2');
INSERT INTO public."Movie"  VALUES('9e8757a4-bf3c-4387-acb9-cf1d5b2eb4ba','Avengers End Game');
INSERT INTO public."Movie"  VALUES('70229a51-df5b-4909-838e-f04383387442','Inside Out');
INSERT INTO public."Movie"  VALUES('098fda70-78f7-4f07-90f0-4d6ad2961246','The Martian');
INSERT INTO public."Movie"  VALUES('36fa8bdc-de6a-447f-92b5-11cb5d7271e2','Avengers 1');
INSERT INTO public."Movie"  VALUES('1810faa8-75a0-4cbd-b688-c34ee4706700','Casablanca');
INSERT INTO public."Movie"  VALUES('28163f55-d39d-4f15-baeb-429e167d2c6e','Avengers 2');
INSERT INTO public."Movie"  VALUES('09305239-5ee2-4131-9108-1cc01b609755','Finding Dory');
INSERT INTO public."Movie"  VALUES('488f8d56-efc6-4bf9-90aa-82c245720ca9','Pretty Woman');
INSERT INTO public."Movie"  VALUES('2b84a535-8f08-4175-addb-3e6908cdfd6b','Friends With Kids');
INSERT INTO public."Movie"  VALUES('01b27e6a-53c9-42dc-bbb0-197ce773899a','Pretty Woman 2: back on the streets');
INSERT INTO public."Movie"  VALUES('34327101-a216-41e1-9c7f-c32c62756ca5','Mundane Beasts and How To Get Rid Of Them');
INSERT INTO public."Movie"  VALUES('aa9a0941-1671-4a0c-89f9-247fe72184f9','The Light Knight');
INSERT INTO public."Movie"  VALUES('f362fa9a-1d0b-4660-8ae2-715b498bf14e','Rambo I');
INSERT INTO public."Movie"  VALUES('954a65f5-3056-4f83-9b3e-89d5348b48c5','Ice Age 2');
INSERT INTO public."Movie"  VALUES('24d818ef-48ea-4a8e-ac52-75de7dae3529','American Pie 2');
INSERT INTO public."Movie"  VALUES('d0541c33-c39c-4163-afde-89eb77eff898','The Notebook');
INSERT INTO public."Movie"  VALUES('4053b290-79aa-488f-90f3-1e687d888d27','The Wolf Of Wallstreet');
INSERT INTO public."Movie"  VALUES('2b0e7580-cdd0-4e13-80ef-548e9d611e81','Chronicles of Narnia');
INSERT INTO public."Movie"  VALUES('e0ee5538-fc83-46d2-8bdf-16609d4ae6dc','SING');
INSERT INTO public."Movie"  VALUES('fbf1d087-124b-4107-9322-c205f58ff262','Titanic');
INSERT INTO public."Movie"  VALUES('b057b489-a763-4e94-ad6c-111ddee229aa','Seven');
INSERT INTO public."Movie"  VALUES('62260611-a3ec-4ddd-b9ae-12e224886b5c','Shrek 2');
INSERT INTO public."Movie"  VALUES('efc3d10c-2317-45dc-8ce7-a054866dd3a5','Avatar 2: Avatar harder');
INSERT INTO public."Movie"  VALUES('b890481d-9217-43b3-9e4e-9996100ed82b','Avatar 3: A good day to Avatar');
INSERT INTO public."Movie"  VALUES('6227ee59-6d74-4db5-9076-dd345ebbf5f2','Love');
INSERT INTO public."Movie"  VALUES('3acb0b46-e464-4ae5-b006-513e65436d49','Allied');
INSERT INTO public."Movie"  VALUES('8cb83e29-b07e-4ea1-9dc9-773315277392','Terminator 3');
INSERT INTO public."Movie"  VALUES('06433fef-5286-4ea0-b998-1fa7a44f9342','Batman Begins');
INSERT INTO public."Movie"  VALUES('400b92b9-1a79-4d25-a543-dde12a12d723','Romeo and Juliet');
INSERT INTO public."Movie"  VALUES('c750730e-8235-4230-8ac2-639196e3ef55','Finding Dory 2: This time it''s personal');
INSERT INTO public."Movie"  VALUES('e1427cf9-0419-422d-bea4-4488ed39c423','Avatar 4: Avatar forever');
INSERT INTO public."Movie"  VALUES('10a7abb0-e28c-454b-b771-8e99a948c55f','Terminator 1');
INSERT INTO public."Movie"  VALUES('e52f199d-ecb3-420f-9115-5f7a98d4edd1','The Martian 2: Pluto or bust');
INSERT INTO public."Movie"  VALUES('2f4527a6-6913-4550-8e04-78b07380d3f5','Ice Age 2');
INSERT INTO public."Movie"  VALUES('79e03427-4c26-40af-936e-147f1cb651b7','Harry Potter and The Sorcerer''s Stone');
INSERT INTO public."Movie"  VALUES('f177fa5a-b08d-46e1-97d6-04da42b5d090','Shrek 3');
--rollback TRUNCATE TABLE public."Movie" CASCADE;