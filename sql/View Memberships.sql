SELECT * 
	FROM [dbo].[Membership]
	INNER JOIN [dbo].[webpages_Membership]
	ON [dbo].[Membership].Id = [webpages_Membership].UserId

GO


