USE [master]
GO

---------- CREATE DATABASE ----------


if exists(select * from sys.databases where name='MassDataHandlerTest') 
BEGIN
  alter database MassDataHandlerTest set single_user with ROLLBACK IMMEDIATE;
  DROP DATABASE [MassDataHandlerTest]
END

CREATE DATABASE [MassDataHandlerTest] ON  PRIMARY 
( NAME = N'MassDataHandlerTest', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\MassDataHandlerTest.mdf' , SIZE = 2048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MassDataHandlerTest_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\MassDataHandlerTest_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 COLLATE SQL_Latin1_General_CP1_CI_AS
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'MassDataHandlerTest', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MassDataHandlerTest].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [MassDataHandlerTest] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MassDataHandlerTest] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MassDataHandlerTest] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MassDataHandlerTest] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MassDataHandlerTest] SET ARITHABORT OFF 
GO
ALTER DATABASE [MassDataHandlerTest] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MassDataHandlerTest] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [MassDataHandlerTest] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MassDataHandlerTest] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MassDataHandlerTest] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MassDataHandlerTest] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MassDataHandlerTest] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MassDataHandlerTest] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MassDataHandlerTest] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MassDataHandlerTest] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MassDataHandlerTest] SET  ENABLE_BROKER 
GO
ALTER DATABASE [MassDataHandlerTest] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MassDataHandlerTest] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MassDataHandlerTest] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MassDataHandlerTest] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MassDataHandlerTest] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MassDataHandlerTest] SET  READ_WRITE 
GO
ALTER DATABASE [MassDataHandlerTest] SET RECOVERY FULL 
GO
ALTER DATABASE [MassDataHandlerTest] SET  MULTI_USER 
GO
ALTER DATABASE [MassDataHandlerTest] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MassDataHandlerTest] SET DB_CHAINING OFF 


---------- CREATE USER ----------
USE [MassDataHandlerTest];

BEGIN TRY
  DROP USER [TestUser]
END TRY
BEGIN CATCH
  --Do nothing
END CATCH
print 'DROP USER [TestUser]'

BEGIN TRY
  DROP LOGIN [TestUser2];
END TRY
BEGIN CATCH
  --Do nothing
END CATCH
print 'DROP LOGIN [TestUser2]'

GO

CREATE LOGIN [TestUser2] 
  WITH PASSWORD = 'MyPassword', 
  DEFAULT_DATABASE=[MassDataHandlerTest];
print 'CREATE LOGIN'

USE [MassDataHandlerTest];
CREATE USER TestUser2 FOR LOGIN TestUser2;
print 'CREATE USER'

GO

--Need to ensure that login has access to these roles so that it
-- can set IDENTITY_INSERT if user uses identity-insert strategy.
use [MassDataHandlerTest]
go
exec sp_addrolemember 'db_owner','TestUser2'
exec sp_addrolemember 'db_ddladmin','TestUser2'



---------- Insert objects ----------
USE [MassDataHandlerTest]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Customer](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerName] [varchar](50) NOT NULL,
	[Notes] [varchar](400) NULL,
	[LastUpdate] [datetime] NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY NONCLUSTERED 
(
	[CustomerId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
GRANT DELETE ON [dbo].[Customer] TO [TestUser2]
GO
GRANT INSERT ON [dbo].[Customer] TO [TestUser2]
GO
GRANT SELECT ON [dbo].[Customer] TO [TestUser2]
GO
GRANT UPDATE ON [dbo].[Customer] TO [TestUser2]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Product]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [varchar](50) NOT NULL,
	[Description] [varchar](400) NULL,
	[LastUpdate] [datetime] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
GRANT DELETE ON [dbo].[Product] TO [TestUser2]
GO
GRANT INSERT ON [dbo].[Product] TO [TestUser2]
GO
GRANT SELECT ON [dbo].[Product] TO [TestUser2]
GO
GRANT UPDATE ON [dbo].[Product] TO [TestUser2]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Order]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Order](
	[CustomerId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[OrderName] [int] NOT NULL,
	[LastUpdate] [nchar](10) NOT NULL
) ON [PRIMARY]
END
GO
GRANT DELETE ON [dbo].[Order] TO [TestUser2]
GO
GRANT INSERT ON [dbo].[Order] TO [TestUser2]
GO
GRANT SELECT ON [dbo].[Order] TO [TestUser2]
GO
GRANT UPDATE ON [dbo].[Order] TO [TestUser2]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Insert Audit table with Trigger
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AuditTrail]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AuditTrail](
	[AuditId] [int] IDENTITY(1,1) NOT NULL,
	[Data] [varchar](50) NULL,
	[LastUpdate] [datetime] NOT NULL,
 CONSTRAINT [PK_AuditTrail] PRIMARY KEY NONCLUSTERED 
(
	[AuditId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
GRANT DELETE ON [dbo].[AuditTrail] TO [TestUser2]
GO
GRANT INSERT ON [dbo].[AuditTrail] TO [TestUser2]
GO
GRANT SELECT ON [dbo].[AuditTrail] TO [TestUser2]
GO
GRANT UPDATE ON [dbo].[AuditTrail] TO [TestUser2]
GO

--Trigger:
IF OBJECT_ID ('[dbo].[TR_AuditTrail]','TR') IS NOT NULL
   DROP TRIGGER dbo.TR_AuditTrail
GO
CREATE TRIGGER TR_AuditTrail ON dbo.Customer
AFTER INSERT
AS
--TODO: get value from table that was just inserted
--Declare @sCustomerName varchar(50)
--select @sCustomerName =  from Customer
insert into AuditTrail (Data,LastUpdate)
values ('xyz', GetDate() ) 



--=============== FOREIGN KEYS ===================

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Order_Customer]') AND parent_object_id = OBJECT_ID(N'[dbo].[Order]'))
  ALTER TABLE [dbo].[Order]  DROP CONSTRAINT [FK_Order_Customer]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO

----
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Order_Product]') AND parent_object_id = OBJECT_ID(N'[dbo].[Order]'))
  ALTER TABLE [dbo].[Order]  DROP CONSTRAINT [FK_Order_Product]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO

----
IF EXISTS (SELECT 1 from sysindexes where name = 'UX1_Customer')
	DROP INDEX [Customer].[UX1_Customer]
GO
CREATE UNIQUE CLUSTERED
  INDEX UX1_Customer ON [Customer] ([CustomerName])
GO


--=============== STORED PROCEDURES ===================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer_Save]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Customer_Save] 
	@CustomerName varchar(50),
	@Notes varchar(400)
AS
BEGIN
	SET NOCOUNT ON;

  insert into Customer (CustomerName, Notes, LastUpdate)
  values (@CustomerName, @Notes, GetDate() )

END
' 
END
GO
GRANT EXECUTE ON [dbo].[Customer_Save] TO [TestUser2]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer_Get]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Customer_Get]
	@CustomerId int
AS
BEGIN
  select * from Customer where CustomerId = @CustomerId
END
' 
END
GO
GRANT EXECUTE ON [dbo].[Customer_Get] TO [TestUser2]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer_Get_Orders]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Customer_Get_Orders]
	@CustomerId int
AS
BEGIN
  select * from [Order] where CustomerId = @CustomerId
END
' 
END
GO
GRANT EXECUTE ON [dbo].[Customer_Get_Orders] TO [TestUser2]
GO




