using System.Text.Json;
using CompetitionMarsSeCsharp.AssertHelpers;
using CompetitionMarsSeCsharp.Pages;
using CompetitionMarsSeCsharp.Pages.Profile;
using CompetitionMarsSeCsharp.TestData;
using CompetitionMarsSeCsharp.Utilities;

namespace CompetitionMarsSeCsharp.Tests
{
    [Category("Regression")]
    [Category("Certifications")]
    [Author("Lister Sandalo")]
    public class TestCertifications : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            GoToProfile();
            ProfilePage profilePage = new ProfilePage(driver);
            profilePage.TabCert.Click();
        }

        [Test]
        [TestCaseSource(nameof(CertJsonData))]
        public void Test01AddCert(CertModel certModel)
        {
            CertificationsTab certTab = new CertificationsTab(driver);
            CertAssertHelper certAssert = new CertAssertHelper(driver);
            certTab.AddCertification(certModel.Certificate, certModel.From, certModel.Year);
            certAssert.AssertBubble("added", certModel.Certificate);
            bool isPresent = certTab.IsCertificationPresent(certModel.Certificate, certModel.From, certModel.Year);
            Assert.IsTrue(isPresent);
            ReportLog.Info($"Successfully added cert: {certModel.Certificate}");
            // Remove added item to maintain state
            certTab.IconRemoveLast.Click();
        }

        [Test]
        public void Test02UpdateCert()
        {
            CertificationsTab certTab = new CertificationsTab(driver);
            CertAssertHelper certAssert = new CertAssertHelper(driver);
            var certModel = CertJsonDataSource();
            int modelCount = certModel.Count;
            // Commenting out using random since we will be using an item not being used by other tests
            //Random random = new Random();
            //int index = random.Next(0, modelCount);
            int index = 0;
            // Select item 1 from source to add then update after. Item 1 is not used on other tests.
            certTab.AddCertification(certModel[index].Certificate, certModel[index].From, certModel[index].Year);
            Thread.Sleep(1000);

            //int exclude = index;
            //int newIndex;
            //do { newIndex = random.Next(0, modelCount); } while (newIndex == exclude);
            int newIndex = 1;
            certTab.UpdateCertItemByModel(certModel[index], certModel[newIndex]);
            certAssert.AssertBubble("updated", certModel[newIndex].Certificate);

            bool isPresent = certTab.IsCertificationPresent(certModel[newIndex].Certificate, certModel[newIndex].From, certModel[newIndex].Year);
            Assert.IsTrue(isPresent);
            ReportLog.Info($"Cert updated" +
                $"\nFrom Cert: {certModel[index].Certificate}, from: {certModel[index].From}, year: {certModel[index].Year}" +
                $"\nTo Cert: {certModel[newIndex].Certificate}, from: {certModel[newIndex].From}, year: {certModel[newIndex].Year}");

            // Remove added item to maintain state
            certTab.IconRemoveLast.Click();
        }

        [Test]
        public void Test03DeleteCert()
        {
            CertificationsTab certTab = new CertificationsTab(driver);
            CertAssertHelper certAssert = new CertAssertHelper(driver);
            var certModel = CertJsonDataSource();
            //Random random = new Random();
            //int index = random.Next(0, certModel.Count);
            int index = 2;
            // Select item 3 from source to add then delete after
            certTab.AddCertification(certModel[index].Certificate, certModel[index].From, certModel[index].Year);
            Thread.Sleep(1000);

            int row = certTab.GetCertificationItemRow(certModel[index].Certificate, certModel[index].From, certModel[index].Year);
            certTab.ClickRemoveIcon(row);
            certAssert.AssertBubble("deleted", certModel[index].Certificate);

            bool isPresent = certTab.IsCertificationPresent(certModel[index].Certificate, certModel[index].From, certModel[index].Year);
            Assert.IsFalse(isPresent);
            ReportLog.Info($"Removed. Cert: {certModel[index].Certificate}");
        }

        [Test]
        public void Test04UnableToAddDuplicate()
        {
            CertificationsTab certTab = new CertificationsTab(driver);
            CertAssertHelper certAssert = new CertAssertHelper(driver);
            var certModel = CertJsonDataSource();
            //Random random = new Random();
            //int index = random.Next(0, certModel.Count);
            int index = 3;
            // Select item 4 (index 3) from source to add then add again
            certTab.AddCertification(certModel[index].Certificate, certModel[index].From, certModel[index].Year);
            Thread.Sleep(1000);
            int rowCount = certTab.GetRowCount();

            certTab.AddCertification(certModel[index].Certificate, certModel[index].From, certModel[index].Year);
            certAssert.AssertBubble("error-duplicate");
            Assert.That(certTab.GetRowCount(), Is.EqualTo(rowCount)); // Check Number of rows is still the same
            // remove added
            int row = certTab.GetCertificationItemRow(certModel[index].Certificate, certModel[index].From, certModel[index].Year);
            certTab.ClickRemoveIcon(row);
        }

        [Test]
        public void Test05UnableToAddIncompleteData()
        {
            CertificationsTab certTab = new CertificationsTab(driver);
            CertAssertHelper certAssert = new CertAssertHelper(driver);
            certTab.BtnAddNew.Click();
            certTab.BtnAdd.Click();
            certAssert.AssertBubble("error-incomplete");
            certTab.BtnCancel.Click();
        }

        [TearDown]
        public void RemoveItems()
        {
            // Reset State: Remove all items remaining
            CertificationsTab certTab = new CertificationsTab(driver);
            int rowCount = certTab.GetRowCount();
            foreach (int value in Enumerable.Range(1, rowCount)) // https://kodify.net/csharp/loop/range/
            {
                certTab.ClickRemoveIcon(1);
                Thread.Sleep(500);
            }
        }

        public static List<CertModel> CertJsonDataSource()
        {
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData\\certificationdata.json");
            var jsonString = File.ReadAllText(jsonFilePath);

            var certModel = JsonSerializer.Deserialize<List<CertModel>>(jsonString);
            return certModel;
        }

        public static IEnumerable<CertModel> CertJsonData()
        {
            var certModel = CertJsonDataSource();
            foreach (var certData in certModel.Skip(4))  // Skip first 4 items as this will be used in other tests
            {
                yield return certData;
            }
        }

    }
}