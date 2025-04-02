using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Queries;

public class GetCohortSupportApprenticeshipsQueryHandler(IInternalApiClient<CommitmentsV2ApiConfiguration> client)
    : IRequestHandler<GetCohortSupportApprenticeshipsQuery, GetCohortSupportApprenticeshipsQueryResult?>
{
    public async Task<GetCohortSupportApprenticeshipsQueryResult?> Handle(GetCohortSupportApprenticeshipsQuery request, CancellationToken cancellationToken)
    {
        var cohortTask = client.Get<GetCohortByIdResponse>(new GetCohortByIdRequest(request.CohortId));
        var statusTask = client.Get<GetCohortSupportStatusByIdResponse>(new GetCohortSupportStatusByIdRequest(request.CohortId));
        var apprenticesTask = client.Get<GetApprovedApprenticeshipsByCohortIdResponse>(new GetApprovedApprenticeshipsByCohortIdRequest(request.CohortId));

        await Task.WhenAll(cohortTask, statusTask, apprenticesTask);

        var cohort = await cohortTask;
        var status = await statusTask;
        var apprenticesResponse = await apprenticesTask;

        if (cohort == null || status == null)
        {
            return null;
        }

        return new GetCohortSupportApprenticeshipsQueryResult
        {
            CohortId = cohort.CohortId, 
            CohortReference = cohort.CohortReference,
            EmployerAccountId = cohort.AccountId,
            EmployerAccountName = cohort.LegalEntityName,
            ProviderName = cohort.ProviderName,
            UkPrn = cohort.ProviderId,
            NoOfApprentices = status.NoOfApprentices,
            CohortStatus = status.CohortStatus,
            ApprovedApprenticeships = apprenticesResponse.ApprovedApprenticeships.Select(x => new ApprovedApprenticeshipCohortSummary
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Uln = x.Uln,
                DateOfBirth = x.DateOfBirth,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Status = x.PaymentStatus.ToString()
            }).ToList(),
        };
    }
}

