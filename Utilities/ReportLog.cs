using AventStack.ExtentReports.Model;

namespace CompetitionMarsSeCsharp.Utilities
{
    public class ReportLog
    {
        public static void Pass(string msg)
        {
            ExtentTestManager.GetTest().Pass(msg);
        }

        public static void Fail(string msg, Media media = null)
        {
            ExtentTestManager.GetTest().Fail(msg, media);
        }

        public static void Skip(string msg)
        {
            ExtentTestManager.GetTest().Skip(msg);
        }

        public static void Info(string msg)
        {
            ExtentTestManager.GetTest().Info(msg);
        }
    }
}
