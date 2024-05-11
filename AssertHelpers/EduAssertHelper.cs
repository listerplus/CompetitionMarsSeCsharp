using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompetitionMarsSeCsharp.Pages;
using CompetitionMarsSeCsharp.Utilities;
using OpenQA.Selenium;

namespace CompetitionMarsSeCsharp.AssertHelpers
{
    public class EduAssertHelper(IWebDriver driver) : BasePage(driver)
    {

        public void AssertBubble(string action) // action: added, updated, deleted, error
        {
            Thread.Sleep(500);
            switch (action)
            {
                case "updated":
                    Assert.AreEqual($"Education as been updated", bubbleSuccess.Text);
                    Assert.AreEqual(popupSuccessColor, bubbleSuccess.GetCssValue("color"));
                    break;
                case "added":
                    Assert.AreEqual($"Education has been added", bubbleSuccess.Text);
                    Assert.AreEqual(popupSuccessColor, bubbleSuccess.GetCssValue("color"));
                    break;
                case "deleted":
                    Assert.AreEqual($"Education entry successfully removed", bubbleSuccess.Text);
                    Assert.AreEqual(popupSuccessColor, bubbleSuccess.GetCssValue("color"));
                    break;
                case "error-duplicate":
                    Assert.AreEqual($"This information is already exist.", bubbleError.Text);
                    ReportLog.Info($"bg color: {bubbleError.GetCssValue("color")}");
                    break;
                case "error-incomplete":
                    Assert.That(bubbleError.Text, Is.EqualTo($"Please enter all the fields"));
                    break;
                default:
                    Assert.Fail();
                    break;
            }
            Thread.Sleep(2000);
        }
    }
}
