using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Queries;

public class GetUlnSupportApprenticeshipsQueryHandler(IInternalApiClient<CommitmentsV2ApiConfiguration> client)
    : IRequestHandler<GetUlnSupportApprenticeshipsQuery, GetUlnSupportApprenticeshipsQueryResult?>
{
    public async Task<GetUlnSupportApprenticeshipsQueryResult?> Handle(GetUlnSupportApprenticeshipsQuery request, CancellationToken cancellationToken)
    {
        var apprentices = await client.Get<GetApprovedApprenticeshipsByUlnResponse>(new GetApprovedApprenticeshipsByUlnRequest(request.Uln));

        return new GetUlnSupportApprenticeshipsQueryResult
        {
            ApprovedApprenticeships = apprentices.ApprovedApprenticeships.Select(x => new ApprovedApprenticeshipUlnSummary
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                EmployerAccountId = x.EmployerAccountId,
                ProviderId = x.ProviderId,
                EmployerName = x.EmployerName,
                Uln = x.Uln,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Status = x.PaymentStatus.ToString()
            }).ToList(),
        };
    }

    private static string BuildTrainingDateString(DateTime startDate, DateTime endDate)
    {
        return startDate.ToString("MM/yy") + " to " + endDate.ToString("MM/yy");
    }
}

