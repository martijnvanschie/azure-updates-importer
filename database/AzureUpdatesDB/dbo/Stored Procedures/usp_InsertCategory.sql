CREATE PROCEDURE [dbo].[usp_InsertCategory]
    @Title NVARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the category title already exists
    IF EXISTS (SELECT 1 FROM [dbo].[Categories] WHERE [Title] = @Title)
    BEGIN
        SELECT [Id] FROM [dbo].[Categories] WHERE [Title] = @Title;
        RETURN;
    END

    -- Insert the new category and return the new identity
    INSERT INTO [dbo].[Categories] ([Title])
    VALUES (@Title);

    SELECT SCOPE_IDENTITY() AS [Id];
END