INSERT INTO public."TbMakanan"(
	"MakananName", "MakananHarga", "MakananStock")
	VALUES ('Ciki', 5000, 10);
	
INSERT INTO public."TbMakanan"(
	"MakananName", "MakananHarga", "MakananStock")
	VALUES ('Yupi', 2000, 12);

INSERT INTO "TbUser"(
	"UserName", "Password", "UserRole")
	VALUES ('Joel', '$2y$12$IUn1GZCzOjXF6gEioSFWYel2BoVExn1kji17GHpXXunB9iZCMHfMq', 'Admin');

INSERT INTO "TbUser"(
	"UserName", "Password", "UserRole")
	VALUES ('theo', '$2y$12$X0MHdVDS97fUTG5VlFjs9.VxgmDUC/GF/rwb5kiWo9mIn.Liam/Yu', 'User');
	

INSERT INTO public."TbTransaction"(
	"MakananId", "UserId", "Quantity", "TransactionDate")
	VALUES (1, 1, 5, '2020-04-2');
	
	INSERT INTO public."TbTransaction"(
	"MakananId", "UserId", "Quantity", "TransactionDate")
	VALUES (2, 2, 3, '2020-04-2');

INSERT INTO public."TbMakanan"(
	"MakananName", "MakananHarga", "MakananStock")
	VALUES ('Permen', 1000, 15);