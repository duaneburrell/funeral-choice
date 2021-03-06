/****** Object:  StoredProcedure [dbo].[sp_InitializeWebPagesRoles]    Script Date: 8/16/2013 9:07:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ralph Hale
-- Create date: 7/15/2013
-- Description:	Creates Roles
-- =============================================
CREATE PROCEDURE [dbo].[sp_InitializeWebPagesRoles] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Delete all existing rows
	DELETE FROM webpages_Roles

	-- Reset Id 
	-- DBCC CHECKIDENT (webpages_Roles, RESEED, 1)

    -- Insert statements for procedure here	
	INSERT INTO webpages_Roles VALUES ('Elevated')
	INSERT INTO webpages_Roles VALUES ('Standard')

	INSERT INTO webpages_Roles VALUES ('Administrator')
	INSERT INTO webpages_Roles VALUES ('Consumer')
	INSERT INTO webpages_Roles VALUES ('Provider')

END



GO
/****** Object:  Table [dbo].[Membership]    Script Date: 8/16/2013 9:07:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Membership](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[MongoUserId] [nvarchar](48) NOT NULL,
 CONSTRAINT [PK_Membership] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[webpages_Membership]    Script Date: 8/16/2013 9:07:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Membership](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[ConfirmationToken] [nvarchar](128) NULL,
	[IsConfirmed] [bit] NULL,
	[LastPasswordFailureDate] [datetime] NULL,
	[PasswordFailuresSinceLastSuccess] [int] NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordChangedDate] [datetime] NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
	[PasswordVerificationToken] [nvarchar](128) NULL,
	[PasswordVerificationTokenExpirationDate] [datetime] NULL,
 CONSTRAINT [PK_webpages_Membership] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[webpages_Roles]    Script Date: 8/16/2013 9:07:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_webpages_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
/****** Object:  Table [dbo].[webpages_UsersInRoles]    Script Date: 8/16/2013 9:07:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[webpages_UsersInRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_webpages_UsersInRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
ALTER TABLE [dbo].[webpages_Membership]  WITH CHECK ADD  CONSTRAINT [FK_webpages_Membership_Membership1] FOREIGN KEY([UserId])
REFERENCES [dbo].[Membership] ([Id])
GO
ALTER TABLE [dbo].[webpages_Membership] CHECK CONSTRAINT [FK_webpages_Membership_Membership1]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK_webpages_UsersInRoles_Membership] FOREIGN KEY([UserId])
REFERENCES [dbo].[Membership] ([Id])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [FK_webpages_UsersInRoles_Membership]
GO
ALTER TABLE [dbo].[webpages_UsersInRoles]  WITH CHECK ADD  CONSTRAINT [FK_webpages_UsersInRoles_webpages_Roles] FOREIGN KEY([RoleId])
REFERENCES [dbo].[webpages_Roles] ([RoleId])
GO
ALTER TABLE [dbo].[webpages_UsersInRoles] CHECK CONSTRAINT [FK_webpages_UsersInRoles_webpages_Roles]
GO
