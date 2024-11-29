## ⛔Never push sensitive information such as client id's, secrets or keys into repositories including in the README file⛔

# das-apimFindApprenticeshipTraining-api

<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status%2FAPIM%2Fdas-apim-endpoints-FindApprenticeshipTraining?repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=refs%2Fpull%2F1550%2Fmerge)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=2872&repoName=SkillsFundingAgency%2Fdas-apim-endpoints&branchName=refs%2Fpull%2F1550%2Fmerge)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-apim-endpoints_FindApprenticeshipTraining&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-apim-endpoints_FindApprenticeshipTraining)

[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://en.wikipedia.org/wiki/MIT_License)

## About

The [SFA.DAS-FindApprenticeshipTraining-Api](https://github.com/SkillsFundingAgency/das-apim-endpoints/tree/029667d9f1023409a610019a2b89833bc56adfbb/src/FindApprenticeshipTraining) covers the outer apis that reside within the APIM gateway. All outer APIs should act as an aggregation layer between the inner apis, having only the necessary logic to form the data to be presented back to the consumer. It should be seen that each outer API is built at a service layer, so it is expected that a function and site of the same service could consume, but not multiple services consuming a single outer API.

## 🚀 Installation

### Pre-Requisites
* A clone of this repository(https://github.com/SkillsFundingAgency/das-apim-endpoints)
* A code editor that supports .NetCore 8 and above
* A storage emulator like Azurite (https://learn.microsoft.com/en-us/azure/storage/common/storage-use-emulator)
* An Azure Active Directory account with the appropriate roles as per the [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apim-endpoints/SFA.DAS.FindApprenticeshipTraining.OuterApi.json)

### Config
You can find the latest config file in [das-employer-config repository](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apim-endpoints/SFA.DAS.FindApprenticeshipTraining.OuterApi.json)

In the SFA.DAS.FindApprenticeshipTraining.Api.API project, if it does not exist already, add appSettings.Development.json file with following content:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
  "ConfigNames": "SFA.DAS.FindApprenticeshipTraining.OuterApi",
  "Environment": "LOCAL",
  "Version": "1.0", 
  "APPINSIGHTS_INSTRUMENTATIONKEY": ""
}
```

* If you are using Azure Storage Emulator for local development purpose, then In your Azure Storage Account, create a table called Configuration and Add the following

ParitionKey: LOCAL
RowKey: SFA.DAS.FindApprenticeshipTraining.Outer.Api.json
Data:
```json
{
  "CoursesApiConfiguration": {
    "url":"https://localhost:5001/",
    "identifier":"https://******.onmicrosoft.com/******"
  },
  "LocationApiConfiguration": {
    "url":"https://localhost:5008/",
    "identifier":"https://******.onmicrosoft.com/******"
  },
  "ApprenticeFeedbackApiConfiguration": {
    "url": "https://localhost:5601/",
    "identifier":"https://******.onmicrosoft.com/******"
  },
  "EmployerFeedbackApiConfiguration": {
    "url": "https://localhost:5301/",
    "identifier":"https://******.onmicrosoft.com/******"
  },
  "AzureAd": {
    "tenant": "***.onmicrosoft.com",
    "identifier":"https://******.onmicrosoft.com/******"
  },
  "FindApprenticeshipTrainingConfiguration": {
    "ApimEndpointsRedisConnectionString": " "
  },
  "ShortlistApiConfiguration": {
    "url": "https://localhost:7047/",
    "identifier":"https://******.onmicrosoft.com/******"
  },
  "RoatpV2ApiConfiguration": {
    "url": "https://localhost:5111/",
    "identifier":"https://******.onmicrosoft.com/******"
  }
}
```

In the web project, if it does not exist already, add `AppSettings.Development.json` file with the following content:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true;",
  "ConfigNames": "SFA.DAS.FindApprenticeshipTraining.OuterApi",
  "Environment": "LOCAL",
  "Version": "1.0", 
  "APPINSIGHTS_INSTRUMENTATIONKEY": ""
}
```

## Technologies
* .NetCore 8.0
* Azure Storage Account
* NUnit
* Moq
* FluentAssertions
* Azure App Insights
* MediatR

## How It Works

### Running

* Open command prompt and change directory to _**/src/FindApprenticeshipTraining/**_
* Run the web project _**/src/FindApprenticeshipTraining/SFA.DAS.FindApprenticeshipTraining.Api.csproj**_

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

### Locations

https://localhost:5003/api/Locations - Endpoint to get search response by given searchTerm

### Training Courses

https://localhost:5003/api/TrainingCourses/{id}/providers - This endpoint is a GET request that retrieves a list of training course providers for a specific training course. It takes in the id of the training course as a path parameter and accepts additional query parameters for filtering and sorting the results.

https://localhost:5001/api/TrainingCourses/{id}/providers/{providerId} - This endpoint is responsible for retrieving information about a specific training course provider.

### Short List

*	GetAllForUser(https://localhost:5003/api/users/{userId}): This endpoint is a GET request that retrieves all the shortlisted training courses for a specific user. It takes a userId as a parameter and returns a list of shortlisted courses in the response.
*	GetExpiredShortlistUserIds(https://localhost:5003/api/users/expired): This endpoint is a GET request that retrieves the user IDs of all expired shortlists. It takes an expiryInDays parameter to specify the number of days after which a shortlist is considered expired. The response contains a list of user IDs.
* 	CreateShortlistForUser(https://localhost:5003/api/users): This endpoint is a POST request that creates a new shortlist item for a user. It takes a shortlistRequest object as a parameter, which includes information such as the user ID, location, and training course details. If the shortlist item is created successfully, it returns a 201 Created status code.
*	DeleteShortlistItemForUser(https://localhost:5003/api/users/{userId}/items/{id}): This endpoint is a DELETE request that deletes a specific shortlist item for a user. It takes the id of the shortlist item and the userId as parameters. If the deletion is successful, it returns a 202 Accepted status code.
*	DeleteShortlistForUser(https://localhost:5003/api/users/{userId}): This endpoint is a DELETE request that deletes the entire shortlist for a user. It takes the userId as a parameter. If the deletion is successful, it returns a 202 Accepted status code.

## License

Licensed under the [MIT license](LICENSE)