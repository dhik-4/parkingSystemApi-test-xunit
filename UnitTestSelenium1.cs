using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Text.Json;

namespace ParkingSystemAPI.Test.xUnit
{
    public class UnitTestSelenium1 : IDisposable
    {
        private readonly IWebDriver driver;
        private readonly string _baseUrl;

        public UnitTestSelenium1()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json", optional: false)
            .Build();

            // Read values
            _baseUrl = config["CustomVariable:BaseUrlCall"];

            //this.driver = _driver;
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();

            //_config = factory.Services.GetRequiredService<IConfiguration>();
            //_baseUrl = _config["CustomVariable:BaseUrlCall"];
        }


        [Fact]
        public void Open_Google_Work()
        {
            driver.Navigate().GoToUrl("https://www.google.com");

            Assert.Contains("Google", driver.Title);

            Thread.Sleep(3000);
        }

        [Fact]
        public void APIVehicleGetTrial()
        {
            driver.Navigate().GoToUrl(_baseUrl + "api/vehicle");

            //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            //wait.Until(d => d != null);

            string pageSrc = driver.PageSource;

            //Assert.Contains("\"ok\":false", driver.PageSource);


            Thread.Sleep(3000);

            
        }

        public void Dispose()
        {
            driver.Quit();
        }
    }
}
