USE [PCPartsDB]
GO

/****** Object:  Table [dbo].[CreditCards]    Script Date: 2/11/2022 9:51:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[CreditCards](
	[CreditCardId] [int] IDENTITY(1,1) NOT NULL,
	[CreditCardNumber] [nvarchar](16) NOT NULL,
	[NameOnCard] [nvarchar](200) NOT NULL,
	[ExpirationDate] [nchar](5) NOT NULL,
	[CCV] [nvarchar](4) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CreditCardId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


