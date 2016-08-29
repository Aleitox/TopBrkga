using System;
using System.IO;

namespace Main.Helpers
{
    public class Logger
    {
        public static void WriteOnFile(string message)
        {
            using (var writer = File.AppendText("Log.txt"))
            {
                writer.WriteLine("{0}{1}{1}", message, Environment.NewLine);
            }
        }

        public static void ClearLog()
        {
            File.WriteAllText(@"Log.txt", string.Empty);
        }

    }
}
