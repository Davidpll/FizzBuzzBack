using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FizzBuzz.IntegrationTest.Presentation
{
    public class FizzBuzzControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public FizzBuzzControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GenerateFizzBuzz_ValidInput_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/fizzbuzz/generate?start=1&limit=3");

            // Assert
            response.EnsureSuccessStatusCode(); 
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Fizz", content);
        }

        [Fact]
        public async Task GenerateFizzBuzz_InvalidInput_ReturnsBadRequest()
        {
            // Act
            var response = await _client.GetAsync("/api/fizzbuzz/generate?start=0&limit=3");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}