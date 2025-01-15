## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# Payments Outer API

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_apis/build/status/das-apim-endpoints-Apprenticeships?branchName=master)](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_build/latest?definitionId=das-apim-endpoints-Payments&branchName=master)
[![Jira Project](https://img.shields.io/badge/Jira-Project-blue)](https://skillsfundingagency.atlassian.net/jira/software/c/projects/FLP/boards/753)
[![Confluence Project](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/NDL/pages/3480354918/Flexible+Payments+Models)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

The Payments outer provides endpoints used by das-funding-payments and should not be used by any other applications.

## How It Works

The Outer API orchestrates calls to multiple inner APIs in order to provide the functionality required by the UI.  Each UI page operation (GET, POST etc) should call a single outer API endpoint only.

## 🚀 Installation

### Pre-Requisites

* A clone of this repository
* A code editor that supports .Net8
* Azure Storage Emulator (Azureite)

### Config

Most of the application configuration is taken from the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config) and the default values can be used in most cases.  The config json will need to be added to the local Azure Storage instance with a a PartitionKey of LOCAL and a RowKey of SFA.DAS.Payments.OuterAPI_1.0.

| Name                                                          | Description                                       | Stub Value                                |
| ------------------------------------------------------------- | ------------------------------------------------- |-------------------------------------------|
| LearnerDataApiConfiguration:Url                               | Url of the data endpoint                          | https://localhost:4000/learner-data-api   |
| LearnerDataApiConfiguration:TokenSettings:Url                 | Url of the token endpoint                         | http://localhost (if using stub api)      |
| LearnerDataApiConfiguration:TokenSettings:Scope               | Token settings                                    |                                           |
| LearnerDataApiConfiguration:TokenSettings:ClientId            | Token settings                                    |                                           |
| LearnerDataApiConfiguration:TokenSettings:Tenant              | Token settings                                    |                                           |
| LearnerDataApiConfiguration:TokenSettings:ClientSecret        | Token settings                                    |                                           |
| LearnerDataApiConfiguration:TokenSettings:GrantType           | Token settings                                    |                                           |
| LearnerDataApiConfiguration:TokenSettings:ShouldSkipForLocal  | For local use only, skips calling token endpoint  | true                                      |

### Local Running

* Ensure you have the following appsettings.development.json in the API project root:

```
{
  "Environment": "LOCAL",
  "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true"
}
```

* Make sure Azure Storage Emulator is running
* Make sure the config above is in Azure Storage
* If you want to use a stub Learner Data api, select the "API with Stubs" Launch Profile (it'll manage config and start the stubs automatically), otherwise just run SFA.DAS.Payments.API with the https launch profile


## 🔗 External Dependencies

The Outer API has many external dependancies which can all be configured to use stubs by following the config above.