using FizzBuzz.Appilcation.Dtos;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Hosting;

namespace FizzBuzz.IntegrationTests.Presentation
{
    public class FizzBuzzControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        //private readonly HttpClient _client;

        //public FizzBuzzControllerTests(WebApplicationFactory<Program> factory)
        //{
        //    // Crea un cliente HTTP para realizar las solicitudes a la aplicación en memoria
        //    _client = factory.CreateClient();
        //}

        //[Fact]
        //public async Task GenerateFizzBuzz_ShouldReturnBadRequest_WhenInvalidRange()
        //{
        //    // Arrange
        //    var request = new FizzBuzzRequest { Start = 5, Limit = 1 }; // Start > Limit
        //    var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

        //    // Act
        //    var response = await _client.GetAsync($"/api/fizzbuzz/generate?start={request.Start}&limit={request.Limit}");

        //    // Assert
        //    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    Assert.Contains("Limit must be greater than or equal to Start.", responseContent);
        //}

        private readonly HttpClient _client;

        public FizzBuzzControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GenerateFizzBuzz_ValidInput_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/api/fizzbuzz/generate?start=1&limit=3");

            // Assert
            response.EnsureSuccessStatusCode(); // Verifica que el código de estado sea 200
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Fizz", content); // Verifica que la respuesta contenga "Fizz"
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
