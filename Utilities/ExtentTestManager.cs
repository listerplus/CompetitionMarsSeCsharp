using AventStack.ExtentReports;
using System.Runtime.CompilerServices;

namespace CompetitionMarsSeCsharp.Utilities
{
    public class ExtentTestManager
    {
        //declare static field unique to each thread
        [ThreadStatic]
        private static ExtentTest mainTest;

        [ThreadStatic]
        private static ExtentTest subTest;

        // reference System.Runtime.CompilerServices
        // Only one thread can execute the method at a time
        // Multiple threads cannot execute the method concurrently
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest CreateMainTest(string testName, string description = null)
        {
            mainTest = ExtentService.GetExtent().CreateTest(testName, description);
            return mainTest;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest CreateSubTest(string testName, string description = null)
        {
            subTest = mainTest.CreateNode(testName, description);
            return subTest;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ExtentTest GetTest()
        {
            return subTest;
        }
    }
}
