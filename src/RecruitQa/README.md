﻿## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# das-apim-endpoints/Recruit

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status%2FAPIM%2Fdas-apim-endpoints-Recruit?repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build?definitionId=2877)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-apim-endpoints_Recruit&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-apim-endpoints_Recruit)

[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

## About

[SFA.DAS.Recruit.Api](https://github.com/SkillsFundingAgency/das-apim-endpoints/tree/master/src/RecruitQa)
is the outer API interface into Recruit functionality.

All outer APIs should act as an aggregation layer between the inner apis, having only the necessary logic to form the data to be presented back to the consumer. It should be seen that each outer API is built at a service layer, so it is expected that a function and site of the same service could consume, but not multiple services consuming a single outer API.

## 🚀 Installation

### Pre-Requisites
* [a clone of this repository](https://github.com/SkillsFundingAgency/das-apim-endpoints)
* .Net Core 8
* an appropriate IDE for development
* a storage emulator like [Azurite](https://learn.microsoft.com/en-us/azure/storage/common/storage-use-emulator)

### Config
You can find the latest config file in [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apim-endpoints/SFA.DAS.RecruitQa.OuterApi.json)

* If you are using Azure Storage Emulator for local development purpose, then In your Azure Storage Account, create a table called Configuration and Add the following

```
Partition Key: LOCAL
Row Key: SFA.DAS.Recruit.Outer.Api.json
Data:
```
```json
{  
  "AzureAd": {
    "tenant": "****.onmicrosoft.com",
    "identifier": "https://****.onmicrosoft.com/******"
  },  
  "RecruitConfiguration": {
    "url": "https://localhost:7226/",
    "identifier": "https://****.onmicrosoft.com/******"
  },
  "NServiceBusConfiguration": {
    "NServiceBusConnectionString":"UseLearningEndpoint=true",
    "NServiceBusLicense": "**** add development license ****"
  }
}
```

## How It Works

### Running

* Open command prompt and change directory to _**/src/RecruitQa/**_
* Run the web project _**/src/RecruitQa/SFA.DAS.RecruitQa.Api.csproj**_

MacOS
```
ASPNETCORE_ENVIRONMENT=Development
dotnet run
```
Windows cmd
```
set ASPNETCORE_ENVIRONMENT=Development
dotnet run
```

### Application logs
Application logs are logged to [Application Insights](https://learn.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) and can be viewed using [Azure Monitor](https://learn.microsoft.com/en-us/azure/azure-monitor/overview) at https://portal.azure.com

## License

Licensed under the [MIT license](LICENSE)