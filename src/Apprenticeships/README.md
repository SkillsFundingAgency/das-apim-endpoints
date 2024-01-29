## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# Apprenticeships Outer API

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_apis/build/status/das-apim-endpoints-Apprenticeships?branchName=master)](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_build/latest?definitionId=das-apim-endpoints-Apprenticeships&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=_projectId_&metric=alert_status)](https://sonarcloud.io/dashboard?id=_projectId_)
[![Jira Project](https://img.shields.io/badge/Jira-Project-blue)](https://skillsfundingagency.atlassian.net/jira/software/c/projects/FLP/boards/753)
[![Confluence Project](https://img.shields.io/badge/Confluence-Project-blue)](https://skillsfundingagency.atlassian.net/wiki/spaces/NDL/pages/3480354918/Flexible+Payments+Models)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

The Apprenticeships outer provides endpoints used by das-apprenticeships-web and should not be used by any other applications.

## How It Works

The Outer API orchestrates calls to multiple inner APIs in order to provide the functionality required by the UI.  Each UI page operation (GET, POST etc) should call a single outer API endpoint only.

## 🚀 Installation

### Pre-Requisites

* A clone of this repository
* A code editor that supports .Net6
* Azure Storage Emulator (Azureite)

### Config

Most of the application configuration is taken from the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config) and the default values can be used in most cases.  The config json will need to be added to the local Azure Storage instance with a a PartitionKey of LOCAL and a RowKey of SFA.DAS.Apprenticeships.OuterAPI_1.0. To run the application locally the following values need to be updated:

| Name                                       | Value                 |
| ------------------------------------------ | --------------------- |
| ApprenticeshipsApiConfiguration:Url        | http://localhost:5013 |
| ApprenticeshipsApiConfiguration:Identifier | Empty string          |

Additionally, the following settings can be changed should you rather use stub implementations of those APIs:

| Name                                        | Value                                        |
| ------------------------------------------- | -------------------------------------------- |
| CoursesApiConfiguration:Url                 | https://localhost:4000/courses-api/          |
| CoursesApiConfiguration:Identifier          | Empty string                                 |
| AccountsInnerApi:Url                        | http://localhost:3999/accounts-api-v2/       |
| AccountsInnerApi:Identifier                 | Empty string                                 |
| EmployerProfilesApiConfiguration:Url        | http://localhost:3999/employer-profiles-api/ |
| EmployerProfilesApiConfiguration:Identifier | Empty string                                 |

## 🔗 External Dependencies

The Outer API has many external dependancies which can all be configured to use stubs by following the config above.

## Running Locally

* Make sure Azure Storage Emulator (Azureite) is running
* Make sure the config has been updated to call any Stub APIs required
* Run the [Commitments Stubs](https://github.com/SkillsFundingAgency/das-commitments-stubs)
* Run the Apprenticeships Inner API
* Run the application