using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ParkingSystemAPI.Models;
using System.Xml;

namespace ParkingSystemAPI.Test.xUnit
{
    public class UnitTestSelenium1 : IDisposable
    {
        private readonly IWebDriver chrome;
        //private readonly IWebDriver msEdge;
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
            chrome = new ChromeDriver();
            chrome.Manage().Window.Maximize();

            //msEdge = new EdgeDriver();
            //msEdge.Manage().Window.Maximize();

        }


        [Fact]
        public void Open_Google_Test()
        {
            chrome.Navigate().GoToUrl("https://www.google.com");

            Assert.Contains("Google", chrome.Title);

            Thread.Sleep(3000);
        }

        [Fact]
        public void APIVehicle_Get()
        {
            chrome.Navigate().GoToUrl(_baseUrl + "api/vehicle");

            //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            //wait.Until(d => d != null);

            string pageSrc = chrome.PageSource;

            var doc = new HtmlDocument();
            doc.LoadHtml(pageSrc);

            string jsonStr = doc.DocumentNode
                  .SelectSingleNode("//pre")
                  .InnerText;


            var objVM = JsonConvert.DeserializeObject<List<VehicleMaster>>(jsonStr);

            Assert.NotNull(objVM);
            Assert.True(objVM.Count > 0);

            //Thread.Sleep(3000);


        }

        public void Dispose()
        {
            if (chrome != null)
            {
                chrome.Quit();
            }
        }
    }
}