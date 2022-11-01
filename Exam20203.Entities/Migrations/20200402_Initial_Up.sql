create table "TbUser"(
	"UserId" int Generated always as identity not null,
	"UserName" varchar(255) not null unique,
	"Password" varchar(100) not null,
	"UserRole" varchar(255) not null,
	"CreatedAt" TIMESTAMPTZ not null Default NOW(),
	"CreatedBy" varchar(255) not null default 'SYSTEM',
	"UpdatedAt" TIMESTAMPTZ not null Default NOW(),
	"UpdatedBy" varchar(255) not null default 'SYSTEM',
	Constraint "PK_TbUser" primary key ("UserId")
)

create table "TbMakanan"(
	"MakananId" Int Generated always as identity not null,
	"MakananName" varchar(255) not null,
	"MakananHarga" decimal(18,2) not null,
	"MakananStock" int not null,
	"CreatedAt" TIMESTAMPTZ not null Default NOW(),
	"CreatedBy" varchar(255) not null default 'SYSTEM',
	"UpdatedAt" TIMESTAMPTZ not null Default NOW(),
	"UpdatedBy" varchar(255) not null default 'SYSTEM',
	constraint "PK_TbMakanan" Primary key ("MakananId")
)

create table "TbTransaction"(
	"TransactionId" int generated always as identity not null,
	"MakananId" int not null,
	"UserId" int not null,
	"Quantity" int not null,
	"TransactionDate" TIMESTAMPTZ not null,
	"CreatedAt" TIMESTAMPTZ not null Default NOW(),
	"CreatedBy" varchar(255) not null default 'SYSTEM',
	"UpdatedAt" TIMESTAMPTZ not null Default NOW(),
	"UpdatedBy" varchar(255) not null default 'SYSTEM',
	constraint "Pk_TbTransaction" Primary key("TransactionId"),
	constraint "FK_TbMakanan" Foreign Key ("MakananId") References "TbMakanan"("MakananId"),
	constraint "FK_TbUser" Foreign Key ("UserId") References "TbUser"("UserId")
)
