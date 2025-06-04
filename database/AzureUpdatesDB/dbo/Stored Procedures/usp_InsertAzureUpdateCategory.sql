CREATE PROCEDURE [dbo].[usp_InsertAzureUpdateCategory]
    @AzureUpdateId NVARCHAR(250),
    @CategoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the join already exists
    IF EXISTS (
        SELECT 1 FROM [dbo].[AzureUpdateCategories]
        WHERE [AzureUpdateId] = @AzureUpdateId AND [CategoryId] = @CategoryId
    )
    BEGIN
        PRINT 'This AzureUpdate-Category relationship already exists.';
        RETURN;
    END

    -- Insert the join
    INSERT INTO [dbo].[AzureUpdateCategories] ([AzureUpdateId], [CategoryId])
    VALUES (@AzureUpdateId, @CategoryId);
END