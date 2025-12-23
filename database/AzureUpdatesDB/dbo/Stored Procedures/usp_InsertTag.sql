CREATE PROCEDURE [dbo].[usp_InsertTag]
    @Title NVARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the tag value already exists
    IF EXISTS (SELECT 1 FROM [dbo].[Tags] WHERE [Value] = @Title)
    BEGIN
        SELECT [Id] FROM [dbo].[Tags] WHERE [Value] = @Title;
        RETURN;
    END

    -- Insert the new tag and return the new identity
    INSERT INTO [dbo].[Tags] ([Value])
    VALUES (@Title);

    SELECT SCOPE_IDENTITY() AS [Id];
END