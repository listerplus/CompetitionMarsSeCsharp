using System;

namespace CompetitionMarsSeCsharp.Utilities
{
    public class Util
    {
        public static string GetProjRootDir()
        {
            string currentDir = Directory.GetCurrentDirectory();
            return currentDir.Split("bin")[0];
        }
    }
}
