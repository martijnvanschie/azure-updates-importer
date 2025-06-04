CREATE PROCEDURE [dbo].[usp_InsertAzureUpdate]
    @Id NVARCHAR(250),
    @Title NVARCHAR(250),
    @Description NVARCHAR(MAX),
    @Url VARCHAR(MAX) = NULL,
    @DatePublished DATETIMEOFFSET
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the entity already exists
    IF EXISTS (SELECT 1 FROM [dbo].[AzureUpdates] WHERE [Id] = @Id)
    BEGIN
        PRINT'AzureUpdate with the specified Id already exists.'
        RETURN;
    END

    -- Insert the new entity
    INSERT INTO [dbo].[AzureUpdates] (
        [Id],
        [Title],
        [Description],
        [Url],
        [DatePublished]
    )
    VALUES (
        @Id,
        @Title,
        @Description,
        @Url,
        @DatePublished
    );
END