CREATE PROCEDURE [dbo].[spSale_Add]
	@CashierId nvarchar(128),
	@SaleDate datetime2(7),
	@SubTotal money,
	@Tax money,
	@Total money
AS
begin
	set nocount on;
	insert into [dbo].[Sale]([CashierId], [SaleDate], [SubTotal], [Tax], [Total])
	values (@CashierId, @SaleDate, @SubTotal, @Tax, @Total);
	select SCOPE_IDENTITY();
end