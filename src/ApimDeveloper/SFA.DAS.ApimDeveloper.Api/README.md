## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# das-apimdeveloper-api

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status%2FAPIM%2Fdas-apim-endpoints-ApimDeveloper?repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=refs%2Fpull%2F1661%2Fmerge)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=2860&repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=refs%2Fpull%2F1661%2Fmerge)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-apim-endpoints_ApimDeveloper&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-apim-endpoints_ApimDeveloper)

[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

## About

The [SFA.DAS-ApimDeveloper-Api](https://github.com/SkillsFundingAgency/das-apim-endpoints/tree/029667d9f1023409a610019a2b89833bc56adfbb/src/ApimDeveloper) covers the outer apis that reside within the APIM gateway. All outer APIs should act as an aggregation layer between the inner apis, having only the necessary logic to form the data to be presented back to the consumer. It should be seen that each outer API is built at a service layer, so it is expected that a function and site of the same service could consume, but not multiple services consuming a single outer API.

## 🚀 Installation

### Pre-Requisites
* A clone of this repository(https://github.com/SkillsFundingAgency/das-apim-endpoints)
* A code editor that supports .NetCore 8 and above
* A storage emulator like Azurite (https://learn.microsoft.com/en-us/azure/storage/common/storage-use-emulator)
* An Azure Active Directory account with the appropriate roles as per the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apim-endpoints/SFA.DAS.ApimDeveloper.OuterApi.json)

### Config
You can find the latest config file in [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apim-endpoints/SFA.DAS.ApimDeveloper.OuterApi.json)

* If you are using Azure Storage Emulator for local development purpose, then In your Azure Storage Account, create a table called Configuration and Add the following

ParitionKey: LOCAL
RowKey: SFA.DAS.ApimDeveloper.Outer.Api.json
Data:
```json
{
  "AccountsInnerApi": {
    "url": "https://localhost:5011/",
    "identifier": "https://****.onmicrosoft.com/******"
  },
  "ApimDeveloperApiConfiguration": {
    "url": "https://localhost:5001/",
    "identifier": "https://****.onmicrosoft.com/******"
  },
  "EmployerProfilesApiConfiguration": {
    "url": "https://localhost:5001/",
    "identifier": "https://****.onmicrosoft.com/******"
  },
  "EmployerUsersApiConfiguration": {
    "url": "https://localhost:5001/",
    "identifier": "https://****.onmicrosoft.com/******"
  },
  "AzureAd": {
    "tenant": "****.onmicrosoft.com",
    "identifier": "https://****.onmicrosoft.com/******"
  },
  "ApimDeveloperMessagingConfiguration": {
    "VerifyThirdPartyAccountTemplateId": "{{Guid}}",
    "ChangePasswordTemplateId": "{{Guid}}"
  },
  "ApimDeveloperConfiguration": {
    "ApimEndpointsRedisConnectionString": " "
  },
  "NServiceBusConfiguration": {
    "NServiceBusConnectionString": "UseLearningEndpoint=true",
    "NServiceBusLicense": "{{SERVICE-BUS-CONNECTION-STRING}}",
  },
  "RoatpV2ApiConfiguration": {
    "url": "https://localhost:5001/",
    "identifier": "https://****.onmicrosoft.com/******"
  }
}
```

In the web project, if it does not exist already, add `AppSettings.Development.json` file with the following content:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
  "ConfigNames": "SFA.DAS.ApimDeveloper.OuterApi",
  "Environment": "LOCAL",
  "Version": "1.0",
  "APPLICATIONINSIGHTS_CONNECTION_STRING": ""
}
```

## Technologies
* .NetCore 8.0
* Azure Storage Account
* NUnit
* Moq
* FluentAssertions
* Azure App Insights
* MediatR

## How It Works

### Running

* Open command prompt and change directory to _**/src/ApimDeveloper/**_
* Run the web project _**/src/ApimDeveloper/SFA.DAS.ApimDeveloper.Api.csproj**_

MacOS
```
ASPNETCORE_ENVIRONMENT=Development dotnet run
```
Windows cmd
```
set ASPNETCORE_ENVIRONMENT=Development
dotnet run
```

### Application logs
Application logs are logged to [Application Insights](https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) and can be viewed using [Azure Monitor](https://learn.microsoft.com/en-us/azure/azure-monitor/overview) at https://portal.azure.com

## Useful URLs

### Products
https://localhost:5001/api/products - Endpoint to get API products are retrieved by UserType

### Subscription

https://localhost:5001/api/subscriptions/products - Endpoint to get all user's product subscriptions

https://localhost:5001/api/subscriptions/{id}/products - Endpoint to get user's product subscription by Id

### User

https://localhost:5001/api/users/{email} - Endpoint to get user information

https://localhost:5001/api/users/authenticate - Endpoint to validate user credentials

## License

Licensed under the [MIT license](LICENSE)