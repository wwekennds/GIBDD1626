IF OBJECT_ID(N'dbo.Clients', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clients
    (
        Id uniqueidentifier NOT NULL CONSTRAINT DF_Clients_Id DEFAULT (newid()),
        Surname nvarchar(100) NOT NULL,
        Name nvarchar(100) NOT NULL,
        Patromic nvarchar(100) NOT NULL,
        Passport nvarchar(50) NOT NULL,
        Registration_city nvarchar(100) NOT NULL,
        Registration_street_address nvarchar(400) NOT NULL,
        Residence_city nvarchar(100) NOT NULL,
        Residence_street_address nvarchar(400) NOT NULL,
        Place_work nvarchar(200) NULL,
        Post nvarchar(200) NULL,
        Mobile_phone nvarchar(20) NOT NULL,
        Email nvarchar(255) NOT NULL,
        Photo varbinary(max) NOT NULL,
        Remarks nvarchar(max) NULL,
        CONSTRAINT PK_Clients PRIMARY KEY (Id)
    );
END;

IF OBJECT_ID(N'dbo.Vehicles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Vehicles
    (
        Id uniqueidentifier NOT NULL CONSTRAINT DF_Vehicles_Id DEFAULT (newid()),
        ClientId uniqueidentifier NOT NULL,
        Series nchar(3) NOT NULL,
        Number char(3) NOT NULL,
        RegionCode varchar(3) NOT NULL,
        Country char(3) NOT NULL CONSTRAINT DF_Vehicles_Country DEFAULT ('RUS'),
        Brand nvarchar(100) NULL,
        Model nvarchar(100) NULL,
        Vin varchar(17) NULL,
        CONSTRAINT PK_Vehicles PRIMARY KEY (Id),
        CONSTRAINT FK_Vehicles_Clients FOREIGN KEY (ClientId) REFERENCES dbo.Clients (Id)
    );
END;

IF OBJECT_ID(N'dbo.Fines', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Fines
    (
        Id uniqueidentifier NOT NULL CONSTRAINT DF_Fines_Id DEFAULT (newid()),
        VehicleId uniqueidentifier NOT NULL,
        ViolationDate datetime NOT NULL,
        ArticleCode nvarchar(50) NULL,
        FineAmount decimal(18, 2) NOT NULL,
        ExternalId nvarchar(100) NULL,
        CreatedAt datetime NULL CONSTRAINT DF_Fines_CreatedAt DEFAULT (getdate()),
        AccidentType nvarchar(200) NULL,
        Status nvarchar(50) NOT NULL CONSTRAINT DF_Fines_Status DEFAULT (N'Не оплачен'),
        CONSTRAINT PK_Fines PRIMARY KEY (Id),
        CONSTRAINT FK_Fines_Vehicles FOREIGN KEY (VehicleId) REFERENCES dbo.Vehicles (Id)
    );
END;
