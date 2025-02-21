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
        var cohortTask = client.Get<GetCohortByIdResponse>(new GetCohortByIdRequest(request.CohortId));
        var statusTask = client.Get<GetCohortSupportStatusByIdResponse>(new GetCohortSupportStatusByIdRequest(request.CohortId));

        await Task.WhenAll(cohortTask, statusTask);

        var cohort = await cohortTask;
        var status = await statusTask;

        if (cohort == null)
        {
            return null;
        }

        return new GetCohortAndSupportStatusQueryResult
        {
            CohortId = cohort.CohortId, 
            CohortReference = cohort.CohortReference,
            EmployerAccountName = cohort.LegalEntityName,
            ProviderName = cohort.ProviderName,
            UkPrn = cohort.ProviderId,
            NoOfApprentices = status.NoOfApprentices,
            CohortStatus = status.CohortStatus
        };
    }
}

