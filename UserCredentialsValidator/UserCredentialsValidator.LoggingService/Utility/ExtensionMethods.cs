using System;

namespace UserCredentialsValidator.LoggingService.Utility
{
    public static class ExtensionMethods
    {
        public static string GetExceptionInfo(this Exception exception)
        {
            return new
            {
                ErrorMessage = exception.Message,
                Source = exception.Source,
                StackTrace = exception.StackTrace,
                Target = exception.TargetSite.ToString(),
                InnerExceptionMessage = exception.InnerException.Message,
            }.ToString();
        }
    }
}