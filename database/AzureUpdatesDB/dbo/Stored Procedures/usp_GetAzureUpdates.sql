CREATE PROCEDURE [dbo].[usp_GetAzureUpdates]
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
    FROM [dbo].[AzureUpdates];
END