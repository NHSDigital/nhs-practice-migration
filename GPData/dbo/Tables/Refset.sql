CREATE TABLE [Refset](
                         [Output_ID] [nvarchar](50) NOT NULL,
                         [Cluster_ID] [nvarchar](50) NOT NULL,
                         [Cluster_Description] [nvarchar](255) NOT NULL,
                         [SNOMED_code] [bigint] NULL,
                         [SNOMED_code_description] [nvarchar](300) NOT NULL,
                         [PCD_Refset_ID] [bigint] NOT NULL
)
GO
CREATE CLUSTERED INDEX [ClusteredIndex-20240304-122812] ON [Refset]
    (
     [SNOMED_code] ASC,
     [PCD_Refset_ID] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
