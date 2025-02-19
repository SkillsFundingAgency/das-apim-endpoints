## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# _Employer PR Outer API_

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status%2Fdas-apim-endpoints-EmployerPR?repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=refs%2Fpull%2F1939%2Fmerge)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=3696&repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=refs%2Fpull%2F1939%2Fmerge)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-apim-endpoints_EmployerPR&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-apim-endpoints_EmployerPR)

## 🚀 Installation

### Pre-Requisites
* A clone of this repository
* A storage emulator like Azurite

### Dependencies
* [Provider Relationships API](https://github.com/SkillsFundingAgency/das-pr-api)
* [Accounts API](https://github.com/SkillsFundingAgency/das-apprentice-accounts-api)
* [Employer Profiles API](https://github.com/SkillsFundingAgency/das-employerusers) (within folder 'src/EmployerProfiles')
* [Roatp V2 API](https://github.com/SkillsFundingAgency/das-roatp-api) 
* [Pensions Regulator API](https://github.com/SkillsFundingAgency/das-pensionsregulator)


### Config

You can find the latest config file in [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apim-endpoints/SFA.DAS.EmployerPR.OuterApi.json) repository.

In the SFA.DAS.EmployerPR.API project, if not exist already, add appSettings.Development.json file with following content:
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
  "ConfigNames": "SFA.DAS.EmployerPR.OuterApi",
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