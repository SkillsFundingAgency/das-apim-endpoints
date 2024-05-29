﻿using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRelationships;

public class GetEmployerRelationshipsResponse
{
    public GetEmployerRelationshipsResponse()
    {

    }
    public List<AccountLegalEntityPermissionsResponseModel> AccountLegalEntities { get; set; } = [];
}

public class AccountLegalEntityPermissionsResponseModel
{
    public required long Id { get; set; }

    public required string PublicHashedId { get; set; }

    public required string Name { get; set; }

    public required long AccountId { get; set; }

    public List<ProviderPermissionsResponseModel> Permissions { get; set; } = [];
}

public class ProviderPermissionsResponseModel
{
    public required long Ukprn { get; set; }

    public required string ProviderName { get; set; }

    public Operation[] Operations { get; set; } = [];
}

