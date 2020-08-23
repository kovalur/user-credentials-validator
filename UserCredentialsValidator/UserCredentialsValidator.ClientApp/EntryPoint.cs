using CommandLine;
using System;
using System.Linq;

namespace UserCredentialsValidator.ClientApp
{
    internal class Options
    {
        [Option('e', "email", Required = false, HelpText = "Email to validate.")]
        public string Email { get; set; }

        [Option('p', "password", Required = false, HelpText = "Password to validate.")]
        public string Password { get; set; }
    }

    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            string email = null;
            string password = null;
            int errorCount = 0;

            ParserResult<Options> options = Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(o =>
            {
                email = o.Email;
                password = o.Password;
            }).WithNotParsed(e =>
            {
                errorCount = e.Count();
            });

            if (email == "''")
                email = email.Replace("''", "");

            if (password == "''")
                password = password.Replace("''", "");

            if (email == String.Empty || password == String.Empty)
            {
                string message = (email == password) ? "Email and Password cannot be empty." : null;

                if (message == null)
                    message = (email == String.Empty) ? "Email cannot be empty." : "Password cannot be empty.";

                Console.WriteLine(message);

                return;
            }
            else if ((email == null && password != null) || (password == null && email != null))
            {
                Console.WriteLine("Not all parameters supplied.");

                return;
            }

            if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(password))
            {
                (bool status, string resultMessage) = ClientConsoleApplication.GetInstance().ValidateUserCredentials(email, password).Result;

                if (status)
                {
                    Console.WriteLine($"\"=>\" {resultMessage}");
                }
                else
                {
                    Console.WriteLine(resultMessage);
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();

                return;
            }
            else if (errorCount == 0)
            {
                Console.WriteLine("Starting in interactive mode...");

                ClientConsoleApplication.GetInstance().StartCyclingValidation().Wait();
            }
        }
    }
}