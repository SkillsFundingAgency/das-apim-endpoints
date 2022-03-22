using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

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
            var apiRequest = new GetApplicationsRequest{Page = request.Page, PageSize = request.PageSize};
            var response = await _levyTransferMatchingApiClient.Get<GetApplicationsResponse>(apiRequest);

            return new GetApplicationsQueryResult
            {
                Page = response.Page,
                PageSize = response.PageSize,
                TotalPages = response.TotalPages,
                TotalApplications = response.TotalApplications,
                Applications = response.Applications.Select(a => new GetApplicationsQueryResult.Application
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
                    NumberOfApprenticesUsed = a.NumberOfApprenticesUsed
                })
            };
        }
    }
}