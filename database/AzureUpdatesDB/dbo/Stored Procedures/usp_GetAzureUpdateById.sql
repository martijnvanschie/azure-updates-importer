CREATE PROCEDURE [dbo].[usp_GetAzureUpdateById]
    @Id NVARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        [Id],
        [Title],
        [Description],
        [Url],
        [DatePublished],
        [DateCreated],
        [DateModified]
    FROM [dbo].[AzureUpdates]
    WHERE [Id] = @Id;
END