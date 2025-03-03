﻿## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# _Apprentice AAN Outer API_

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status%2FAPIM%2Fdas-apim-endpoints-ApprenticeAan?repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=refs%2Fpull%2F1335%2Fmerge)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=3012&repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=refs%2Fpull%2F1335%2Fmerge)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-apim-endpoints_ApprenticeAan&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-apim-endpoints_ApprenticeAan)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

This Apprentice AAN Outer API solution is part of Apprentice Ambassador Network (AAN) project. 

## 🚀 Installation

### Pre-Requisites
* A clone of this repository
* A storage emulator like Azurite

### Dependencies
* AAN Hub Api: https://github.com/SkillsFundingAgency/das-aan-hub-api
* Location Api: https://github.com/SkillsFundingAgency/das-location-api
* Apprentice Accounts Api: https://github.com/SkillsFundingAgency/das-apprentice-accounts-api
* Courses Api: https://github.com/SkillsFundingAgency/das-courses-api
* Commitments V2 Api (see notes for V2): https://github.com/SkillsFundingAgency/das-commitments 
  
### Config

You can find the latest config file in [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apim-endpoints/SFA.DAS.ApprenticeAan.OuterApi.json) repository.

In the SFA.DAS.ApprenticeAan.API project, if not exist already, add appSettings.Development.json file with following content:
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
  "ConfigNames": "SFA.DAS.ApprenticeAan.OuterApi",
  "Environment": "LOCAL",
  "Version": "1.0"
}

```

## Technologies
* .Net 8.0
* Azure Table Storage
* NUnit
* Moq
* FluentAssertions
