using System;
using System.IO;

namespace Naval_combat
{
    public enum LogLevel
    {
        Debug,
        Info,
        Warning,
        Error
    }

    public class Logger
    {
        private string logFilePath;
        private LogLevel logLevel;

        public Logger(LogLevel logLevel, string logFilePath)
        {
            this.logLevel = logLevel;
            this.logFilePath = logFilePath;

            if (!string.IsNullOrEmpty(logFilePath))
            {
                File.WriteAllText(logFilePath, string.Empty);
            }
        }

        public void Log(LogLevel level, string message)
        {
            if (level < logLevel)
            {
                return;
            }

            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] - {message}";

            try
            {
                if (!string.IsNullOrEmpty(logFilePath))
                {
                    File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
        }

        public void LogException(Exception ex, string customMessage = "", [System.Runtime.CompilerServices.CallerMemberName] string callerMemberName = "")
        {
            // Логируем исключение
            Log(LogLevel.Error, $"Exception: {ex.GetType().Name}, Message: {ex.Message}, StackTrace: {ex.StackTrace}, Caller: {callerMemberName}, CustomMessage: {customMessage}");
        }
    }
}
