For building the SFA.DAS.LearnerData,Need to add the nuget source mentioned below.

Name : Package Source ( Any name you wish to add )
source : https://sfa-gov-uk.pkgs.visualstudio.com/_packaging/dct-pkg/nuget/v3/index.json

## Configuration

Configuration is loaded from Azure Table Storage using `ConfigNames` (for example `SFA.DAS.LearnerData.OuterApi`). See `das-employer-config/das-apim-endpoints/SFA.DAS.LearnerData.OuterApi.json` for the full template.

### Redis cache

Non-local environments use shared APIM Redis via `CacheConfiguration`:

```json
"CacheConfiguration": {
  "ApimEndpointsRedisConnectionString": "<redis-connection-string>"
}
```

The pipeline supplies this via `ConfigurationSecrets.ApimEndpointsRedisConnectionString` (mapped through the config schema `environmentVariable`). Local and DEV runs use in-memory distributed cache instead.

`ICacheStorageService` (for example FJAA agencies, 2-hour TTL) and `ILearnerDataCacheService` both use the same `IDistributedCache` registration.