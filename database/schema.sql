IF NOT EXISTS (SELECT name
FROM sys.databases
WHERE name = 'EnhanzerAssignmentDb')
BEGIN
    CREATE DATABASE EnhanzerAssignmentDb;
END
GO

USE EnhanzerAssignmentDb;
GO

IF NOT EXISTS (SELECT *
FROM sys.tables
WHERE name = 'Location_Details')
BEGIN
    CREATE TABLE Location_Details
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        CompanyCode NVARCHAR(200) NOT NULL,
        Location_Code NVARCHAR(50) NOT NULL,
        Location_Name NVARCHAR(200) NOT NULL,
        CreatedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );

    CREATE INDEX IX_Location_Details_CompanyCode_LocationCode
        ON Location_Details (CompanyCode, Location_Code);
END
GO


IF NOT EXISTS (SELECT *
FROM sys.tables
WHERE name = 'PurchaseBillHeaders')
BEGIN
    CREATE TABLE PurchaseBillHeaders
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        CreatedByEmail NVARCHAR(200) NOT NULL,
        CreatedAtUtc DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
    );
END
GO


IF NOT EXISTS (SELECT *
FROM sys.tables
WHERE name = 'PurchaseBillLines')
BEGIN
    CREATE TABLE PurchaseBillLines
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        PurchaseBillHeaderId INT NOT NULL
            REFERENCES PurchaseBillHeaders(Id) ON DELETE CASCADE,
        Item NVARCHAR(100) NOT NULL,
        Batch NVARCHAR(200) NOT NULL,
        StandardCost DECIMAL(18,2) NOT NULL,
        StandardPrice DECIMAL(18,2) NOT NULL,
        Qty INT NOT NULL,
        FreeQty INT NOT NULL DEFAULT 0,
        Discount DECIMAL(5,2) NOT NULL DEFAULT 0,
        TotalCost DECIMAL(18,2) NOT NULL,
        TotalSelling DECIMAL(18,2) NOT NULL
    );
END
GO
