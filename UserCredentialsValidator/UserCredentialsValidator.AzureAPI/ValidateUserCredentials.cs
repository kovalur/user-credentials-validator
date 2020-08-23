using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using UserCredentialsValidator.AzureAPI.Utility;

namespace UserCredentialsValidator.AzureAPI
{
    public static class ValidateUserCredentials
    {
        [FunctionName("ValidateUserCredentials")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function [ValidateUserCredentials] processed a request.");

            log.LogInformation("Read and validate the HTTP request JSON.");

            dynamic data = null;
            try
            {
                string reqBody = await new StreamReader(req.Body).ReadToEndAsync();

                data = JsonConvert.DeserializeObject(reqBody);
            }
            catch (Exception ex)
            {
                log.LogInformation($"Unable to deserialize the request Body JSON to a .NET dynamic object. {ex.Message}");

                return ContentResultWrapper.Result(HttpStatusCode.BadRequest, "Invalid HTTP request JSON format.");
            }

            string email = data?.email;
            string password = data?.password;

            if (String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(password))
            {
                log.LogInformation("Invalid HTTP request params received.");

                return ContentResultWrapper.Result(HttpStatusCode.BadRequest, "Invalid params received.");
            }

            string domain = null;
            try
            {
                MailAddress mailAddress = new MailAddress(email);

                domain = mailAddress.Host;
            }
            catch (FormatException)
            {
                return ContentResultWrapper.Result(HttpStatusCode.Accepted, "Invalid email address format.");
            }

            if (!domain.IsRegularMailAddressDomain())
            {
                return ContentResultWrapper.Result(HttpStatusCode.Accepted, "We are only supporting regular email address domains (e.g 'example@google.com').");
            }

            if (!password.ContainsOnlyAlphanumericCharacters())
            {
                return ContentResultWrapper.Result(HttpStatusCode.Accepted, "Password contains invalid characters.");
            }

            return ContentResultWrapper.Result(HttpStatusCode.OK, "Valid user credentials.");
        }
    }
}