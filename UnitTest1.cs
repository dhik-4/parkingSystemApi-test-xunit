using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ParkingSystemAPI.Test.xUnit
{
    public class UnitTest1 : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private string _baseUrl;

        public UnitTest1(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _config = factory.Services.GetRequiredService<IConfiguration>();
            _baseUrl = _config["CustomVariable:BaseUrlCall"];
        }

        [Fact]
        public async Task Fare_Test()
        {
            var response = await _client.GetAsync(_baseUrl + "api/Fare");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var product = await response.Content.ReadFromJsonAsync<dynamic>();
            JsonElement _jElem = product;
            Assert.True(_jElem.EnumerateArray().Count() >= 0);
        }

        [Fact]
        public async Task Debug_ConfigFile()
        {
            Console.WriteLine("Current Directory: " + Directory.GetCurrentDirectory());
            Console.WriteLine("File Exists: " + File.Exists("appsettings.Test.json"));

            Assert.True(true); // just to run the test
        }
    }
}