# das-apim-endpoints

[![Build Status](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_apis/build/status/das-apim-endpoints?branchName=master)](https://sfa-gov-uk.visualstudio.com/Digital%20Apprenticeship%20Service/_build/latest?definitionId=2208&branchName=master)

## Requirements

* DotNet Core 2.2 and any supported IDE for DEV running.
* Azure Storage Account
* [das-courses-api](https://github.com/SkillsFundingAgency/das-courses-api)

## About

das-apim-endpoints covers the outer apis that reside within the APIM gateway. Currently this only has the outer api definition for **Find Apprenticeship Training** and relies on **das-courses-api**.

## Local running

You are able to run the API by doing the following:

* In your Azure Storage Account, create a table called Configuration and add the following. Note that the tenant and identifier are not required for local dev.
```
ParitionKey: LOCAL
RowKey: SFA.DAS.FindApprenticeshipTraining.OuterApi_1.0
data: {
  "CoursesApiConfiguration": {
    "tenant":"********.onmicrosoft.com",
    "url":"https://localhost:5001/",
    "identifier":"https://**********.onmicrosoft.com/*******"
  }
}
```
* Start the api project ```SFA.DAS.FindApprenticeshipTraining.Api```

Starting the API will then show the swagger definition with the available operations.