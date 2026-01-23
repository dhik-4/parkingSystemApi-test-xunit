using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParkingSystemAPI.CustomModels.Vehicle;
using ParkingSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ParkingSystemAPI.Test.xUnit
{
    public class UnitTest1 : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        private string _baseUrl;
        private readonly AppDbContext _dbContext;
        private readonly IServiceScopeFactory _scopeFactory;

        public UnitTest1(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _config = factory.Services.GetRequiredService<IConfiguration>();
            _baseUrl = _config["CustomVariable:BaseUrlCall"];

            _dbContext = factory.Services
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<AppDbContext>();

            //_clientProgram = fact.CreateClient();
            _scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
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
        public async Task Swagger_Should_Load()
        {
            var response = await _client.GetAsync("/swagger/index.html");
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task AddVehicle_Test()
        {
            var data = new 
            {
                Code = "SB",
                VehicleName = "Speed Boat Test"
            };

            var strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _dbContext.Database.BeginTransactionAsync();

                // POST
                var response = await _client.PostAsJsonAsync(
                    "/api/vehicle",
                    data
                );

                response.EnsureSuccessStatusCode();
                Assert.True(response.IsSuccessStatusCode);

                // ASSERT (inside transaction)
                var vehicle = await _dbContext.VehicleMasters
                    .FirstOrDefaultAsync(v => v.VehicleName == data.VehicleName);

                Assert.NotNull(vehicle);

                // ROLLBACK
                await transaction.RollbackAsync();
            });
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