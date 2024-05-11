using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using CompetitionMarsSeCsharp.Utilities;
using CompetitionMarsSeCsharp.Pages.Profile;
using CompetitionMarsSeCsharp.Pages;

namespace CompetitionMarsSeCsharp.Tests
{
    public class BaseTest
    {
        public IWebDriver driver;
        //Creating a class for extent test instead of declaring Threadlocal driver holding instance of ExtentTest
        //public ThreadLocal<ExtentTest> extent_test = new ThreadLocal<ExtentTest>();

        // Called once prior to executing any of the tests in a fixture
        [OneTimeSetUp]
        public void BaseFixtureSetup()
        {
            ExtentTestManager.CreateMainTest(GetType().Name);
            AppConfig config = AppConfig.LoadConfiguration();
            driver = DriverSetup.BrowserSetup(driver, config.Browser);
            driver.Navigate().GoToUrl(config.Url);
        }

        public IWebDriver GetDriver()
        {
            return driver;
        }

        [OneTimeTearDown]
        public void BaseFixtureTeardown()
        {
            driver.Quit();
            ExtentService.GetExtent().Flush();
        }

        // Called before each test method in the derived class
        [SetUp]
        public void BaseSetup()
        {
            //test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
            //extent_test.Value = test
            ExtentTestManager.CreateSubTest(TestContext.CurrentContext.Test.Name);
        }

        // Performed after each test method in derived class
        [TearDown]
        public void BaseTeardown()
        {
            try
            {
                var status = TestContext.CurrentContext.Result.Outcome.Status;
                var errorMessage = string.IsNullOrEmpty(TestContext.CurrentContext.Result.Message) ? "" : string.Format("<pre>{0}</pre>", TestContext.CurrentContext.Result.Message);
                var stackTrace = string.IsNullOrEmpty(TestContext.CurrentContext.Result.StackTrace) ? "" : string.Format("<pre>{0}</pre>", TestContext.CurrentContext.Result.StackTrace);

                switch (status)
                {
                    case TestStatus.Failed:
                        ReportLog.Fail("FAILED");
                        ReportLog.Fail(errorMessage);
                        ReportLog.Fail(stackTrace);
                        ReportLog.Fail("Screenshot", CaptureScreenshot(TestContext.CurrentContext.Test.Name));
                        break;

                    case TestStatus.Skipped:
                        ReportLog.Skip("SKIPPED");
                        break;

                    case TestStatus.Passed:
                        ReportLog.Pass("PASSED");
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception: " + ex);
            }
/*            finally
            {
                driver.Quit();
            }*/
        }
        public Media CaptureScreenshot(string name)
        {
            var screenshot = ((ITakesScreenshot)driver).GetScreenshot().AsBase64EncodedString;
            return MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot, name).Build();
        }

        public void GoToProfile()
        {
            HomePage homePage = new HomePage(driver);
            ProfilePage profilePage = new ProfilePage(driver);
            if (!profilePage.IsAtProfilePage())
            {
                homePage.Login();
                Thread.Sleep(2000);
            }
            profilePage.TabProfile.Click();
        }

    }
}
