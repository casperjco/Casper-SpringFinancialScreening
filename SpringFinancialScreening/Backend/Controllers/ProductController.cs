using Backend.Models;
using Backend.Queries;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Dapper;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
		private readonly ILogger<ProductController> _logger;
		private readonly IConfiguration _configuration;

		private readonly string _dbConnectionString;

		public ProductController(ILogger<ProductController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

			_dbConnectionString = _configuration.GetConnectionString("DatabaseConnectionString") ?? string.Empty;
		}

        [HttpGet]
		/// <summary>
		/// Get a list of products
		/// </summary>
		public IActionResult GetProducts([FromQuery] string query = "")
        {
            using var dbConnection = new MySqlConnection(_dbConnectionString);
            dbConnection.Open();

			IEnumerable<Product> products;

			if (string.IsNullOrWhiteSpace(query))
			{
				string sqlQuery = SqlQueries.GetAllProducts;
				products = dbConnection
					.Query<Product>(sqlQuery)
					.ToList();
			}
			else
			{
				string sqlQuery = SqlQueries.GetFilteredProducts;
				products = dbConnection
					.Query<Product>(sqlQuery, new { q = query, like = $"%{query}%" })
					.ToList();
			}

			return Ok(products);
		}

		[HttpPost("generate")]
		public IActionResult GenerateProducts()
		{
			using var dbConnection = new MySqlConnection(_dbConnectionString);
			dbConnection.Open();

			// Faker for realistic fake product data
			var faker = new Faker<Product>()
				.RuleFor(p => p.Name, f => f.Commerce.ProductName())
				.RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
				.RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
				.RuleFor(p => p.Brand, f => f.Company.CompanyName())
				.RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(10, 2000)))
				.RuleFor(p => p.StockAmount, f => f.Random.Int(0, 500))
				.RuleFor(p => p.SKU, f => f.Commerce.Ean13());

			var products = faker.Generate(1000);

			var sql = SqlQueries.InsertProducts;

			dbConnection.Execute(sql, products);

			return Ok("1000 random products inserted.");
		}
	}
}
