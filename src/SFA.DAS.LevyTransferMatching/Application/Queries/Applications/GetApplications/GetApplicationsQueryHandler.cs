using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using GetApplicationsRequest = SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests.Applications.GetApplicationsRequest;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications
{
    public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQuery, GetApplicationsResult>
    {
        private readonly ILevyTransferMatchingService _levyTransferMatchingService;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetApplicationsQueryHandler(ILevyTransferMatchingService levyTransferMatchingService, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _levyTransferMatchingService = levyTransferMatchingService;
            _coursesApiClient = coursesApiClient;
        }

        public async Task<GetApplicationsResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            var ltm = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest
            {
                AccountId = request.AccountId
            });

            if (!ltm.Applications.Any())
            {
                return new GetApplicationsResult
                {
                    Applications = new List<Models.Application>()
                };
            }

            var distinctStandards = ltm.Applications.Select(app => app.StandardId).Distinct();
            var standardTasks = new List<Task<GetStandardsListItem>>(distinctStandards.Count());
            
            standardTasks.AddRange(distinctStandards.Select(standardId => _coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(standardId))));

            await Task.WhenAll(standardTasks);

            foreach (var application in ltm.Applications)
            {
                var standard = standardTasks.Select(s => s.Result).Single(s => s.StandardUId == application.StandardId);
                application.Standard = new Standard()
                {
                    LarsCode = standard.LarsCode,
                    Level = standard.Level,
                    StandardUId = standard.StandardUId,
                    Title = standard.Title,
                    ApprenticeshipFunding = standard.ApprenticeshipFunding?.Select(funding =>
                        new ApprenticeshipFunding()
                        {
                            Duration = funding.Duration,
                            EffectiveFrom = funding.EffectiveFrom,
                            EffectiveTo = funding.EffectiveTo,
                            MaxEmployerLevyCap = funding.MaxEmployerLevyCap
                        })
                };
            }

            return new GetApplicationsResult
            {
                Applications = ltm.Applications.ToList()
            };
        }
    }
}
