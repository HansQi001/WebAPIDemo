using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using WebAPIDemo.APIApp.Tests.Auth;
using WebAPIDemo.APIApp.Tests.Factories;

namespace WebAPIDemo.APIApp.Tests.Controllers
{
    public class AuthControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(FakeAuthHandler.AuthenticationScheme);
        }

        [Fact]
        public async Task Login_PostWithNoBody_Returns415()
        {
            var response = await _client.PostAsync("api/Auth/Login", null);

            Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
        }

        [Fact]
        public async Task Login_PostWithNoUsername_Returns415()
        {
            // Create the object you want to send
            var payload = new { username = "Hans" };

            // Serialize to JSON
            var json = JsonSerializer.Serialize(payload);

            // Wrap in StringContent with correct encoding and media type
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Auth/Login", content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_Post_Returns200()
        {
            // Create the object you want to send
            var payload = new { username = "Hans", Password = "1234" };

            // Serialize to JSON
            var json = JsonSerializer.Serialize(payload);

            // Wrap in StringContent with correct encoding and media type
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/Auth/Login", content);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
