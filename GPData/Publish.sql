﻿/*
Deployment script for GPData

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar RunAutomatedScripts "false"
:setvar DatabaseName "GPData"
:setvar DefaultFilePrefix "GPData"
:setvar DefaultDataPath "/var/opt/mssql/data/"
:setvar DefaultLogPath "/var/opt/mssql/data/"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO

IF (DB_ID(N'$(DatabaseName)') IS NOT NULL)
BEGIN
    DECLARE @rc      int,                       -- return code
            @fn      nvarchar(4000),            -- file name for back up
            @dir     nvarchar(4000)             -- backup directory

    EXEC @rc = [master].[dbo].[xp_instance_regread] N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer', N'BackupDirectory', @dir output, 'no_output'
    if (@rc = 0) SELECT @dir = @dir + N'\'

    IF (@dir IS NULL)
    BEGIN 
        EXEC @rc = [master].[dbo].[xp_instance_regread] N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\MSSQLServer', N'DefaultData', @dir output, 'no_output'
        if (@rc = 0) SELECT @dir = @dir + N'\'
    END

    IF (@dir IS NULL)
    BEGIN
        EXEC @rc = [master].[dbo].[xp_instance_regread] N'HKEY_LOCAL_MACHINE', N'Software\Microsoft\MSSQLServer\Setup', N'SQLDataRoot', @dir output, 'no_output'
        if (@rc = 0) SELECT @dir = @dir + N'\Backup\'
    END

    IF (@dir IS NULL)
    BEGIN
        SELECT @dir = N'$(DefaultDataPath)'
    END

    SELECT  @fn = @dir + N'$(DatabaseName)' + N'-' + 
            CONVERT(nchar(8), GETDATE(), 112) + N'-' + 
            RIGHT(N'0' + RTRIM(CONVERT(nchar(2), DATEPART(hh, GETDATE()))), 2) + 
            RIGHT(N'0' + RTRIM(CONVERT(nchar(2), DATEPART(mi, getdate()))), 2) + 
            RIGHT(N'0' + RTRIM(CONVERT(nchar(2), DATEPART(ss, getdate()))), 2) + 
            N'.bak' 
            BACKUP DATABASE [$(DatabaseName)] TO DISK = @fn
END
GO
USE [$(DatabaseName)];


GO
PRINT N'Dropping SqlForeignKeyConstraint unnamed constraint on [dbo].[Observation]...';


GO
ALTER TABLE [dbo].[Observation] DROP CONSTRAINT [FK__Observati__Perfo__5AB9788F];


GO
PRINT N'Dropping SqlForeignKeyConstraint unnamed constraint on [dbo].[DiagnosticReport]...';


GO
ALTER TABLE [dbo].[DiagnosticReport] DROP CONSTRAINT [FK__Diagnosti__Resul__3493CFA7];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK__Immunizat__React__74AE54BC]...';


GO
ALTER TABLE [dbo].[Immunization] DROP CONSTRAINT [FK__Immunizat__React__74AE54BC];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK__Observati__Subje__40F9A68C]...';


GO
ALTER TABLE [dbo].[Observation] DROP CONSTRAINT [FK__Observati__Subje__40F9A68C];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK__Observati__Conte__534D60F1]...';


GO
ALTER TABLE [dbo].[Observation] DROP CONSTRAINT [FK__Observati__Conte__534D60F1];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK_Patient_Address_Patient]...';


GO
ALTER TABLE [dbo].[Patient_Address] DROP CONSTRAINT [FK_Patient_Address_Patient];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK__Immunizat__Subje__6FE99F9F]...';


GO
ALTER TABLE [dbo].[Immunization] DROP CONSTRAINT [FK__Immunizat__Subje__6FE99F9F];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK__Medicatio__Patie__3E1D39E1]...';


GO
ALTER TABLE [dbo].[MedicationStatement] DROP CONSTRAINT [FK__Medicatio__Patie__3E1D39E1];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK_Compositio_Patient]...';


GO
ALTER TABLE [dbo].[Composition] DROP CONSTRAINT [FK_Compositio_Patient];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK_ReferralRequest_Patient]...';


GO
ALTER TABLE [dbo].[ReferralRequest] DROP CONSTRAINT [FK_ReferralRequest_Patient];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK__Specimen__Subjec__6FB49575]...';


GO
ALTER TABLE [dbo].[Specimen] DROP CONSTRAINT [FK__Specimen__Subjec__6FB49575];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK__Encounter__Patie__412EB0B6]...';


GO
ALTER TABLE [dbo].[Encounter] DROP CONSTRAINT [FK__Encounter__Patie__412EB0B6];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK__AllergyIn__NoteA__31B762FC]...';


GO
ALTER TABLE [dbo].[AllergyIntollerance] DROP CONSTRAINT [FK__AllergyIn__NoteA__31B762FC];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK_Patient_Links_Patient]...';


GO
ALTER TABLE [dbo].[Patient_Links] DROP CONSTRAINT [FK_Patient_Links_Patient];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK_Patient_Practicioner]...';


GO
ALTER TABLE [dbo].[Patient] DROP CONSTRAINT [FK_Patient_Practicioner];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK_Patient_Organization1]...';


GO
ALTER TABLE [dbo].[Patient] DROP CONSTRAINT [FK_Patient_Organization1];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK_Patient_Links_Patient1]...';


GO
ALTER TABLE [dbo].[Patient_Links] DROP CONSTRAINT [FK_Patient_Links_Patient1];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK__AllergyIn__Subje__2CF2ADDF]...';


GO
ALTER TABLE [dbo].[AllergyIntollerance] DROP CONSTRAINT [FK__AllergyIn__Subje__2CF2ADDF];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK_Patient_Patient]...';


GO
ALTER TABLE [dbo].[Patient] DROP CONSTRAINT [FK_Patient_Patient];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK__Medicatio__Patie__3A4CA8FD]...';


GO
ALTER TABLE [dbo].[MedicationRequest] DROP CONSTRAINT [FK__Medicatio__Patie__3A4CA8FD];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK__Diagnosti__Subje__1EA48E88]...';


GO
ALTER TABLE [dbo].[DiagnosticReport] DROP CONSTRAINT [FK__Diagnosti__Subje__1EA48E88];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK_ProcedureRequest_Subject]...';


GO
ALTER TABLE [dbo].[ProcedureRequest] DROP CONSTRAINT [FK_ProcedureRequest_Subject];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK_Patient_Organization]...';


GO
ALTER TABLE [dbo].[Patient] DROP CONSTRAINT [FK_Patient_Organization];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK_Patient_Contacts_Patient]...';


GO
ALTER TABLE [dbo].[Patient_Contacts] DROP CONSTRAINT [FK_Patient_Contacts_Patient];


GO
PRINT N'Dropping SqlForeignKeyConstraint [dbo].[FK__Condition__Subje__40F9A68C]...';


GO
ALTER TABLE [dbo].[Condition] DROP CONSTRAINT [FK__Condition__Subje__40F9A68C];


GO
PRINT N'Starting rebuilding table [dbo].[Observation]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Observation] (
    [Id]                      UNIQUEIDENTIFIER NOT NULL,
    [OriginalId]              NVARCHAR (200)   NULL,
    [Status]                  NVARCHAR (MAX)   NULL,
    [Category]                NVARCHAR (MAX)   NULL,
    [Code]                    NVARCHAR (MAX)   NULL,
    [SubjectId]               UNIQUEIDENTIFIER NULL,
    [ContextId]               UNIQUEIDENTIFIER NULL,
    [EffectiveDate]           DATETIME         NULL,
    [EffectiveDateFrom]       DATETIME         NULL,
    [EffectiveDateTo]         DATETIME         NULL,
    [Issued]                  DATETIME         NULL,
    [PerformerId]             UNIQUEIDENTIFIER NULL,
    [Interpretation]          NVARCHAR (MAX)   NULL,
    [DataAbsentReason]        NVARCHAR (MAX)   NULL,
    [Comment]                 NVARCHAR (MAX)   NULL,
    [BodySite]                NVARCHAR (MAX)   NULL,
    [Method]                  NVARCHAR (MAX)   NULL,
    [Device]                  UNIQUEIDENTIFIER NULL,
    [ReferenceRangeLow]       INT              NULL,
    [ReferenceRangeHigh]      INT              NULL,
    [ReferenceRangeType]      NVARCHAR (MAX)   NULL,
    [ReferenceRangeAppliesTo] NVARCHAR (MAX)   NULL,
    [ReferenceRangeAge]       NVARCHAR (MAX)   NULL,
    [RelatedTo]               UNIQUEIDENTIFIER NULL,
    [Entityid]                UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Observation])
    BEGIN
        INSERT INTO [dbo].[tmp_ms_xx_Observation] ([Id], [Status], [Category], [Code], [SubjectId], [ContextId], [EffectiveDate], [EffectiveDateFrom], [EffectiveDateTo], [Issued], [PerformerId])
        SELECT   [Id],
                 [Status],
                 [Category],
                 [Code],
                 [SubjectId],
                 [ContextId],
                 [EffectiveDate],
                 [EffectiveDateFrom],
                 [EffectiveDateTo],
                 [Issued],
                 [PerformerId]
        FROM     [dbo].[Observation]
        ORDER BY [Id] ASC;
    END

DROP TABLE [dbo].[Observation];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Observation]', N'Observation';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
/*
The column [dbo].[Patient].[OriginalId] on table [dbo].[Patient] must be added, but the column has no default value and does not allow NULL values. If the table contains data, the ALTER script will not work. To avoid this issue you must either: add a default value to the column, mark it as allowing NULL values, or enable the generation of smart-defaults as a deployment option.
*/
GO
PRINT N'Starting rebuilding table [dbo].[Patient]...';


GO
BEGIN TRANSACTION;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

CREATE TABLE [dbo].[tmp_ms_xx_Patient] (
    [Id]                       UNIQUEIDENTIFIER NOT NULL,
    [OriginalId]               NVARCHAR (255)   NOT NULL,
    [ManagingOrganization]     UNIQUEIDENTIFIER NOT NULL,
    [Practicioner]             UNIQUEIDENTIFIER NOT NULL,
    [HomeAddress]              UNIQUEIDENTIFIER NULL,
    [Sex]                      NVARCHAR (255)   NULL,
    [DateOfBirthUTC]           DATETIME         NOT NULL,
    [DateOfDeathUTC]           DATETIME         NULL,
    [Title]                    NVARCHAR (255)   NULL,
    [GivenName]                NVARCHAR (255)   NOT NULL,
    [MiddleNames]              NVARCHAR (MAX)   NULL,
    [Surname]                  NVARCHAR (255)   NOT NULL,
    [DateOfRegistrationUTC]    DATETIME         NULL,
    [NhsNumber]                NVARCHAR (255)   NULL,
    [PatientNumber]            NVARCHAR (255)   NULL,
    [PatientTypeDescription]   NVARCHAR (255)   NULL,
    [DummyType]                NVARCHAR (255)   NULL,
    [ResidentialInstituteCode] NVARCHAR (255)   NULL,
    [NHSNumberStatus]          NVARCHAR (255)   NULL,
    [CarerName]                NVARCHAR (255)   NULL,
    [CarerRelation]            NVARCHAR (255)   NULL,
    [PersonGuid]               NVARCHAR (255)   NULL,
    [DateOfDeactivation]       DATETIME         NULL,
    [Deleted]                  BIT              NULL,
    [Active]                   BIT              NULL,
    [SpineSensitive]           BIT              NULL,
    [IsConfidential]           BIT              NULL,
    [EmailAddress]             NVARCHAR (255)   NULL,
    [HomePhone]                NVARCHAR (20)    NULL,
    [MobilePhone]              NVARCHAR (20)    NULL,
    [ProcessingId]             NVARCHAR (255)   NULL,
    [Ethnicity]                NVARCHAR (255)   NULL,
    [Religion]                 NVARCHAR (255)   NULL,
    [NominatedPharmacy]        UNIQUEIDENTIFIER NULL,
    [EntityId]                 UNIQUEIDENTIFIER NULL,
    CONSTRAINT [tmp_ms_xx_constraint_PK__Patient__DBA60F19AE6332BC1] PRIMARY KEY CLUSTERED ([Id] ASC)
);

IF EXISTS (SELECT TOP 1 1 
           FROM   [dbo].[Patient])
    BEGIN
        INSERT INTO [dbo].[tmp_ms_xx_Patient] ([Id], [ManagingOrganization], [Practicioner], [Sex], [DateOfBirthUTC], [DateOfDeathUTC], [Title], [GivenName], [MiddleNames], [Surname], [DateOfRegistrationUTC], [NhsNumber], [PatientNumber], [PatientTypeDescription], [DummyType], [ResidentialInstituteCode], [NHSNumberStatus], [CarerName], [CarerRelation], [PersonGuid], [DateOfDeactivation], [Deleted], [Active], [SpineSensitive], [IsConfidential], [EmailAddress], [HomePhone], [MobilePhone], [ProcessingId], [Ethnicity], [Religion], [NominatedPharmacy])
        SELECT   [Id],
                 [ManagingOrganization],
                 [Practicioner],
                 [Sex],
                 [DateOfBirthUTC],
                 [DateOfDeathUTC],
                 [Title],
                 [GivenName],
                 [MiddleNames],
                 [Surname],
                 [DateOfRegistrationUTC],
                 [NhsNumber],
                 [PatientNumber],
                 [PatientTypeDescription],
                 [DummyType],
                 [ResidentialInstituteCode],
                 [NHSNumberStatus],
                 [CarerName],
                 [CarerRelation],
                 [PersonGuid],
                 [DateOfDeactivation],
                 [Deleted],
                 [Active],
                 [SpineSensitive],
                 [IsConfidential],
                 [EmailAddress],
                 [HomePhone],
                 [MobilePhone],
                 [ProcessingId],
                 [Ethnicity],
                 [Religion],
                 [NominatedPharmacy]
        FROM     [dbo].[Patient]
        ORDER BY [Id] ASC;
    END

DROP TABLE [dbo].[Patient];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Patient]', N'Patient';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_constraint_PK__Patient__DBA60F19AE6332BC1]', N'PK__Patient__DBA60F19AE6332BC', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating SqlForeignKeyConstraint unnamed constraint on [dbo].[Observation]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Observation] WITH NOCHECK
    ADD FOREIGN KEY ([PerformerId]) REFERENCES [dbo].[Practicioner] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint unnamed constraint on [dbo].[DiagnosticReport]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[DiagnosticReport] WITH NOCHECK
    ADD FOREIGN KEY ([Result]) REFERENCES [dbo].[Observation] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK__Immunizat__React__74AE54BC]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Immunization] WITH NOCHECK
    ADD CONSTRAINT [FK__Immunizat__React__74AE54BC] FOREIGN KEY ([ReactionDetailId]) REFERENCES [dbo].[Observation] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK__Observati__Subje__40F9A68C]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Observation] WITH NOCHECK
    ADD CONSTRAINT [FK__Observati__Subje__40F9A68C] FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK__Observati__Conte__534D60F1]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Observation] WITH NOCHECK
    ADD CONSTRAINT [FK__Observati__Conte__534D60F1] FOREIGN KEY ([ContextId]) REFERENCES [dbo].[Encounter] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK__Observati__Device]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Observation] WITH NOCHECK
    ADD CONSTRAINT [FK__Observati__Device] FOREIGN KEY ([Device]) REFERENCES [dbo].[Device] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK__Observati__Entity]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Observation] WITH NOCHECK
    ADD CONSTRAINT [FK__Observati__Entity] FOREIGN KEY ([Entityid]) REFERENCES [dbo].[Entity] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK__Observati__Observation]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Observation] WITH NOCHECK
    ADD CONSTRAINT [FK__Observati__Observation] FOREIGN KEY ([RelatedTo]) REFERENCES [dbo].[Observation] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK__Observati__Perf]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Observation] WITH NOCHECK
    ADD CONSTRAINT [FK__Observati__Perf] FOREIGN KEY ([PerformerId]) REFERENCES [dbo].[Entity] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK_Patient_Address_Patient]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Patient_Address] WITH NOCHECK
    ADD CONSTRAINT [FK_Patient_Address_Patient] FOREIGN KEY ([PatientId]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK__Immunizat__Subje__6FE99F9F]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Immunization] WITH NOCHECK
    ADD CONSTRAINT [FK__Immunizat__Subje__6FE99F9F] FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK__Medicatio__Patie__3E1D39E1]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[MedicationStatement] WITH NOCHECK
    ADD CONSTRAINT [FK__Medicatio__Patie__3E1D39E1] FOREIGN KEY ([Subject]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK_Compositio_Patient]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Composition] WITH NOCHECK
    ADD CONSTRAINT [FK_Compositio_Patient] FOREIGN KEY ([Subject]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK_ReferralRequest_Patient]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[ReferralRequest] WITH NOCHECK
    ADD CONSTRAINT [FK_ReferralRequest_Patient] FOREIGN KEY ([Subject]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK__Specimen__Subjec__6FB49575]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Specimen] WITH NOCHECK
    ADD CONSTRAINT [FK__Specimen__Subjec__6FB49575] FOREIGN KEY ([SubjectId]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK__Encounter__Patie__412EB0B6]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Encounter] WITH NOCHECK
    ADD CONSTRAINT [FK__Encounter__Patie__412EB0B6] FOREIGN KEY ([PatientGuid]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK__AllergyIn__NoteA__31B762FC]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[AllergyIntollerance] WITH NOCHECK
    ADD CONSTRAINT [FK__AllergyIn__NoteA__31B762FC] FOREIGN KEY ([NoteAuthor]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK_Patient_Links_Patient]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Patient_Links] WITH NOCHECK
    ADD CONSTRAINT [FK_Patient_Links_Patient] FOREIGN KEY ([PatientId]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK_Patient_Practicioner]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Patient] WITH NOCHECK
    ADD CONSTRAINT [FK_Patient_Practicioner] FOREIGN KEY ([Practicioner]) REFERENCES [dbo].[Practicioner] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK_Patient_Organization1]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Patient] WITH NOCHECK
    ADD CONSTRAINT [FK_Patient_Organization1] FOREIGN KEY ([NominatedPharmacy]) REFERENCES [dbo].[Organization] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK_Patient_Links_Patient1]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Patient_Links] WITH NOCHECK
    ADD CONSTRAINT [FK_Patient_Links_Patient1] FOREIGN KEY ([LinkedPatient]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK__AllergyIn__Subje__2CF2ADDF]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[AllergyIntollerance] WITH NOCHECK
    ADD CONSTRAINT [FK__AllergyIn__Subje__2CF2ADDF] FOREIGN KEY ([Subject]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK_Patient_Patient]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Patient] WITH NOCHECK
    ADD CONSTRAINT [FK_Patient_Patient] FOREIGN KEY ([Id]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK__Medicatio__Patie__3A4CA8FD]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[MedicationRequest] WITH NOCHECK
    ADD CONSTRAINT [FK__Medicatio__Patie__3A4CA8FD] FOREIGN KEY ([Subject]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK__Diagnosti__Subje__1EA48E88]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[DiagnosticReport] WITH NOCHECK
    ADD CONSTRAINT [FK__Diagnosti__Subje__1EA48E88] FOREIGN KEY ([Subject]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK_ProcedureRequest_Subject]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[ProcedureRequest] WITH NOCHECK
    ADD CONSTRAINT [FK_ProcedureRequest_Subject] FOREIGN KEY ([Subject]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK_Patient_Organization]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Patient] WITH NOCHECK
    ADD CONSTRAINT [FK_Patient_Organization] FOREIGN KEY ([ManagingOrganization]) REFERENCES [dbo].[Organization] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK_Patient_Contacts_Patient]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Patient_Contacts] WITH NOCHECK
    ADD CONSTRAINT [FK_Patient_Contacts_Patient] FOREIGN KEY ([PatientId]) REFERENCES [dbo].[Patient] ([Id]);


GO
PRINT N'Creating SqlForeignKeyConstraint [dbo].[FK__Condition__Subje__40F9A68C]...';


GO
WAITFOR DELAY '00:00.010';

ALTER TABLE [dbo].[Condition] WITH NOCHECK
    ADD CONSTRAINT [FK__Condition__Subje__40F9A68C] FOREIGN KEY ([Subject]) REFERENCES [dbo].[Patient] ([Id]);


GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF NOT EXISTS (SELECT * FROM [dbo].[EntityType])
BEGIN
	-- Disable constraints for all tables:
	EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT all'

	INSERT [dbo].[EntityType] ([EntityType], [EntityName]) VALUES (1,N'Organization')
	INSERT [dbo].[EntityType] ([EntityType], [EntityName]) VALUES (2,N'Location')
	INSERT [dbo].[EntityType] ([EntityType], [EntityName]) VALUES (3,N'Patient')
	INSERT [dbo].[EntityType] ([EntityType], [EntityName]) VALUES (4,N'Practitioner')
	
	-- Re-enable constraints for all tables:
	EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all';	

END
GO

GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[Immunization] WITH CHECK CHECK CONSTRAINT [FK__Immunizat__React__74AE54BC];

ALTER TABLE [dbo].[Patient_Address] WITH CHECK CHECK CONSTRAINT [FK_Patient_Address_Patient];

ALTER TABLE [dbo].[Immunization] WITH CHECK CHECK CONSTRAINT [FK__Immunizat__Subje__6FE99F9F];

ALTER TABLE [dbo].[MedicationStatement] WITH CHECK CHECK CONSTRAINT [FK__Medicatio__Patie__3E1D39E1];

ALTER TABLE [dbo].[Composition] WITH CHECK CHECK CONSTRAINT [FK_Compositio_Patient];

ALTER TABLE [dbo].[ReferralRequest] WITH CHECK CHECK CONSTRAINT [FK_ReferralRequest_Patient];

ALTER TABLE [dbo].[Specimen] WITH CHECK CHECK CONSTRAINT [FK__Specimen__Subjec__6FB49575];

ALTER TABLE [dbo].[Encounter] WITH CHECK CHECK CONSTRAINT [FK__Encounter__Patie__412EB0B6];

ALTER TABLE [dbo].[AllergyIntollerance] WITH CHECK CHECK CONSTRAINT [FK__AllergyIn__NoteA__31B762FC];

ALTER TABLE [dbo].[Patient_Links] WITH CHECK CHECK CONSTRAINT [FK_Patient_Links_Patient];

ALTER TABLE [dbo].[Patient] WITH CHECK CHECK CONSTRAINT [FK_Patient_Practicioner];

ALTER TABLE [dbo].[Patient] WITH CHECK CHECK CONSTRAINT [FK_Patient_Organization1];

ALTER TABLE [dbo].[Patient_Links] WITH CHECK CHECK CONSTRAINT [FK_Patient_Links_Patient1];

ALTER TABLE [dbo].[AllergyIntollerance] WITH CHECK CHECK CONSTRAINT [FK__AllergyIn__Subje__2CF2ADDF];

ALTER TABLE [dbo].[Patient] WITH CHECK CHECK CONSTRAINT [FK_Patient_Patient];

ALTER TABLE [dbo].[MedicationRequest] WITH CHECK CHECK CONSTRAINT [FK__Medicatio__Patie__3A4CA8FD];

ALTER TABLE [dbo].[ProcedureRequest] WITH CHECK CHECK CONSTRAINT [FK_ProcedureRequest_Subject];

ALTER TABLE [dbo].[Patient] WITH CHECK CHECK CONSTRAINT [FK_Patient_Organization];

ALTER TABLE [dbo].[Patient_Contacts] WITH CHECK CHECK CONSTRAINT [FK_Patient_Contacts_Patient];

ALTER TABLE [dbo].[Condition] WITH CHECK CHECK CONSTRAINT [FK__Condition__Subje__40F9A68C];


GO
CREATE TABLE [#__checkStatus] (
    id           INT            IDENTITY (1, 1) PRIMARY KEY CLUSTERED,
    [Schema]     NVARCHAR (256),
    [Table]      NVARCHAR (256),
    [Constraint] NVARCHAR (256)
);

SET NOCOUNT ON;

DECLARE tableconstraintnames CURSOR LOCAL FORWARD_ONLY
    FOR SELECT SCHEMA_NAME([schema_id]),
               OBJECT_NAME([parent_object_id]),
               [name],
               0
        FROM   [sys].[objects]
        WHERE  [parent_object_id] IN (OBJECT_ID(N'dbo.Observation'), OBJECT_ID(N'dbo.DiagnosticReport'))
               AND [type] IN (N'F', N'C')
                   AND [object_id] IN (SELECT [object_id]
                                       FROM   [sys].[check_constraints]
                                       WHERE  [is_not_trusted] <> 0
                                              AND [is_disabled] = 0
                                       UNION
                                       SELECT [object_id]
                                       FROM   [sys].[foreign_keys]
                                       WHERE  [is_not_trusted] <> 0
                                              AND [is_disabled] = 0);

DECLARE @schemaname AS NVARCHAR (256);

DECLARE @tablename AS NVARCHAR (256);

DECLARE @checkname AS NVARCHAR (256);

DECLARE @is_not_trusted AS INT;

DECLARE @statement AS NVARCHAR (1024);

BEGIN TRY
    OPEN tableconstraintnames;
    FETCH tableconstraintnames INTO @schemaname, @tablename, @checkname, @is_not_trusted;
    WHILE @@fetch_status = 0
        BEGIN
            PRINT N'Checking constraint: ' + @checkname + N' [' + @schemaname + N'].[' + @tablename + N']';
            SET @statement = N'ALTER TABLE [' + @schemaname + N'].[' + @tablename + N'] WITH ' + CASE @is_not_trusted WHEN 0 THEN N'CHECK' ELSE N'NOCHECK' END + N' CHECK CONSTRAINT [' + @checkname + N']';
            BEGIN TRY
                EXECUTE [sp_executesql] @statement;
            END TRY
            BEGIN CATCH
                INSERT  [#__checkStatus] ([Schema], [Table], [Constraint])
                VALUES                  (@schemaname, @tablename, @checkname);
            END CATCH
            FETCH tableconstraintnames INTO @schemaname, @tablename, @checkname, @is_not_trusted;
        END
END TRY
BEGIN CATCH
    PRINT ERROR_MESSAGE();
END CATCH

IF CURSOR_STATUS(N'LOCAL', N'tableconstraintnames') >= 0
    CLOSE tableconstraintnames;

IF CURSOR_STATUS(N'LOCAL', N'tableconstraintnames') = -1
    DEALLOCATE tableconstraintnames;

SELECT N'Constraint verification failed:' + [Schema] + N'.' + [Table] + N',' + [Constraint]
FROM   [#__checkStatus];

IF @@ROWCOUNT > 0
    BEGIN
        DROP TABLE [#__checkStatus];
        RAISERROR (N'An error occurred while verifying constraints', 16, 127);
    END

SET NOCOUNT OFF;

DROP TABLE [#__checkStatus];


GO
PRINT N'Update complete.';


GO
