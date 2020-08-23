using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UserCredentialsValidator.LoggingService;
using UserCredentialsValidator.SharedResources;

namespace UserCredentialsValidator.ClientApp
{
    public class ClientConsoleApplication
    {
        private static ClientConsoleApplication _clientConsoleApplication = null;
        private HttpClient _httpClient = new HttpClient();
        private readonly Uri _validateUserCredentialsFunctionUri = new Uri($"{ResourcesManager.AzureAPIBaseURL}/ValidateUserCredentials?code={ResourcesManager.ValidateUserCredentialsFunctionKey}");

        private ClientConsoleApplication() { }

        public static ClientConsoleApplication GetInstance()
        {
            if (_clientConsoleApplication == null)
            {
                _clientConsoleApplication = new ClientConsoleApplication();
            }
            return _clientConsoleApplication;
        }
        internal async Task StartCyclingValidation()
        {
            string email = null;
            string password = null;
            bool isFirstRun = true;

            Console.WriteLine(new String('*', 42));

            Console.WriteLine("Please enter User Credentials to validate (or press 'Ctrl+C' to exit).");

            while (true)
            {
                if (isFirstRun)
                {
                    isFirstRun = false;
                }
                else
                {
                    Console.WriteLine("Please enter new User Credentials to validate (or press 'Ctrl+C' to exit).");
                }

                Console.Write("Email: ");
                email = Console.ReadLine();

                Console.Write("Password: ");
                password = Console.ReadLine();

                (bool status, string resultMessage) = await ValidateUserCredentials(email, password);

                if (status)
                {
                    Console.WriteLine($"\"=>\" {resultMessage}");
                }
                else
                {
                    Console.WriteLine(resultMessage);
                }

                Console.WriteLine(new String('*', 42));
            }
        }
        public async Task<(bool, string)> ValidateUserCredentials(string email, string password)
        {
            bool isEmptyEmail = String.IsNullOrWhiteSpace(email);
            bool isEmptyPassword = String.IsNullOrWhiteSpace(password);
            string resultMessage = null;

            if (isEmptyEmail || isEmptyPassword)
            {
                string message = (isEmptyEmail == isEmptyPassword) ? "Email and Password cannot be empty." : null;

                if (message == null)
                    message = String.IsNullOrWhiteSpace(email) ? "Email cannot be empty." : "Password cannot be empty.";

                resultMessage = message;

                return (false, resultMessage);
            }

            Dictionary<string, string> fields = new Dictionary<string, string> {
                    { "email", email },
                    { "password", password }};

            try
            {
                string jsonPayload = JsonConvert.SerializeObject(fields, Formatting.Indented);

                HttpResponseMessage response = await _httpClient.PostAsync(_validateUserCredentialsFunctionUri, new StringContent(jsonPayload));
                string responseReason = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                    resultMessage = responseReason;
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    resultMessage = "Encountered an error. Unable to perform credentials validation (press 'Ctrl+C' to exit).";

                    responseReason.LogMessage();

                    return (false, resultMessage);
                }

                return (true, resultMessage);
            }
            catch (Exception ex)
            {
                resultMessage = "Something went wrong! Please check your internet connection (press 'Ctrl+C' to exit).";

                ex.LogException();

                return (false, resultMessage);
            }
        }
    }
}