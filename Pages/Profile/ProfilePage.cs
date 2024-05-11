using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompetitionMarsSeCsharp.Utilities;
using OpenQA.Selenium;

namespace CompetitionMarsSeCsharp.Pages.Profile
{
    public class ProfilePage(IWebDriver driver) : BasePage(driver)
    {

        // By Locator
        public static By TabProfileBy = By.XPath("//section//a[@href='/Account/Profile']");

        // Web Elements
        public IWebElement TabProfile => WaitUtil.WaitClickable(driver, TabProfileBy);
        public IWebElement TabEdu => driver.FindElement(By.XPath("//a[@data-tab='third']"));
        public IWebElement TabCert => WaitUtil.WaitVisible(driver, By.XPath("//a[@data-tab='fourth']"));


        // Methods
        public bool IsAtProfilePage()
        {
            return IsFound(TabProfileBy);
        }

    }
}
