﻿## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# das-apim-endpoints/VacanciesManage

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status%2FAPIM%2Fdas-apim-endpoints-VacanciesManage?repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=2882&repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=master)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-apim-endpoints_VacanciesManage&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-apim-endpoints_VacanciesManage)

[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

## About

The [SFA.DAS-VacanciesManage-Api](https://github.com/SkillsFundingAgency/das-apim-endpoints/tree/master/src/VacanciesManage) covers the outer apis that reside within the APIM gateway. All outer APIs should act as an aggregation layer between the inner apis, having only the necessary logic to form the data to be presented back to the consumer. It should be seen that each outer API is built at a service layer, so it is expected that a function and site of the same service could consume, but not multiple services consuming a single outer API.

## 🚀 Installation

### Pre-Requisites
* A clone of this repository(https://github.com/SkillsFundingAgency/das-apim-endpoints)
* A code editor that supports .NetCore 8 and above
* A storage emulator like Azurite (https://learn.microsoft.com/en-us/azure/storage/common/storage-use-emulator)
* An Azure Active Directory account with the appropriate roles as per the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apim-endpoints/SFA.DAS.Vacancies.Manage.OuterApi.json)

### Config
You can find the latest config file in [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apim-endpoints/SFA.DAS.Vacancies.Manage.OuterApi.json)

* If you are using Azure Storage Emulator for local development purpose, then In your Azure Storage Account, create a table called Configuration and Add the following

ParitionKey: LOCAL
RowKey: SFA.DAS.Vacancies.Manage.Outer.Api.json
Data:
```json
{
  "RecruitApiConfiguration": {
    "url": "https://localhost:5040/",
    "identifier": "https://****.onmicrosoft.com/******"
  },
  "AzureAd": {
    "tenant": "citizenazuresfabisgov.onmicrosoft.com",
    "identifier": "https://****.onmicrosoft.com/******"
  },
  "AccountsInnerApi": {
    "url": "https://localhost:5011/",
    "identifier": "https://****.onmicrosoft.com/******"
  },
  "ProviderRelationshipsApi": {
    "url": "https://{{service-endpoint}}.service.gov.uk",
    "identifier": "https://****.onmicrosoft.com/******"
  },
  "CoursesApiConfiguration": {
    "url": "https://localhost:5001/",
    "identifier": "https://****.onmicrosoft.com/******"
  },
  "RoatpV2ApiConfiguration": {
    "url": "https://localhost:5111/",
    "identifier": "https://****.onmicrosoft.com/******"
  },
  "VacanciesManageConfiguration": {
    "ApimEndpointsRedisConnectionString": " "
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
  "ConfigNames": "SFA.DAS.Vacancies.Manage.OuterApi",
  "Environment": "LOCAL",
  "Version": "1.0",
  "APPINSIGHTS_INSTRUMENTATIONKEY": ""
}
```

## Technologies
* .NetCore 8.0
* Azure Storage Account
* NUnit
* NLog
* Moq
* FluentAssertions
* Azure App Insights
* MediatR

## How It Works

### Running

* Open command prompt and change directory to _**/src/VacanciesManage/**_
* Run the web project _**/src/VacanciesManage/SFA.DAS.VacanciesManage.Api.csproj**_

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

### Vacancy

https://localhost:5022/api/vacancy - Endpoint to creates an apprenticeship vacancy using the specified values.

https://localhost:5022/api/vacancy/{vacancyReference} - Endpoint to GET vacancy by reference number and returns details of a specific vacancy. If no vacancy found then a 404 response is returned.

### Reference Data

https://localhost:5022/api/ReferenceData/qualifications - Endpoint to retrieve GET list of qualifications.

https://localhost:5022/api/ReferenceData/skills - Endpoint to retrieve GET list of skills.

https://localhost:5022/api/ReferenceData/courses - Endpoint to retrieve GET list of training courses.

### Account Legal Entities

https://localhost:5022/api/AccountLegalEntities - Endpoint to get a list of Account Legal Entities that are connected to your subscription. The AccountLegalEntityPublicHashedId is required for Vacancy creation. If you are a provider only Accounts that have given permission for you to act on there behalf will show in the list. If you are an employer then only legal entities that have a signed agreement will be in the list.

## License

Licensed under the [MIT license](LICENSE)