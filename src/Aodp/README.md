## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# _AODP Outer API_

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

This AODP Outer API solution is part of AODP project. 

## 🚀 Installation

### Pre-Requisites
* A clone of this repository
* A storage emulator like Azurite

### Dependencies
* AODP Api: https://github.com/SkillsFundingAgency/das-aodp-api

### Config

You can find the latest config file in [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apim-endpoints/SFA.DAS.Aodp.OuterApi.json) repository.

In the SFA.DAS.Aodp.Api project, if not exist already, add appSettings.Development.json file with following content:
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
  "ConfigNames": "SFA.DAS.Aodp.OuterApi",
  "Environment": "LOCAL",
  "Version": "1.0",
  "AodpInnerApiConfiguration": {
    "url": "https://localhost:7026/",
    "identifier": ""
  }
}
```

## Technologies
* .Net 8.0
* Azure Table Storage
* NUnit
* Moq
