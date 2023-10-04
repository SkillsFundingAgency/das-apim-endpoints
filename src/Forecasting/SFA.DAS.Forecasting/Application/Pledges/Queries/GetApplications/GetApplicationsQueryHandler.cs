using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;
using ApplicationStatus = SFA.DAS.Forecasting.Application.Pledges.Constants.ApplicationStatus;

namespace SFA.DAS.Forecasting.Application.Pledges.Queries.GetApplications
{
    public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQuery, GetApplicationsQueryResult>
    {
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _levyTransferMatchingApiClient;

        public GetApplicationsQueryHandler(ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> levyTransferMatchingApiClient)
        {
            _levyTransferMatchingApiClient = levyTransferMatchingApiClient;
        }

        public async Task<GetApplicationsQueryResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            var apiRequest = new GetApplicationsRequest{ PledgeId = request.PledgeId};
            var response = await _levyTransferMatchingApiClient.Get<GetApplicationsResponse>(apiRequest);

            return new GetApplicationsQueryResult
            {
                Applications = response.Applications
                    .Where(a => a.Status == ApplicationStatus.Accepted || a.Status == ApplicationStatus.Approved)
                    .Select(a => new GetApplicationsQueryResult.Application
                {
                    Id = a.Id,
                    PledgeId = a.PledgeId,
                    EmployerAccountId = a.EmployerAccountId,
                    StandardId = a.StandardId,
                    StandardTitle = a.StandardTitle,
                    StandardLevel = a.StandardLevel,
                    StandardDuration = a.StandardDuration,
                    StandardMaxFunding = a.StandardMaxFunding,
                    StartDate = a.StartDate,
                    NumberOfApprentices = a.NumberOfApprentices,
                    NumberOfApprenticesUsed = a.NumberOfApprenticesUsed,
                    Status = a.Status
                })
            };
        }
    }
}