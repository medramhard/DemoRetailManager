CREATE PROCEDURE [dbo].[spProduct_Get]
	@Id int

AS
begin
	set nocount on;
	select [Id], [Name], [Description], [RetailPrice], [QuantityInStock], [CreateDate], [LastModified], [IsTaxable]
	from [dbo].[Product]
	where [Id] = @Id;
end