DELETE FROM [dbo].[webpages_UsersInRoles]
DELETE FROM [dbo].[webpages_Membership]
DELETE FROM [dbo].[webpages_Roles]
DELETE FROM [dbo].[Membership]

DECLARE @RC int

-- TODO: Set parameter values here.
EXECUTE @RC = [dbo].[sp_InitializeWebPagesRoles] 

GO


