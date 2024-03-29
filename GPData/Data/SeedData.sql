BEGIN
	-- Disable constraints for all tables:
	EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT all'
    DELETE FROM [dbo].[EntityType]
	INSERT [dbo].[EntityType] ([EntityType], [EntityName]) VALUES (1,N'Organization')
	INSERT [dbo].[EntityType] ([EntityType], [EntityName]) VALUES (2,N'Location')
	INSERT [dbo].[EntityType] ([EntityType], [EntityName]) VALUES (3,N'Patient')
	INSERT [dbo].[EntityType] ([EntityType], [EntityName]) VALUES (4,N'Practitioner')
	INSERT [dbo].[EntityType] ([EntityType], [EntityName]) VALUES (5,N'Observation')
	INSERT [dbo].[EntityType] ([EntityType], [EntityName]) VALUES (6,N'Condition')
    INSERT [dbo].[EntityType] ([EntityType], [EntityName]) VALUES (7,N'DiaryEntry')
    INSERT [dbo].[EntityType] ([EntityType], [EntityName]) VALUES (8,N'ProcedureRequest')
	-- Re-enable constraints for all tables:
	EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all';	
END 