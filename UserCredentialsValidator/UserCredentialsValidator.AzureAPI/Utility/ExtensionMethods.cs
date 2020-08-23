using System.Text.RegularExpressions;

namespace UserCredentialsValidator.AzureAPI.Utility
{
    internal static class ExtensionMethods
    {
        private static readonly Regex _regexPattern = new Regex("^[a-zA-Z0-9]*$");

        public static bool ContainsOnlyAlphanumericCharacters(this string str)
        {
            return _regexPattern.IsMatch(str);
        }
        public static bool IsRegularMailAddressDomain(this string email)
        {
            return email.Split('.').Length == 2;
        }
    }
}