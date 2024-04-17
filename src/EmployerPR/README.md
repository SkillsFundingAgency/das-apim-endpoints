## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# _Employer PR Outer API_

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

## Build and sonar cloud statuses to be added at a later date, once available


## 🚀 Installation

### Pre-Requisites
* A clone of this repository
* A storage emulator like Azurite

### Dependencies
* Provider Relationships Api: https://github.com/SkillsFundingAgency/das-pr-api
* Accounts Api: https://github.com/SkillsFundingAgency/das-apprentice-accounts-api
* Employer Profiles Api: https://github.com/SkillsFundingAgency/das-employerusers (within folder 'src/EmployerProfiles')


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