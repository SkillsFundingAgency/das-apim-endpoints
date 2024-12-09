using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;
using ApplicationStatus = SFA.DAS.Forecasting.Application.Pledges.Constants.ApplicationStatus;

namespace SFA.DAS.Forecasting.Application.Pledges.Queries.GetApplications;

public class GetApplicationsQueryHandler(ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> levyTransferMatchingApiClient)
    : IRequestHandler<GetApplicationsQuery, GetApplicationsQueryResult>
{
    public async Task<GetApplicationsQueryResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
    {
        var apiRequest = new GetApplicationsRequest{ PledgeId = request.PledgeId};
        var response = await levyTransferMatchingApiClient.Get<GetApplicationsResponse>(apiRequest);

        return new GetApplicationsQueryResult
        {
            Applications = response.Applications
                .Where(application => application.Status == ApplicationStatus.Accepted || application.Status == ApplicationStatus.Approved)
                .Select(application => new GetApplicationsQueryResult.Application
                {
                    Id = application.Id,
                    PledgeId = application.PledgeId,
                    EmployerAccountId = application.EmployerAccountId,
                    StandardId = application.StandardId,
                    StandardTitle = application.StandardTitle,
                    StandardLevel = application.StandardLevel,
                    StandardDuration = application.StandardDuration,
                    StandardMaxFunding = application.StandardMaxFunding,
                    StartDate = application.StartDate,
                    NumberOfApprentices = application.NumberOfApprentices,
                    NumberOfApprenticesUsed = application.NumberOfApprenticesUsed,
                    Status = application.Status
                })
        };
    }
}