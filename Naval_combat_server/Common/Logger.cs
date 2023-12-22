using System;
using System.IO;

namespace Naval_combat_server.Common
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
        private object fileLock = new object();
        private Mutex mutex;

        public Logger(LogLevel logLevel, string logFilePath)
        {
            this.logLevel = logLevel;
            this.logFilePath = logFilePath;

            if (!string.IsNullOrEmpty(logFilePath))
            {
                File.WriteAllText(logFilePath, string.Empty);
            }

            // Используем глобальный мьютекс с именем "Global\\MyLoggerMutex"
            mutex = new Mutex(false, "Global\\MyLoggerMutex");
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
                lock (fileLock)
                {
                    // Захватываем мьютекс перед входом в критическую секцию
                    mutex.WaitOne();
                    try
                    {
                        // Критическая секция
                        File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                    }
                    finally
                    {
                        // Освобождаем мьютекс после выхода из критической секции
                        mutex.ReleaseMutex();
                    }
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
