using CompetitionMarsSeCsharp.TestData;
using CompetitionMarsSeCsharp.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace CompetitionMarsSeCsharp.Pages.Profile
{
    public class EducationTab(IWebDriver driver) : BasePage(driver)
    {
        // Web Elements
        public IWebElement BtnAddNew => driver.FindElement(By.XPath("//div[@data-tab='third']//table//div[@class='ui teal button '][(text()='Add New')]"));
        public IWebElement FieldUni => driver.FindElement(By.Name("instituteName"));
        public IWebElement FieldDegree => driver.FindElement(By.Name("degree"));
        public IWebElement DropCountry => driver.FindElement(By.Name("country"));
        public IWebElement DropTitle => driver.FindElement(By.Name("title"));
        public IWebElement DropYear => driver.FindElement(By.Name("yearOfGraduation"));
        public IWebElement BtnAdd => driver.FindElement(By.XPath("//div[@data-tab='third']//input[@value='Add']"));
        public IWebElement BtnCancel => driver.FindElement(By.XPath("//div[@data-tab='third']//input[@value='Cancel']"));
        public IWebElement BtnUpdate => driver.FindElement(By.XPath("//div[@data-tab='third']//input[@value='Update']"));
        public IWebElement IconRemoveLast => driver.FindElement(By.XPath("//div[@data-tab='third']//table/tbody[last()]//i[@class='remove icon']"));
        public IList<IWebElement> EducationRows => driver.FindElements(By.XPath("//div[@data-tab='third']//table/tbody"));

        // Methods
        public void InputEducation(string uni, string country, string title, string degree, string year)
        {
            FieldUni.Clear();
            FieldUni.SendKeys(uni);
            SelectElement dropdownCountry = new SelectElement(DropCountry);
            dropdownCountry.SelectByValue(country);
            SelectElement dropdownTitle = new SelectElement(DropTitle);
            dropdownTitle.SelectByValue(title);
            FieldDegree.Clear();
            FieldDegree.SendKeys(degree);
            SelectElement dropdownYear = new SelectElement(DropYear);
            dropdownYear.SelectByValue(year);
        }

        public void AddEducation(string uni, string country, string title, string degree, string year)
        {
            BtnAddNew.Click();
            InputEducation(uni, country, title, degree, year);
            BtnAdd.Click();
        }

        public int GetRowCount()
        {
            return EducationRows.Count;
        }

        public int GetEducationItemRow(string uni, string country, string title, string degree, string yr)
        {
            string getUni, getCountry, getTitle, getDegree, getYear;

            for (int i = 1; i <= GetRowCount(); i++)
            {
                try
                {
                    getCountry = driver.FindElement(By.XPath($"//div[@data-tab='third']//table/tbody[{i}]/tr/td[1]")).Text;
                    getUni = driver.FindElement(By.XPath($"//div[@data-tab='third']//table/tbody[{i}]/tr/td[2]")).Text;
                    getTitle = driver.FindElement(By.XPath($"//div[@data-tab='third']//table/tbody[{i}]/tr/td[3]")).Text;
                    getDegree = driver.FindElement(By.XPath($"//div[@data-tab='third']//table/tbody[{i}]/tr/td[4]")).Text;
                    getYear = driver.FindElement(By.XPath($"//div[@data-tab='third']//table/tbody[{i}]/tr/td[5]")).Text;
                    if (country.Equals(getCountry) && uni.Equals(getUni) && title.Equals(getTitle) && degree.Equals(getDegree) && yr.Equals(getYear))
                    {
                        ReportLog.Info($"Education Present at row: {i}");
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

        public bool IsEducationPresent(string uni, string country, string title, string degree, string yr, int rowNum = 0)
        {
            bool isPresent = false;
            string getUni, getCountry, getTitle, getDegree, getYear;
            int eduRowPresent;

            if (rowNum == 0)
            {
                eduRowPresent = GetEducationItemRow(uni, country, title, degree, yr);
                if (eduRowPresent > 0)
                {
                    isPresent = true;
                }
            }
            else
            {
                getCountry = WaitUtil.WaitVisible(driver, By.XPath($"//div[@data-tab='third']//table/tbody[{rowNum}]/tr/td[1]")).Text;
                getUni = WaitUtil.WaitVisible(driver, By.XPath($"//div[@data-tab='third']//table/tbody[{rowNum}]/tr/td[2]")).Text;
                getTitle = driver.FindElement(By.XPath($"//div[@data-tab='third']//table/tbody[{rowNum}]/tr/td[3]")).Text;
                getDegree = driver.FindElement(By.XPath($"//div[@data-tab='third']//table/tbody[{rowNum}]/tr/td[4]")).Text;
                getYear = driver.FindElement(By.XPath($"//div[@data-tab='third']//table/tbody[{rowNum}]/tr/td[5]")).Text;

                ReportLog.Info($"Retrieved [row {rowNum}]: Country: {getCountry}, Uni: {getUni}");
                if (country.Equals(getCountry) && uni.Equals(getUni) && title.Equals(getTitle) && degree.Equals(getDegree) && yr.Equals(getYear)) { isPresent = true; }
            }

            return isPresent;
        }

        public void ClickWriteIcon(int row)
        {
            driver.FindElement(By.XPath($"//div[@data-tab='third']//table/tbody[{row}]//i[@class='outline write icon']")).Click();
        }

        public void ClickRemoveIcon(int row)
        {
            driver.FindElement(By.XPath($"//div[@data-tab='third']//table/tbody[{row}]//i[@class='remove icon']")).Click();
        }



        public void UpdateEduItemByModel(EduModel model1, EduModel model2)
        {
            int rowNum = GetEducationItemRow(model1.University, model1.Country, model1.Title, model1.Degree, model1.Year);
            ClickWriteIcon(rowNum);
            InputEducation(model2.University, model2.Country, model2.Title, model2.Degree, model2.Year);
            BtnUpdate.Click();
        }

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
