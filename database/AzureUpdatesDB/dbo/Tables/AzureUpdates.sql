CREATE TABLE [dbo].[AzureUpdates]
(
  [Id]  NVARCHAR (250) NOT NULL,
  [Title]  NVARCHAR (250) NOT NULL,
  [Description]  NVARCHAR (MAX) NOT NULL,
  [Url] VARCHAR(MAX) NULL,
  [DatePublished] DATETIMEOFFSET NOT NULL,
  [DateCreated]  DATETIMEOFFSET DEFAULT (getutcdate()) NOT NULL,
  [DateModified] DATETIMEOFFSET DEFAULT (getutcdate()) NOT NULL,
  CONSTRAINT "PK_AzureFeeds" PRIMARY KEY (Id)
)
