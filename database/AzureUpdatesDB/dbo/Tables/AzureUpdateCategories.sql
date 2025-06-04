CREATE TABLE [dbo].[AzureUpdateCategories]
(
  [AzureUpdateId] NVARCHAR(250) NOT NULL,
  [CategoryId] INT NOT NULL,
  CONSTRAINT PK_AzureUpdateCategories PRIMARY KEY (AzureUpdateId, CategoryId),
  CONSTRAINT FK_AzureUpdateCategories_AzureUpdates FOREIGN KEY (AzureUpdateId)
      REFERENCES [dbo].[AzureUpdates]([Id]) ON DELETE CASCADE,
  CONSTRAINT FK_AzureUpdateCategories_Categories FOREIGN KEY (CategoryId)
      REFERENCES [dbo].[Categories]([Id]) ON DELETE CASCADE
)