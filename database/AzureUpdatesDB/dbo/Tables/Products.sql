CREATE TABLE [dbo].[Products]
(
  [Id] INT IDENTITY(0,1) NOT NULL,
  [Name] NVARCHAR(250) NOT NULL,
  [DateCreated] DATETIMEOFFSET DEFAULT (getutcdate()) NOT NULL,
  [DateModified] DATETIMEOFFSET DEFAULT (getutcdate()) NOT NULL,
  CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
)