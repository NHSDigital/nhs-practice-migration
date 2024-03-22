﻿CREATE TABLE [dbo].[ProcedureRequest] (
    [Id]                         UNIQUEIDENTIFIER NOT NULL,
    [OriginalId]                 NVARCHAR (255)   NULL,
    [Intent]                     INT              NOT NULL,
    [Identifier]                 UNIQUEIDENTIFIER NULL,
    [Status]                     NVARCHAR (255)   NULL,
    [AuthoredOn]                 DATETIME         NULL,
    [Occurence]                  DATETIME         NULL,
    [Category]                   NVARCHAR (255)   NULL,
    [Code]                       UNIQUEIDENTIFIER NULL,
    [Subject]                    UNIQUEIDENTIFIER NULL,
    [Context]                    UNIQUEIDENTIFIER NULL,
    [Encounter]                  UNIQUEIDENTIFIER NULL,
    [Requester]                  UNIQUEIDENTIFIER NULL,
    [OnBehalfOf]                 UNIQUEIDENTIFIER NULL,
    [Performer]                  UNIQUEIDENTIFIER NULL,
    [Reason]                     NVARCHAR (255)   NULL,
    [ReasonReference]            UNIQUEIDENTIFIER NULL,
    [SupportingInfo]             UNIQUEIDENTIFIER NULL,
    [Specimen]                   UNIQUEIDENTIFIER NULL,
    [BodySite]                   NVARCHAR (255)   NULL,
    [NoteText]                   NVARCHAR (MAX)   NULL,
    [NoteAuthored]               DATETIME         NULL,
    [NoteAuthor]                 UNIQUEIDENTIFIER NULL,
    [EntityId]                   UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ProcedureRequest_Context] FOREIGN KEY ([Context]) REFERENCES [dbo].[Entity] ([Id]),
    CONSTRAINT [FK_ProcedureRequest_Code] FOREIGN KEY ([Code]) REFERENCES [dbo].[Coding] ([Id]),
    CONSTRAINT [FK_ProcedureRequest_Subject] FOREIGN KEY ([Subject]) REFERENCES [dbo].[Patient] ([Id]),
    CONSTRAINT [FK_ProcedureRequest_Encounter] FOREIGN KEY ([Encounter]) REFERENCES [dbo].[Encounter] ([Id]),
    CONSTRAINT [FK_ProcedureRequest_Identifier] FOREIGN KEY ([Requester]) REFERENCES [dbo].[Entity] ([Id]),
    CONSTRAINT [FK_ProcedureRequest_OnBehalfOf] FOREIGN KEY ([OnBehalfOf]) REFERENCES [dbo].[Organization] ([Id]),
    CONSTRAINT [FK_ProcedureRequest_RPerformer] FOREIGN KEY ([Performer]) REFERENCES [dbo].[Entity] ([Id]),
    CONSTRAINT [FK_ProcedureRequest_RequestingOrganization] FOREIGN KEY ([ReasonReference]) REFERENCES [dbo].[Entity] ([Id]),
    CONSTRAINT [FK_ProcedureRequest_ReasonReference] FOREIGN KEY ([SupportingInfo]) REFERENCES [dbo].[Observation] ([Id]),
    CONSTRAINT [FK_ProcedureRequest_Specimen] FOREIGN KEY ([Specimen]) REFERENCES [dbo].[Specimen] ([Id]),
    CONSTRAINT [FK_ProcedureRequest_NoteAuthor] FOREIGN KEY ([NoteAuthor]) REFERENCES [dbo].[Entity] ([Id]),
    CONSTRAINT [FK_ProcedureRequest_Entity] FOREIGN KEY ([EntityId]) REFERENCES [dbo].[Entity] ([Id])
);

