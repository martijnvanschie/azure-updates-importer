CREATE PROCEDURE [dbo].[usp_InsertProduct]
    @Title NVARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the product title already exists
    IF EXISTS (SELECT 1 FROM [dbo].[Products] WHERE [Name] = @Title)
    BEGIN
        SELECT [Id] FROM [dbo].[Products] WHERE [Name] = @Title;
        RETURN;
    END

    -- Insert the new product and return the new identity
    INSERT INTO [dbo].[Products] ([Name])
    VALUES (@Title);

    SELECT SCOPE_IDENTITY() AS [Id];
    END