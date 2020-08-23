using System;
using System.IO;
using System.Text;
using UserCredentialsValidator.LoggingService.Utility;
using UserCredentialsValidator.SharedResources;

namespace UserCredentialsValidator.LoggingService
{
    public static class Logger
    {
        private readonly static string _logFilePath = Path.Combine(Path.GetTempPath(), ResourcesManager.LogFileName);

        private static string CreateExceptionMessage(Exception ex)
        {
            return $"[{DateTime.UtcNow}] {ex.Message}\n{ex.GetExceptionInfo()}.\n";
        }
        private static string CreateMessage(string message)
        {
            return $"[{DateTime.UtcNow}] {message.TrimEnd('.')}.\n";
        }

        public static void LogException(this Exception ex)
        {
            File.AppendAllText(_logFilePath, CreateExceptionMessage(ex), Encoding.UTF8);
        }
        public static void LogMessage(this String message)
        {
            File.AppendAllText(_logFilePath, CreateMessage(message), Encoding.UTF8);
        }
    }
}