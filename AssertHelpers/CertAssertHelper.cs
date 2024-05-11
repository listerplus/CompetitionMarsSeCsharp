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
    public class CertAssertHelper(IWebDriver driver) : BasePage(driver)
    {

        public void AssertBubble(string action, string name = null) // action: added, updated, deleted, error-incomplete, err
        {
            Thread.Sleep(500);
            switch (action)
            {
                case "updated":
                    Assert.That(bubbleSuccess.Text, Is.EqualTo($"{name} has been updated to your certification"));
                    Assert.That(bubbleSuccess.GetCssValue("color"), Is.EqualTo(popupSuccessColor));
                    break;
                case "added":
                    Assert.That(bubbleSuccess.Text, Is.EqualTo($"{name} has been added to your certification"));
                    Assert.That(bubbleSuccess.GetCssValue("color"), Is.EqualTo(popupSuccessColor));
                    break;
                case "deleted":
                    Assert.That(bubbleSuccess.Text, Is.EqualTo($"{name} has been deleted from your certification"));
                    Assert.That(bubbleSuccess.GetCssValue("color"), Is.EqualTo(popupSuccessColor));
                    break;
                case "error-duplicate":
                    Assert.That(bubbleError.Text, Is.EqualTo($"This information is already exist."));
                    ReportLog.Info($"bg color: {bubbleError.GetCssValue("color")}");
                    break;
                case "error-incomplete":
                    Assert.That(bubbleError.Text, Is.EqualTo($"Please enter Certification Name, Certification From and Certification Year"));
                    break;
                default:
                    Assert.Fail();
                    ReportLog.Info($"Action: '{action}' not in the list");
                    break;
            }
            Thread.Sleep(2000);
        }

    }
}
