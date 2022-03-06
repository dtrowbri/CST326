﻿USE [PCPartsDB]
GO

/****** Object:  Table [dbo].[Orders_X_OrderItems]    Script Date: 3/2/2022 9:24:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Orders_X_OrderItems](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[OrderItemId] [int] NOT NULL,
	[OrderId] [int] NOT NULL,
 CONSTRAINT [PK_Orders_X_OrderItems] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


