using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace UserCredentialsValidator.AzureAPI.Utility
{
    internal static class ContentResultWrapper
    {
        public static ActionResult Result(HttpStatusCode statusCode, string reason) => new ContentResult
        {
            StatusCode = (int)statusCode,
            Content = $"{reason}",
            ContentType = "text/plain",
        };
    }
}