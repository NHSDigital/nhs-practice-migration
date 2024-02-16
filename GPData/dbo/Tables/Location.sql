﻿CREATE TABLE [dbo].[Location] (
    [Id]                     UNIQUEIDENTIFIER NOT NULL,
    [ODSSiteCodeID]          NVARCHAR (50)    NULL,
    [Status]                 NVARCHAR (50)    NULL,
    [OperationalStatus]      NVARCHAR (50)    NULL,
    [Name]                   NVARCHAR (255)   NULL,
    [Alias]                  NVARCHAR (255)   NULL,
    [Description]            NVARCHAR (MAX)   NULL,
    [Type]                   NVARCHAR (50)    NULL,
    [Telecom]                NVARCHAR (20)    NULL,
    [AddressID]              UNIQUEIDENTIFIER NULL,
    [PhysicalType]           NVARCHAR (50)    NULL,
    [Longitude]              DECIMAL (9, 6)   NULL,
    [Latitude]               DECIMAL (9, 6)   NULL,
    [Altitude]               DECIMAL (9, 6)   NULL,
    [ManagingOrganizationID] UNIQUEIDENTIFIER NULL,
    [PartOfID]               UNIQUEIDENTIFIER NULL,
    [EntityId]               UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK__Location__3214EC073C4874F8] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK__Location__Addres__4F47C5E3] FOREIGN KEY ([AddressID]) REFERENCES [dbo].[Address] ([Id]),
    CONSTRAINT [FK__Location__Managi__4E53A1AA] FOREIGN KEY ([ManagingOrganizationID]) REFERENCES [dbo].[Organization] ([Id]),
    CONSTRAINT [FK__Location__PartOf__4D5F7D71] FOREIGN KEY ([PartOfID]) REFERENCES [dbo].[Location] ([Id])
);

