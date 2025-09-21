namespace Backend.Queries
{
	public static class SqlQueries
	{
		public const string GetAllProducts = @"
			SELECT 
				Id, Name, Description, Category, Brand, Price, StockAmount, SKU
			FROM 
				Products;
		";

		public const string GetFilteredProducts = @"
			SELECT 
				Id, Name, Description, Category, Brand, Price, StockAmount, SKU
			FROM 
				Products
            WHERE MATCH(Name, Description) AGAINST (@q IN NATURAL LANGUAGE MODE)
                OR Category LIKE @like
                OR Brand LIKE @like
                OR SKU LIKE @like";

		public const string InsertProducts = @"
			INSERT INTO Products 
				(Name, Description, Category, Brand, Price, StockAmount, SKU) 
			VALUES 
				(@Name, @Description, @Category, @Brand, @Price, @StockAmount, @SKU)";
	}
}
