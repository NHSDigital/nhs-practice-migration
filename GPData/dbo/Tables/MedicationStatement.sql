﻿CREATE TABLE [dbo].[MedicationStatement] (
    [Id]                       UNIQUEIDENTIFIER NOT NULL,
    [Status]                   NVARCHAR (MAX)   NULL,
    [PrescribingAgencyCode]    NVARCHAR (MAX)   NULL,
    [PrescribingAgencyDisplay] NVARCHAR (MAX)   NULL,
    [LastIssueDateUTC]         DATETIME         NULL,
    [DateAssertedUTC]          DATETIME         NULL,
    [DosageLastChanged]        DATETIME         NULL,
    [MedicationRequestId]      UNIQUEIDENTIFIER NULL,
    [CrossCareIdentifier]      UNIQUEIDENTIFIER NULL,
    [MedicationId]             UNIQUEIDENTIFIER NULL,
    [EffectivePeriodStart]     DATETIME         NULL,
    [EffectivePeriodEnd]       DATETIME         NULL,
    [PatientId]                UNIQUEIDENTIFIER NULL,
    [Taken]                    NVARCHAR (MAX)   NULL,
    [DosageRoute]              NVARCHAR (MAX)   NULL,
    [DosageMethod]             NVARCHAR (MAX)   NULL,
    [DosageRangeLow]           DECIMAL (18, 2)  NULL,
    [DosageRangeHigh]          DECIMAL (18, 2)  NULL,
    [MaxDosePerPeriod]         NVARCHAR (MAX)   NULL,
    [MaxDosePerAdministration] DECIMAL (18, 2)  NULL,
    [MaxDosePerLifeTime]       DECIMAL (18, 2)  NULL,
    [DosageText]               NVARCHAR (MAX)   NULL,
    [DosagePatientInstruction] NVARCHAR (MAX)   NULL,
    [DosageRateRatio]          NVARCHAR (MAX)   NULL,
    [DosageRateRange]          NVARCHAR (MAX)   NULL,
    [DosageRateQuantity]       NVARCHAR (MAX)   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([MedicationId]) REFERENCES [dbo].[Medication] ([Id]),
    FOREIGN KEY ([MedicationRequestId]) REFERENCES [dbo].[MedicationRequest] ([Id]),
    FOREIGN KEY ([PatientId]) REFERENCES [dbo].[Patient] ([Id])
);
