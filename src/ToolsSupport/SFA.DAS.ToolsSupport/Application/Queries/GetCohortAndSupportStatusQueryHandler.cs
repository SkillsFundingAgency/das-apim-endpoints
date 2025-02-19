using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Queries;

public class GetCohortAndSupportStatusQueryHandler(IInternalApiClient<CommitmentsV2ApiConfiguration> client)
    : IRequestHandler<GetCohortAndSupportStatusQuery, GetCohortAndSupportStatusQueryResult?>
{
    public async Task<GetCohortAndSupportStatusQueryResult?> Handle(GetCohortAndSupportStatusQuery request, CancellationToken cancellationToken)
    {
        var response = await client.Get<GetCohortByIdResponse>(new GetCohortByIdRequest(request.CohortId));
        var status = await client.Get<GetCohortSupportStatusByIdResponse>(new GetCohortSupportStatusByIdRequest(request.CohortId));

        if (response == null || status == null)
        {
            return null;
        }

        return new GetCohortAndSupportStatusQueryResult
        {
            CohortId = response.CohortId, 
            CohortReference = response.CohortReference,
            EmployerAccountName = response.LegalEntityName,
            ProviderName = response.ProviderName,
            UkPrn = response.ProviderId,
            NoOfApprentices = status.NoOfApprentices,
            CohortStatus = status.CohortStatus
        };
    }
}

