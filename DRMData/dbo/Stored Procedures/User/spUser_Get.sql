CREATE PROCEDURE [dbo].[spUser_Get]
	@Id nvarchar(128)
as
begin
	set nocount on;
	select [Id], [FirstName], [LastName], [EmailAddress], [CreatedDate]
	from [dbo].[User]
	where [Id] = @Id;
end