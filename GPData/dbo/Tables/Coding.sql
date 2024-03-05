CREATE TABLE [dbo].[Coding] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [ReadCode]     VARCHAR (250)    NULL,
    [SnomedCode]   BIGINT           NULL,
    [LocalCode]    VARCHAR (250)    NULL,
    [NationalCode] VARCHAR (250)    NULL,
    [Description]  VARCHAR (250)    NULL,
    CONSTRAINT [PK_Coding] PRIMARY KEY CLUSTERED ([Id] ASC),
);

