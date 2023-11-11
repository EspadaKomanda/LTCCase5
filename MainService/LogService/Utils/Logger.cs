namespace LogService.Utils
{
    public class Logger
    {
        private string logFilePath;

        public Logger(string filePath)
        {
            logFilePath = filePath;
        }

        public async Task LogAsync(LogLevel level, string message)
        {
            string logMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " [" + level.ToString() + "] - " + message;

            try
            {
                using (StreamWriter sw = File.AppendText(logFilePath))
                {
                    await sw.WriteLineAsync(logMessage);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while writing to the log file: " + e.Message);
            }
        }

        public async Task<List<LogEntry>> ReadLogsAsync(DateTime startTime, DateTime endTime)
        {
            List<LogEntry> logs = new List<LogEntry>();

            try
            {
                using (StreamReader sr = new StreamReader(logFilePath))
                {
                    string line;
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        string[] parts = line.Split(new char[] { ' ' }, 4);
                        if (parts.Length == 4)
                        {
                            DateTime timestamp;
                            if (DateTime.TryParse(parts[0] + " " + parts[1], out timestamp))
                            {
                                LogLevel level;
                                if (Enum.TryParse(parts[2].Trim('[', ']'), out level))
                                {
                                    string message = parts[3];
                                    if (timestamp >= startTime && timestamp <= endTime)
                                    {
                                        logs.Add(new LogEntry { Timestamp = timestamp, Level = level, Message = message });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while reading the log file: " + e.Message);
            }

            return logs;
        }
    }
}
