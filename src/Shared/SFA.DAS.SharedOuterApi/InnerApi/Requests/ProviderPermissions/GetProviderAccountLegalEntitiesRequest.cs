using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;

public class GetProviderAccountLegalEntitiesRequest : IGetApiRequest﻿
{
    private readonly int? _ukprn;
    private readonly string _operations = "";

    public GetProviderAccountLegalEntitiesRequest(int? ukprn, List<Operation> operations)
    {
        _ukprn = ukprn;
        if (!operations.IsNullOrEmpty())
        {
            foreach (Operation operation in operations)
                this._operations += $"&operations={(int)operation}";
        }
        else
            _operations = "&operations=";
    }

    public string GetUrl => $"accountproviderlegalentities?ukprn={_ukprn}{_operations}";
}