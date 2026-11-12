# MSA Brokerage Outer API

Outer API for the MSA Brokerage pilot. Mayoral Strategic Authorities (via JUDE) will call it through APIM.

This is the initial project setup so the Azure resources, APIM product and pipeline can be set up. The endpoints will follow.

## Running locally

1. Add a row to local Azure Table Storage (Azurite), table `Configuration`:
   - PartitionKey: `LOCAL`
   - RowKey: `SFA.DAS.MsaBrokerage.OuterApi_1.0`
   - Data: the config for `CandidateApiConfiguration`, `FindApprenticeshipApiConfiguration`, `RecruitAltApiConfiguration`, `CoursesApiConfiguration`, `MsaBrokerageConfiguration` and `AzureAd`.
2. Run `SFA.DAS.MsaBrokerage.Api`. Swagger opens at https://localhost:5291.

## Dependencies

- Candidate Account API
- Find an Apprenticeship API
- Recruit API
- Courses API
- Redis (shared APIM endpoints cache) outside LOCAL/DEV
