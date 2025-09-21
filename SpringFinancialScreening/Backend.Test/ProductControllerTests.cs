using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;

namespace Backend.Test
{
	public class ProductControllerTests
	{
		private readonly HttpClient _client;

		public ProductControllerTests()
		{
			_client = new HttpClient();
		}

		[Fact]
		public async Task GetProducts_WithoutQuery_ReturnsAllProducts()
		{
			// Arrange & Act
			var response = await _client.GetAsync("/api/products");

			// Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}
	}
}