using System.IO;

namespace WindowsColourModeSwitcher
{
    class Logger
    {
        public static string LOG_FILE { get; private set; } = "log.txt";
        private static string SESSION_BRACKET_LOG = "----------------------------------------------------";


        private static void WriteToFile(string message)
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LOG_FILE);
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{message}");
            }
        }
        
        private static string GenSection(string sectionName)
        {
            int numberOfDashes = (80 - sectionName.Length) / 2;
            string dashes = new string('-', numberOfDashes);
            return $"{dashes} [ {sectionName} ] {dashes}";
        }

        public static void Log(string message, LOG_TYPE logType = LOG_TYPE.INFO)
        {
            WriteToFile($"{DateTime.Now} [{logType.ToString()}]: {message}");
        }

        public static void LogStart()
        {
            WriteToFile($"{SESSION_BRACKET_LOG}{GenSection("Starting the application session")}{SESSION_BRACKET_LOG}");
        }

        public static void LogEnd()
        {
            WriteToFile($"{SESSION_BRACKET_LOG}{GenSection("Ending session")}{SESSION_BRACKET_LOG}\n\n\n");
        }
    }

    enum LOG_TYPE
    {
        INFO,
        WARNING,
        ERROR
    }
}