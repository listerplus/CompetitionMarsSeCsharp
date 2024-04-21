using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompetitionMarsSeCsharp.Utilities;
using OpenQA.Selenium;

namespace CompetitionMarsSeCsharp.Pages
{
    public  class HomePage(IWebDriver driver) : BasePage(driver)
    {

        // Web Elements
        public IWebElement BtnSignIn => WaitUtil.WaitVisible(driver, By.XPath("//a[@class='item'][(text()='Sign In')]"));
        public IWebElement FieldEmail => driver.FindElement(By.XPath("//input[@name='email']"));
        public IWebElement FieldPassword => driver.FindElement(By.XPath("//input[@name='password']"));
        public IWebElement BtnLogin => driver.FindElement(By.XPath("//button[@class='fluid ui teal button']"));


        // Text
        public readonly string emailValid = "one@test.com";
        public readonly string passwordValid = "Password1.";

        // Methods
        public void ClickSignIn()
        {
            BtnSignIn.Click();
        }

        public void Login()
        {
            ClickSignIn();
            FieldEmail.SendKeys(emailValid);
            FieldPassword.SendKeys(passwordValid);
            BtnLogin.Click();
        }

    }
}
