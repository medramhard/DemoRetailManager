CREATE PROCEDURE [dbo].[spProduct_UpdateQuantity]
	@Id INT,          
    @Name NVARCHAR (100),
    @Description NVARCHAR (MAX),
    @RetailPrice MONEY,
    @QuantityInStock INT,
    @IsTaxable BIT
AS
begin
	set nocount on;

	update [dbo].[Product]
	set [QuantityInStock] = @QuantityInStock
	where [Id] = @Id;
end
