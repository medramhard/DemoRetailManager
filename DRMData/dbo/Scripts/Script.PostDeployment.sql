if not exists (select * from [dbo].[Product])

begin
	insert into [dbo].[Product] ([Name], [Description], [RetailPrice], [QuantityInStock], [IsTaxable])
	values ('Puffs Ultra Soft Facial Tissuess', 'Mega cube, 72 facial tissues per box', 3.98, 20, 1),
	('AJAX Liquid Dish Soap', '90 floz., Orange scent dish soap', 5.74, 50, 1),
	('OxiClean Laundry Detergent', '118 floz., Arm & Hammer Plus OxiClean Odor Blasters Fresh Burst, 75 Loads Liquid Laundry Detergent', 8.98, 40, 1);

	insert into [dbo].[Inventory] ([ProductId], [Quantity], [PurchasePrice])
	values (1, 20, 3.98),
	(2, 50, 5.74),
	(3, 40, 8.98);
end