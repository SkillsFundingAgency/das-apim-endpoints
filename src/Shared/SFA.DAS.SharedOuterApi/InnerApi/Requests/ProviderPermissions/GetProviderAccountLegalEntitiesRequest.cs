using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;

public class GetProviderAccountLegalEntitiesRequest : IGetApiRequest﻿
{
    private readonly int? _ukprn;
    private readonly string _operations = "";

    public GetProviderAccountLegalEntitiesRequest(int? ukprn, List<Operation> operations)
    {
        _ukprn = ukprn;
        _operations = string.Join('&', operations.Select(o => $"operations={(int)o}"));
    }

    public string GetUrl => $"accountproviderlegalentities?ukprn={_ukprn}&{_operations}";
}