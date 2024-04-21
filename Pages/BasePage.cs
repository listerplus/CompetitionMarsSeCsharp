using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompetitionMarsSeCsharp.Utilities;
using OpenQA.Selenium;

namespace CompetitionMarsSeCsharp.Pages
{
    public  class BasePage
    {
        protected readonly IWebDriver driver;

        // Web Elements
        public IWebElement bubbleSuccess => WaitUtil.WaitVisible(driver, By.XPath($"//div[@class='ns-box ns-growl ns-effect-jelly ns-type-success ns-show']/div"));
        public IWebElement bubbleError => WaitUtil.WaitVisible(driver, By.XPath($"//div[@class='ns-box ns-growl ns-effect-jelly ns-type-error ns-show']/div"));

        // Constants
        public string popupSuccessColor = "rgba(43, 60, 97, 1)";
        public string popupErrorColor = "rgba(43, 60, 97, 1)"; // To Update

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void DoClick (IWebElement element)
        {
            element.Click();
        }

        public void DoSendKeys(IWebElement element, string text)
        {
            element.SendKeys(text);
        }

        public string GetElementText(IWebElement element)
        {
            return element.Text;
        }

        public bool IsFound(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

    }
}
