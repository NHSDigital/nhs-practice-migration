﻿CREATE TABLE [dbo].[StructureMap] (
    [Id]                  UNIQUEIDENTIFIER NOT NULL,
    [Identifier]          UNIQUEIDENTIFIER NULL,
    [Version]             VARCHAR (255)    NULL,
    [Name]                VARCHAR (255)    NULL,
    [Title]               VARCHAR (255)    NULL,
    [Status]              VARCHAR (255)    NULL,
    [Experimental]        BIT              NULL,
    [Date]                DATETIME         NULL,
    [Publisher]           VARCHAR (255)    NOT NULL,
    [ContactName]         VARCHAR (255)    NULL,
    [ContactNumber]       VARCHAR (255)    NULL,
    [Description]         VARCHAR (255)    NULL,
    [UseContextCode]      VARCHAR (255)    NULL,
    [UseContextQuantity]  DECIMAL (9, 2)   NULL,
    [UseContextRangeHigh] INT              NULL,
    [UseContextRangeLow]  INT              NULL,
    [Jurisdiction]        VARCHAR (255)    NULL,
    [Purpose]             VARCHAR (255)    NULL,
    [Copyright]           VARCHAR (255)    NULL,
    [Structure]           VARCHAR (255)    NULL,
    [Import]              VARCHAR (255)    NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StructureMap_Identifier] FOREIGN KEY ([Identifier]) REFERENCES [dbo].[Identifier] ([Id])
);

