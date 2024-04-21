using System.Text.Json;
using CompetitionMarsSeCsharp.Pages;
using CompetitionMarsSeCsharp.Pages.Profile;
using CompetitionMarsSeCsharp.TestData;

namespace CompetitionMarsSeCsharp.Tests
{
    [Category("Regression")]
    [Category("Education")]
    [Author("Lister Sandalo")]
    public class TestEducation : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            HomePage homePage = new HomePage(driver);
            ProfilePage profilePage = new ProfilePage(driver);
            if (!profilePage.IsAtProfilePage())
            {
                homePage.Login();
                Thread.Sleep(2000);
            }
            profilePage.TabProfile.Click();
            profilePage.TabEdu.Click();
        }

        [Test]
        [TestCaseSource(nameof(EduJsonData))]
        public void Test01AddEducation(EduModel eduModel)
        {
            EducationTab eduTab = new EducationTab(driver);
            eduTab.AddEducation(eduModel.University, eduModel.Country, eduModel.Title, eduModel.Degree, eduModel.Year);
            eduTab.AssertBubble("added");
            bool isPresent = eduTab.IsEducationPresent(eduModel.University, eduModel.Country, eduModel.Title, eduModel.Degree, eduModel.Year);
            Assert.IsTrue(isPresent);
            // Remove added item to maintain state
            eduTab.IconRemoveLast.Click();
        }

        [Test]
        public void Test02UpdateEducation()
        {
            EducationTab eduTab = new EducationTab(driver);
            var eduModel = EduJsonDataSource();
            int modelCount = eduModel.Count;
            Random random = new Random();
            int index = random.Next(0, modelCount);
            // Select a random item from source to add then update after
            eduTab.AddEducation(eduModel[index].University, eduModel[index].Country, eduModel[index].Title, eduModel[index].Degree, eduModel[index].Year);
            Thread.Sleep(1000);

            int exclude = index;
            int newIndex;
            do { newIndex = random.Next(0, modelCount); } while (newIndex == exclude);
            eduTab.UpdateEduItemByModel(eduModel[index], eduModel[newIndex]);
            eduTab.AssertBubble("updated");

            bool isPresent = eduTab.IsEducationPresent(eduModel[newIndex].University, eduModel[newIndex].Country, eduModel[newIndex].Title, eduModel[newIndex].Degree, eduModel[newIndex].Year);
            Assert.IsTrue(isPresent);

            // Remove added item to maintain state
            eduTab.IconRemoveLast.Click();
        }

        [Test]
        public void Test03DeleteEducation()
        {
            EducationTab eduTab = new EducationTab(driver);
            var eduModel = EduJsonDataSource();
            Random random = new Random();
            int index = random.Next(0, eduModel.Count);
            // Select a random item from source to add then delete after
            eduTab.AddEducation(eduModel[index].University, eduModel[index].Country, eduModel[index].Title, eduModel[index].Degree, eduModel[index].Year);
            Thread.Sleep(1000);

            int row = eduTab.GetEducationItemRow(eduModel[index].University, eduModel[index].Country, eduModel[index].Title, eduModel[index].Degree, eduModel[index].Year);
            eduTab.ClickRemoveIcon(row);
            eduTab.AssertBubble("deleted");

            bool isPresent = eduTab.IsEducationPresent(eduModel[index].University, eduModel[index].Country, eduModel[index].Title, eduModel[index].Degree, eduModel[index].Year);
            Assert.IsFalse(isPresent);
        }

        [Test]
        public void Test04UnableToAddDuplicate()
        {
            EducationTab eduTab = new EducationTab(driver);
            var eduModel = EduJsonDataSource();
            Random random = new Random();
            int index = random.Next(0, eduModel.Count);
            // Select a random item from source to add then add again
            eduTab.AddEducation(eduModel[index].University, eduModel[index].Country, eduModel[index].Title, eduModel[index].Degree, eduModel[index].Year);
            Thread.Sleep(1000);
            int rowCount = eduTab.GetRowCount();

            eduTab.AddEducation(eduModel[index].University, eduModel[index].Country, eduModel[index].Title, eduModel[index].Degree, eduModel[index].Year);
            eduTab.AssertBubble("error");
            Assert.That(eduTab.GetRowCount(), Is.EqualTo(rowCount)); // Check Number of rows is still the same
            // remove added
            int row = eduTab.GetEducationItemRow(eduModel[index].University, eduModel[index].Country, eduModel[index].Title, eduModel[index].Degree, eduModel[index].Year);
            eduTab.ClickRemoveIcon(row);
        }

        //[TearDown]
        public void TearDown()
        {
            // Remove all Education items
        }

        public static List<EduModel> EduJsonDataSource()
        {
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData\\educationdata.json");
            var jsonString = File.ReadAllText(jsonFilePath);

            var eduModel = JsonSerializer.Deserialize<List<EduModel>>(jsonString);
            return eduModel;
        }

        public static IEnumerable<EduModel> EduJsonData()
        {
            var eduModel = EduJsonDataSource();
            foreach (var eduData in eduModel)
            {
                yield return eduData;
            }
        }

    }
}