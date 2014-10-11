if not exists (select 1 from sys.schemas where name = 'Inventory')
begin
	exec sp_executesql N'create schema Inventory'
	
	print 'Schema Inventory was created.'
end
else
begin
	print 'Schema Inventory already exists.'
end

if not exists (select 1 from sys.tables where object_id = object_id('Inventory.Product'))
begin
	create table Inventory.Product
	(
		Id int identity primary key,
		Name varchar(127) not null
	)

	print 'Table Product was created.'
end
else
begin
	print 'Table Product already exists.'
end

if exists (select 1 from sys.procedures where object_id = object_id('Inventory.SaveProduct'))
begin
	drop procedure Inventory.SaveProduct
end
GO

/*

	Either creates or updates a product as specified.

	@ID: -1 if the product is invalid; its ID otherwise.

*/
create procedure Inventory.SaveProduct
(
	@Id int out,
	@Name varchar(127)
)
as
begin
	set nocount on

	if ltrim(isnull(@Name, '')) = ''
	begin
		select @Id = -1		
	end
	else
	begin
		;merge Inventory.Product target
		using (select
			isnull(@Id, 0) as Id) source
		on (target.Id = source.Id)
		when matched then
			update
			set
				Name = @Name
		when not matched then
			insert (
				Name)
			values (
				@Name);

		select @Id = @@IDENTITY
	end
end
GO

print 'Procedure Inventory.SaveProduct was set up.'
GO

if exists (select 1 from sys.procedures where object_id = object_id('Inventory.DeleteProduct'))
begin
	drop procedure Inventory.DeleteProduct
end
GO

/*

	Deletes the product with the specified ID.

	@Result: 1 if it worked, 0 otherwise
*/
create procedure Inventory.DeleteProduct
(
	@Id int,
	@Result bit out
)
as
begin
	set nocount on

	delete Inventory.Product
	where
		Id = @Id

	select @Result =
		case
			when @@ROWCOUNT = 1 then 1
			else 0
		end
end
GO

print 'Procedure Inventory.DeleteProduct was set up.'
GO

if exists (select 1 from sys.procedures where object_id = object_id('Inventory.GetProducts'))
begin
	drop procedure Inventory.GetProducts
end
GO

/*

	Gets products with names like the specified value.

	Selects: Id, Name
*/
create procedure Inventory.GetProducts
(
	@NameSearch varchar(127)
)
as
begin
	set nocount on

	-- Clean up our search.
	select @NameSearch = '%' + ltrim(rtrim(@NameSearch)) + '%'

	select
		Id,
		Name
	from
		Inventory.Product
	where
		Name like @NameSearch
end
GO

print 'Procedure Inventory.GetProducts was set up.'
GO