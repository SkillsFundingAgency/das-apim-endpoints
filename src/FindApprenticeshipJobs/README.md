## ?Never push sensitive information such as client id's, secrets or keys into repositories including in the README file?

# APIM FindApprenticeship Jobs

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status%2FAPIM%2Fdas-apim-endpoints-FindApprenticeshipJobs?repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=3499&repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-apim-endpoints_FindApprenticeshipJobs&metric=alert_status)](https://sonarcloud.io/dashboard?id=SkillsFundingAgency_das-apim-endpoints_FindApprenticeshipJobs)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

This Find Apprenticeship Jobs Outer API solution is part of FindAnApprenticeship project. The APIM FindApprenticeship Jobs API is the outer API used to connect the [das-recruit](https://github.com/SkillsFundingAgency/das-recruit) and [das-courses-api](https://github.com/SkillsFundingAgency/das-courses-api) inner APIs, also fetches vacancies from NHS external Api to list them in the FindApprenticeship service.

## Installation

### Pre-Requisites

* A clone of this repository
* A code editor that supports Azure functions and .NetCore 8.0
* An Azure Active Directory account with the appropriate roles as per the [config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apim-developer-api)
* SQL server - Publish the `SFA.DAS.APIM.Developer.Database` project to create the SQL database
* Azure Storage Emulator(https://learn.microsoft.com/en-us/azure/storage/common/storage-use-emulator)

### Local running

The APIM FindApprenticeship Jobs api uses the standard Apprenticeship Service configuration. All configuration can be found in the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apim-endpoints/SFA.DAS.FindApprenticeshipJobs.OuterApi.json).

* appsettings.json file
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
  "ConfigNames": "SFA.DAS.FindApprenticeshipJobs.OuterApi",
  "Environment": "LOCAL",
  "ResourceEnvironmentName": "LOCAL",
  "Version": "1.0",
  "APPINSIGHTS_INSTRUMENTATIONKEY": ""
}
```

You must have the Azure Storage emulator running, and in that a table created called `Configuration` in that table add the following:

Azure Table Storage config

Row Key: SFA.DAS.FindApprenticeshipJobs.OuterApi_1.0

Partition Key: LOCAL

Data:

```json
{
  "CoursesApiConfiguration": {
    "url":"https://localhost:5001/",
    "identifier":"https://{TENANT-NAME}/{IDENTIFIER}",
  },
  "RecruitApiConfiguration": {
    "url": "https://localhost:5040/",
    "identifier": "https://{TENANT-NAME}/{IDENTIFIER}",
  },
  "AzureAd": {
    "Identifier": "https://{TENANT-NAME}/{IDENTIFIER}",
    "Tenant": "{TENANT-NAME}"
  }
  "FindApprenticeshipJobsConfiguration": {
    "ApimEndpointsRedisConnectionString": " "
  },
  "LocationApiConfiguration": {
    "url":"https://localhost:5008/",
    "identifier": "https://{TENANT-NAME}/{IDENTIFIER}",
  },
  "CandidateApiConfiguration": {
    "identifier": "https://{TENANT-NAME}/{IDENTIFIER}",
    "url": "https://localhost:7277/"
  },
  "NServiceBusConfiguration": {
    "NServiceBusConnectionString":"UseLearningEndpoint=true",
    "NServiceBusLicense": "",
  }
}
```

## Technologies

* .NetCore 8.0
* SQL
* Azure Table Storage
* NUnit
* Moq
* FluentAssertions

## How It Works

### Running

* Open command prompt and change directory to _**/src/SFA.DAS.FindApprenticeshipJobs/**_
* Run the web project _**/src/SFA.DAS.FindApprenticeshipJobs/SFA.DAS.FindApprenticeshipJobs.Api.csproj**_

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

### Live Vacancies
https://localhost:7111/api/LiveVacancies - Endpoint to get all live vacancies

https://localhost:7111/api/LiveVacancies/{vacancyReference} - Endpoint to get a live vacancy by given vacancy reference.

https://localhost:7111/api/LiveVacancies/{vacancyReference} - Endpoint to post a vacancy closing soon notification.

https://localhost:7111/api/LiveVacancies/{vacancyReference}/close - Endpoint to close a vacancy.

### NHS Vacancies

https://localhost:7111/api/NhsVacancies - Endpoint to get all live NHS vacancies.

## License

Licensed under the [MIT license](LICENSE)