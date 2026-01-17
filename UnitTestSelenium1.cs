using OpenQA.Selenium;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;

namespace ParkingSystemAPI.Test.xUnit
{
    public class UnitTestSelenium1 : IDisposable
    {
        private readonly IWebDriver driver;

        public UnitTestSelenium1()
        {
            //this.driver = _driver;
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }


        [Fact]
        public void Open_Google_Should_Work()
        {
            driver.Navigate().GoToUrl("https://www.google.com");

            Assert.Contains("Google", driver.Title);

            Thread.Sleep(5000);
        }

        public void Dispose()
        {
            driver.Quit();
        }
    }
}
