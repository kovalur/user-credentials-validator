# user-credentials-validator
Client Console application with RESTful API to validate User Credentials.
## Getting Started
These instructions will walk you through the project up and running on your local machine for development and testing purposes (using live RESTful API). See Deployment for notes on how to deploy a new API on a live system.
### Prerequisites
```
git clone https://github.com/kovalur/user-credentials-validator
cd .\user-credentials-validator\UserCredentialsValidator
```
### Building Client Console application code
```
dotnet restore
dotnet build UserCredentialsValidator.ClientApp --configuration Debug
```
### Running the tests
```
dotnet test --nologo -v n
```
### Usage
#### non-interactive
```
cd .\UserCredentialsValidator.ClientApp\bin\Debug\netcoreapp3.1
.\ClientApp.exe --help
.\ClientApp.exe -e "example@gmail.com" -p "password"
```
#### interactive
```
cd .\UserCredentialsValidator.ClientApp\bin\Debug\netcoreapp3.1
.\ClientApp.exe
```
### Logs
```
tail -n 20 %TEMP%\UserCredentialsValidator.log
```
## Deployment
Additional notes about deploying API.
### Publishing Azure API
* Start Publishing process of UserCredentialsValidator.AzureAPI project using Visual Studio (update Azure API address to any other you like).
* Go to https://portal.azure.com/ and get Azure Storage Account connection string.
* Update AzureWebJobsStorage and AzureWebJobsDashboard strings in local.settings.dev.json file of UserCredentialsValidator.AzureAPI project (replace "UseDevelopmentStorage=true"), rename local.settings.dev.json to local.settings.json.
* Complete Publishing.
* Go to https://portal.azure.com/ and get your own ValidateUserCredentials function key.
* Update AzureAPIBaseURL and ValidateUserCredentialsFunctionKey strings in UserCredentialsValidator.SharedResources project resources.
* Rebuild Client Console application.
### Building Client Console application code
```
dotnet restore
dotnet build UserCredentialsValidator.ClientApp --configuration Release
```
