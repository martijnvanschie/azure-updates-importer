CREATE TABLE [dbo].[Categories]
(
  [Id] INT IDENTITY(0,1) NOT NULL,
  [Title] NVARCHAR(250) NOT NULL,
  [DateCreated]  DATETIMEOFFSET DEFAULT (getutcdate()) NOT NULL,
  [DateModified] DATETIMEOFFSET DEFAULT (getutcdate()) NOT NULL,
  CONSTRAINT "PK_Categories" PRIMARY KEY (Id)
)
