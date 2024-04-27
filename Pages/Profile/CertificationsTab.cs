using CompetitionMarsSeCsharp.TestData;
using CompetitionMarsSeCsharp.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CompetitionMarsSeCsharp.Pages.Profile
{
    public class CertificationsTab(IWebDriver driver) : BasePage(driver)
    {
        // Web Elements
        public IWebElement BtnAddNew => driver.FindElement(By.XPath("//div[@data-tab='fourth']//table//div[@class='ui teal button '][(text()='Add New')]"));
        public IWebElement FieldCert => driver.FindElement(By.Name("certificationName"));
        public IWebElement FieldFrom => driver.FindElement(By.Name("certificationFrom"));
        public IWebElement DropYear => driver.FindElement(By.Name("certificationYear"));
        public IWebElement BtnAdd => driver.FindElement(By.XPath("//div[@data-tab='fourth']//input[@value='Add']"));
        public IWebElement BtnCancel => driver.FindElement(By.XPath("//div[@data-tab='fourth']//input[@value='Cancel']"));
        public IWebElement BtnUpdate => driver.FindElement(By.XPath("//div[@data-tab='fourth']//input[@value='Update']"));
        public IWebElement IconRemoveLast => driver.FindElement(By.XPath("//div[@data-tab='fourth']//table/tbody[last()]//i[@class='remove icon']"));
        public IList<IWebElement> CertificationRows => driver.FindElements(By.XPath("//div[@data-tab='fourth']//table/tbody"));

        // Methods
        public void InputCertification(string cert, string from, string year)
        {
            FieldCert.Clear();
            FieldCert.SendKeys(cert);
            FieldFrom.Clear();
            FieldFrom.SendKeys(from);
            SelectElement dropdownYear = new SelectElement(DropYear);
            dropdownYear.SelectByValue(year);
        }

        public void AddCertification(string cert, string from, string year)
        {
            BtnAddNew.Click();
            InputCertification(cert, from, year);
            BtnAdd.Click();
        }

        public int GetRowCount()
        {
            return CertificationRows.Count;
        }

        public int GetCertificationItemRow(string cert, string from, string yr)
        {
            string getCert, getFrom, getYear;

            for (int i = 1; i <= GetRowCount(); i++)
            {
                try
                {
                    getCert = driver.FindElement(By.XPath($"//div[@data-tab='fourth']//table/tbody[{i}]/tr/td[1]")).Text;
                    getFrom = driver.FindElement(By.XPath($"//div[@data-tab='fourth']//table/tbody[{i}]/tr/td[2]")).Text;
                    getYear = driver.FindElement(By.XPath($"//div[@data-tab='fourth']//table/tbody[{i}]/tr/td[3]")).Text;
                    if (cert.Equals(getCert) && from.Equals(getFrom) && yr.Equals(getYear))
                    {
                        //ReportLog.Info($"Cert Present at row: {i}");
                        return i;
                    }
                }
                catch (NoSuchElementException)
                {
                    continue;
                }
            }
            return 0;
        }

        public bool IsCertificationPresent(string cert, string from, string yr, int rowNum = 0)
        {
            bool isPresent = false;
            string getCert, getFrom, getYear;
            int eduRowPresent;

            if (rowNum == 0)
            {
                eduRowPresent = GetCertificationItemRow(cert, from, yr);
                if (eduRowPresent > 0)
                {
                    isPresent = true;
                }
            }
            else
            {
                getCert = WaitUtil.WaitVisible(driver, By.XPath($"//div[@data-tab='fourth']//table/tbody[{rowNum}]/tr/td[1]")).Text;
                getFrom = driver.FindElement(By.XPath($"//div[@data-tab='fourth']//table/tbody[{rowNum}]/tr/td[2]")).Text;
                getYear = driver.FindElement(By.XPath($"//div[@data-tab='fourth']//table/tbody[{rowNum}]/tr/td[3]")).Text;

                ReportLog.Info($"Retrieved [row {rowNum}]: Cert: {getCert}");
                if (cert.Equals(getCert) && from.Equals(getFrom) && yr.Equals(getYear)) { isPresent = true; }
            }

            return isPresent;
        }

        public void ClickWriteIcon(int row)
        {
            driver.FindElement(By.XPath($"//div[@data-tab='fourth']//table/tbody[{row}]//i[@class='outline write icon']")).Click();
        }

        public void ClickRemoveIcon(int row)
        {
            driver.FindElement(By.XPath($"//div[@data-tab='fourth']//table/tbody[{row}]//i[@class='remove icon']")).Click();
        }

        public void UpdateCertItemByModel(CertModel model1, CertModel model2)
        {
            int rowNum = GetCertificationItemRow(model1.Certificate, model1.From, model1.Year);
            ClickWriteIcon(rowNum);
            InputCertification(model2.Certificate, model2.From, model2.Year);
            BtnUpdate.Click();
        }

        public void AssertBubble(string action, string name=null) // action: added, updated, deleted, error-incomplete, err
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
