﻿## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# das-apim-endpoints/EmployerProfiles

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status%2FAPIM%2Fdas-apim-endpoints-EmployerProfiles?repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=3119&repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=master)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-apim-endpoints_EmployerProfiles&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-apim-endpoints_EmployerProfiles)

[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

## About

The [SFA.DAS-EmployerProfiles-Api](https://github.com/SkillsFundingAgency/das-apim-endpoints/tree/43f8822b49e4ea17a326b1809229d3fd4039eb5f/src/EmployerProfiles) covers the outer apis that reside within the APIM gateway. All outer APIs should act as an aggregation layer between the inner apis, having only the necessary logic to form the data to be presented back to the consumer. It should be seen that each outer API is built at a service layer, so it is expected that a function and site of the same service could consume, but not multiple services consuming a single outer API.

## 🚀 Installation

### Pre-Requisites
* A clone of this repository(https://github.com/SkillsFundingAgency/das-apim-endpoints)
* A code editor that supports .NetCore 8 and above
* A storage emulator like Azurite (https://learn.microsoft.com/en-us/azure/storage/common/storage-use-emulator)
* An Azure Active Directory account with the appropriate roles as per the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apim-endpoints/SFA.DAS.EmployerProfiles.OuterApi.json)

### Config
You can find the latest config file in [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apim-endpoints/SFA.DAS.EmployerProfiles.OuterApi.json)

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
  "EmployerProfilesApiConfiguration": {
    "url": "https://localhost:5001/",
    "identifier": "https://****.onmicrosoft.com/******"
  },
   "AzureAd": {
    "tenant": "********.onmicrosoft.com",
    "identifier": "https://********.onmicrosoft.com/******"
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
  "ConfigNames": "SFA.DAS.EmployerProfiles.OuterApi",
  "Environment": "LOCAL",
  "Version": "1.0",
  "APPINSIGHTS_INSTRUMENTATIONKEY": ""
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

* Open command prompt and change directory to _**/src/EmployerProfiles/**_
* Run the web project _**/src/EmployerProfiles/SFA.DAS.EmployerProfiles.Api.csproj**_

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

### Accounts

https://localhost:5001/api/AccountUsers/{userId}/accounts - Endpoint to retrieve the user accounts based on the user ID and email.

https://localhost:5001/api/AccountUsers/{userId}/upsert-user - Endpoint to add & update the employer information.


## License

Licensed under the [MIT license](LICENSE)