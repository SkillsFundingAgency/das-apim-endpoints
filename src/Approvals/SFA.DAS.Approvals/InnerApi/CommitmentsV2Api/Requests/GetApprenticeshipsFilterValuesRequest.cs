using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;

public class GetApprenticeshipsFilterValuesRequest(long? providerId, long? employerAccountId) : IGetApiRequest
{
    public long? EmployerAccountId { get; set; } = employerAccountId;

    public long? ProviderId { get; set; } = providerId;

    public string GetUrl
    {
        get
        {
            var queryParameters = new Dictionary<string, string>();

            if (EmployerAccountId.HasValue)
            {
                queryParameters.Add("employerAccountId", EmployerAccountId.Value.ToString());
            }

            if (ProviderId.HasValue)
            {
                queryParameters.Add("providerId", ProviderId.Value.ToString());
            }

            return QueryHelpers.AddQueryString("api/apprenticeships/filters", queryParameters);
        }
    }
}