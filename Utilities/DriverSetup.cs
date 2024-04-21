using System;
using System.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace CompetitionMarsSeCsharp.Utilities
{
    public class DriverSetup
    {
        public static IWebDriver BrowserSetup(IWebDriver driver, string browserType)
        {
            switch (browserType.ToLower())
            {
                case "chrome":
                    driver = new ChromeDriver();
                    break;
                case "firefox":
                    driver = new FirefoxDriver();
                    break;
                case "edge":
                    driver = new EdgeDriver();
                    break;
                default:
                    Console.WriteLine($"Unrecognized browser: {browserType}. Defaulting to Chrome.");
                    driver = new ChromeDriver();
                    break;
            }
            driver.Manage().Window.Maximize();
            return driver;

        }
    }
}
