using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.EmploymentCheckApi.Requests;
using SFA.DAS.Approvals.InnerApi.EmploymentCheckApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.Application.EmploymentChecks.Queries.GetEmploymentChecksQuery;

public class GetEmploymentChecksQueryHandler(IEmploymentCheckApiClient<EmploymentCheckConfiguration> client)
    : IRequestHandler<GetEmploymentChecksQuery, GetEmploymentChecksResult>
{
    public async Task<GetEmploymentChecksResult> Handle(GetEmploymentChecksQuery request, CancellationToken cancellationToken)
    {
        var apiRequest = new GetEmploymentCheckLearnersRequest(request.ApprenticeshipIds);
        var response = await client.GetWithResponseCode<GetEmploymentChecksResponse>(apiRequest);

        if (!response.StatusCode.IsSuccessStatusCode())
        {
            throw new InvalidOperationException($"Employment check API error: {response.ErrorContent}");
        }

        return new GetEmploymentChecksResult
        {
            Checks = response.Body?.Checks ?? []
        };
    }
}
