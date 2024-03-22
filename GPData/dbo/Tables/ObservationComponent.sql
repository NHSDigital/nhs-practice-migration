CREATE TABLE dbo.[ObservationComponent]
(
    Id                      UNIQUEIDENTIFIER    NOT NULL,
    ObservationId           UNIQUEIDENTIFIER    NOT NULL,
    CodeId                  UNIQUEIDENTIFIER    NOT NULL,
    ValueQuantity           DECIMAL             NULL,
    ValueQuantityUnit       NVARCHAR(MAX)       NULL,
    ValueCode               NVARCHAR(MAX)       NULL,
    ValueString             NVARCHAR(MAX)       NULL,
    ValueRangeLow           INT                 NULL,
    ValueRangeHigh          INT                 NULL,
    ValueRatioNumerator     INT                 NULL,
    ValueRatioDenominator   INT                 NULL,
    SampledDataOrigin       INT                 NULL,
    SampledDataPeriod       DECIMAL             NULL,
    SampledDataFactor       DECIMAL             NULL,
    SampledDataLowerLimit   DECIMAL             NULL,
    SampledDataUpperLimit   DECIMAL             NULL,
    SampledData             NVARCHAR(MAX)       NULL,
    ValueTime               DATETIME            NULL,
    ValueDateTime           DATETIME            NULL,
    ValueDateTimeStart      DATETIME            NULL,
    ValueDateTimeEnd        DATETIME            NULL,
    DataAbsentReason        NVARCHAR(MAX)       NULL,

    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ObservationId]) REFERENCES [dbo].[Observation] ([Id]),
    CONSTRAINT [FK__Observati__Code] FOREIGN KEY ([CodeId]) REFERENCES [dbo].[Coding] ([Id])
);