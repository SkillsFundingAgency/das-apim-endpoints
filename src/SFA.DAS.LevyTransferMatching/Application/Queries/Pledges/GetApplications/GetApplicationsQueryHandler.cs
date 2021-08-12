using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications
{
    public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQuery, GetApplicationsQueryResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetApplicationsQueryHandler(ILevyTransferMatchingService levyTransferMatchingService, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _coursesApiClient = coursesApiClient;
        }

        public async Task<GetApplicationsQueryResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            var applicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest(request.PledgeId));
            var standardResponse = await _coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(applicationsResponse.Applications.First().StandardId));

            return new GetApplicationsQueryResult()
            {
                Applications = applicationsResponse.Applications,
                Standard = new Standard()
                {
                    LarsCode = standardResponse.LarsCode,
                    Level = standardResponse.Level,
                    StandardUId = standardResponse.StandardUId,
                    Title = standardResponse.Title,
                    ApprenticeshipFunding = standardResponse.ApprenticeshipFunding?.Select(funding => new ApprenticeshipFunding()
                    {
                        Duration = funding.Duration,
                        EffectiveFrom = funding.EffectiveFrom,
                        EffectiveTo = funding.EffectiveTo,
                        MaxEmployerLevyCap = funding.MaxEmployerLevyCap
                    })
                }
            };
        }
    }
}