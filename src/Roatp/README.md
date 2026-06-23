# _Roatp Outer API_

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_apis/build/status%2FAPIM%2Fdas-apim-endpoints-Roatp?repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=master)](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_build/latest?definitionId=2879&repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-apim-endpoints_Roatp&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-apim-endpoints_Roatp)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

## 🚀 Installation

### Pre-Requisites
* A clone of this repository
* A storage emulator like Azurite
* Visual studio or similar IDE 

### Dependencies
* Charities Api: [das-charities-api](https://github.com/SkillsFundingAgency/das-charities-api)
* Roatp Api: [das-roatp-service](https://github.com/SkillsFundingAgency/das-roatp-service)


### Config

You can find the latest config file in [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apim-endpoints/SFA.DAS.RoATP.OuterApi.json) repository.

In the SFA.DAS.Roatp.API project, if not exist already, add appSettings.Development.json file with following content:
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
  "ConfigNames": "SFA.DAS.RoATP.OuterApi",
  "Environment": "LOCAL",
  "Version": "1.0",
  "APPINSIGHTS_INSTRUMENTATIONKEY": ""
}
```

## Technologies
* .Net 10.0
* Azure Table Storage
* NUnit
* Moq
* FluentAssertions