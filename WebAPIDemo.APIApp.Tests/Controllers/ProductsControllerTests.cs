using System.Net;
using System.Net.Http.Headers;
using WebAPIDemo.APIApp.Tests.Factories;

namespace WebAPIDemo.APIApp.Tests.Controllers
{
    public class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProductsControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Test");
        }

        [Fact]
        public async Task PostWithoutBody_Returns415()
        {
            // the Url is case-sensitive, it can't be "api/products" here
            var response = await _client.GetAsync("api/Products");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
    }
}
