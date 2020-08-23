using System.Collections.Generic;
using UserCredentialsValidator.ClientApp;
using Xunit;
using Xunit.Abstractions;

namespace UserCredentialsValidator.ClientAppTests
{
    public class UserCredentialsFixture
    {
        public ClientConsoleApplication _clientConsoleApp { get; private set; }

        public UserCredentialsFixture()
        {
            _clientConsoleApp = ClientConsoleApplication.GetInstance();
        }
    }

    public class UserCredentialsTest : IClassFixture<UserCredentialsFixture>
    {
        private UserCredentialsFixture _fixture = null;
        private ITestOutputHelper _output = null;

        public UserCredentialsTest(UserCredentialsFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
        }

        public static IEnumerable<object[]> UserCredentials
        {
            get
            {
                yield return new object[] { "example1@domain.com", "aBcD0915AbCd77", true, "Valid user credentials." };
                yield return new object[] { "example2@domain.com", "abcABC1690~!@#$\"%^&*()_+|.,./?{}:''", true, "Password contains invalid characters." };
                yield return new object[] { "example3@domain", "aBcD0915AbCd77", true, "We are only supporting regular email address domains (e.g 'example@google.com')." };
                yield return new object[] { "example3@", "aBcD0915AbCd77", true, "Invalid email address format." };
                yield return new object[] { "example4", "aBcD0915AbCd77", true, "Invalid email address format." };
                yield return new object[] { "", "aBcD0915AbCd77", false, "Email cannot be empty." };
                yield return new object[] { "example5", "", false, "Password cannot be empty." };
                yield return new object[] { null, "aBcD0915AbCd77", false, "Email cannot be empty." };
                yield return new object[] { "example6", null, false, "Password cannot be empty." };
                yield return new object[] { null, null, false, "Email and Password cannot be empty." };
            }
        }

        [Theory]
        [MemberData(nameof(UserCredentials))]
        public async void TestValidateUserCredentials(string email, string password, bool expectedStatus, string expectedMessage)
        {
            (bool status, string resultMessage) = await _fixture._clientConsoleApp.ValidateUserCredentials(email, password);

            Assert.Equal(expectedStatus, status);
            Assert.Equal(expectedMessage, resultMessage);

            if (status)
            {
                _output.WriteLine($"status[{status}] \"=>\" {resultMessage}");
            }
            else
            {
                _output.WriteLine($"status[{status}] : {resultMessage}");
            }
        }
    }
}