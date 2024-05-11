using System.Text.Json;
using CompetitionMarsSeCsharp.AssertHelpers;
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
            GoToProfile();
            ProfilePage profilePage = new ProfilePage(driver);
            profilePage.TabEdu.Click();
        }

        [Test]
        [TestCaseSource(nameof(EduJsonData))]
        public void Test01AddEducation(EduModel eduModel)
        {
            EducationTab eduTab = new EducationTab(driver);
            EduAssertHelper eduAssert = new EduAssertHelper(driver);

            // Before Start: Delete specific data for this test and not wipe out other data
            eduTab.DeleteEduByModel(eduModel);

            // Start Test
            eduTab.AddEducation(eduModel.University, eduModel.Country, eduModel.Title, eduModel.Degree, eduModel.Year);
            eduAssert.AssertBubble("added");
            bool isPresent = eduTab.IsEducationPresent(eduModel.University, eduModel.Country, eduModel.Title, eduModel.Degree, eduModel.Year);
            Assert.IsTrue(isPresent);

            // After Test: Remove added item to maintain state
            eduTab.DeleteEduByModel(eduModel);
        }

        [Test]
        public void Test02UpdateEducation()
        {
            EducationTab eduTab = new EducationTab(driver);
            EduAssertHelper eduAssert = new EduAssertHelper(driver);
            var eduModel = EduJsonDataSource();
            int index = 0;
            int newIndex = 1;

            // Before Start: Delete specific data for this test and not wipe out other data
            eduTab.DeleteEduByModel(eduModel[index]);
            eduTab.DeleteEduByModel(eduModel[newIndex]);

            // Start Test
            // Select item from source to add then update after
            eduTab.AddEducation(eduModel[index].University, eduModel[index].Country, eduModel[index].Title, eduModel[index].Degree, eduModel[index].Year);
            Thread.Sleep(1000);
            eduTab.UpdateEduItemByModel(eduModel[index], eduModel[newIndex]);
            eduAssert.AssertBubble("updated");

            bool isPresent = eduTab.IsEducationPresent(eduModel[newIndex].University, eduModel[newIndex].Country, eduModel[newIndex].Title, eduModel[newIndex].Degree, eduModel[newIndex].Year);
            Assert.IsTrue(isPresent);

            // After Test:Remove data used if there is to maintain state
            eduTab.DeleteEduByModel(eduModel[index]);
            eduTab.DeleteEduByModel(eduModel[newIndex]);
        }

        [Test]
        public void Test03DeleteEducation()
        {
            EducationTab eduTab = new EducationTab(driver);
            EduAssertHelper eduAssert = new EduAssertHelper(driver);
            var eduModel = EduJsonDataSource();
            //Random random = new Random();
            //int index = random.Next(0, eduModel.Count);
            int index = 2;
            // Select item from source to add then delete after
            eduTab.AddEducation(eduModel[index].University, eduModel[index].Country, eduModel[index].Title, eduModel[index].Degree, eduModel[index].Year);
            Thread.Sleep(1000);

            int row = eduTab.GetEducationItemRow(eduModel[index].University, eduModel[index].Country, eduModel[index].Title, eduModel[index].Degree, eduModel[index].Year);
            eduTab.ClickRemoveIcon(row);
            eduAssert.AssertBubble("deleted");

            bool isPresent = eduTab.IsEducationPresent(eduModel[index].University, eduModel[index].Country, eduModel[index].Title, eduModel[index].Degree, eduModel[index].Year);
            Assert.IsFalse(isPresent);
        }

        [Test]
        public void Test04UnableToAddDuplicate()
        {
            EducationTab eduTab = new EducationTab(driver);
            EduAssertHelper eduAssert = new EduAssertHelper(driver);
            var eduModel = EduJsonDataSource();
            int index = 3;

            // Before Start: Delete specific data for this test and not wipe out other data
            eduTab.DeleteEduByModel(eduModel[index]);

            // Start Test
            // Select item from source to add then add again
            eduTab.AddEducation(eduModel[index].University, eduModel[index].Country, eduModel[index].Title, eduModel[index].Degree, eduModel[index].Year);
            Thread.Sleep(1000);
            int rowCount = eduTab.GetRowCount();

            eduTab.AddEducation(eduModel[index].University, eduModel[index].Country, eduModel[index].Title, eduModel[index].Degree, eduModel[index].Year);
            eduAssert.AssertBubble("error-duplicate");
            Assert.That(eduTab.GetRowCount(), Is.EqualTo(rowCount)); // Check Number of rows is still the same

            // After Test:Remove data used if there is to maintain state
            eduTab.DeleteEduByModel(eduModel[index]);
        }

        [Test]
        public void Test05UnableToAddIncompleteData()
        {
            EducationTab eduTab = new EducationTab(driver);
            EduAssertHelper eduAssert = new EduAssertHelper(driver);
            eduTab.BtnAddNew.Click();
            eduTab.BtnAdd.Click();
            eduAssert.AssertBubble("error-incomplete");
            eduTab.BtnCancel.Click();
        }

        //[TearDown]
        public void RemoveItems()
        {
            // Remove all items
            EducationTab eduTab = new EducationTab(driver);
            int rowCount = eduTab.GetRowCount();
            foreach (int value in Enumerable.Range(1, rowCount))
            {
                eduTab.ClickRemoveIcon(1);
                Thread.Sleep(500);
            }
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
            foreach (var eduData in eduModel.Skip(4))  // Skip first 4 items as this will be used in other tests
            {
                yield return eduData;
            }
        }

    }
}