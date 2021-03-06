USE [master]
GO

IF DB_ID (N'Devices') IS NOT NULL
DROP DATABASE Devices;  /* BE CAREFULL NOT TO HAVE YOUR OWN DEVICES DATABASE OUT OF THE SCOPE OF THIS EXERCISE */
GO

CREATE DATABASE Devices;
GO

USE [Devices]
GO

CREATE TABLE [dbo].[EnergyMeter](
	[id] [uniqueidentifier] NOT NULL,
	[serialNumber] [nvarchar](50) NOT NULL,
	[brand] [nvarchar](50) NULL,
	[model] [nvarchar](50) NULL,
 CONSTRAINT [PK_EnergyMeter] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE UNIQUE NONCLUSTERED INDEX [IX_EnergyMeter_serialNumber] ON [dbo].[EnergyMeter]
(
	[serialNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO



CREATE TABLE [dbo].[Gateway](
	[id] [uniqueidentifier] NOT NULL,
	[serialNumber] [nvarchar](50) NOT NULL,
	[brand] [nvarchar](50) NULL,
	[model] [nvarchar](50) NULL,
	[ip] [nvarchar](15) NULL,
	[port] [int] NULL,
 CONSTRAINT [PK_Gateway] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Gateway_serialNumber] ON [dbo].[Gateway]
(
	[serialNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE TABLE [dbo].[WaterMeter](
	[id] [uniqueidentifier] NOT NULL,
	[serialNumber] [nvarchar](50) NOT NULL,
	[brand] [nvarchar](50) NULL,
	[model] [nvarchar](50) NULL,
 CONSTRAINT [PK_WaterMeter] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_WaterMeter_serialNumber] ON [dbo].[WaterMeter]
(
	[serialNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
