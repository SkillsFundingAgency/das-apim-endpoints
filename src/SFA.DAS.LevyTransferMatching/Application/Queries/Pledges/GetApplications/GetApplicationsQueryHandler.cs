using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.LevyTransferMatching.Application.Queries.Shared.GetApplications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplications
{
    public class GetApplicationsQueryHandler : GetApplicationsQueryHandlerBase<GetApplicationsQuery, GetApplicationsQueryResult>
    {
        public GetApplicationsQueryHandler(ILevyTransferMatchingService levyTransferMatchingService, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient) : 
            base(levyTransferMatchingService, coursesApiClient)
        {
        }

        public override async Task<GetApplicationsQueryResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            var applicationsResponse = await _levyTransferMatchingService.GetApplications(new GetApplicationsRequest
            {
                PledgeId = request?.PledgeId,
                AccountId = request?.AccountId
            });

            if (applicationsResponse.Applications == null)
            {
                return new GetApplicationsQueryResult()
                {
                    Applications = null
                };
            }

            var pledgeResponse = await _levyTransferMatchingService.GetPledge(request.PledgeId.Value);
            var distinctStandards = applicationsResponse.Applications.Select(app => app.StandardId).Distinct();
            var standardTasks = new List<Task<GetStandardsListItem>>(distinctStandards.Count());

            standardTasks.AddRange(distinctStandards.Select(standardId => _coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(standardId))));

            await Task.WhenAll(standardTasks);

            foreach (var application in applicationsResponse.Applications)
            {
                var standard = standardTasks.Select(s => s.Result).Single(s => s.StandardUId == application.StandardId);
                application.Standard = new Standard()
                {
                    LarsCode = standard.LarsCode,
                    Level = standard.Level,
                    StandardUId = standard.StandardUId,
                    Title = standard.Title,
                    Route = standard.Route,
                    ApprenticeshipFunding = standard.ApprenticeshipFunding?.Select(funding =>
                        new ApprenticeshipFunding()
                        {
                            Duration = funding.Duration,
                            EffectiveFrom = funding.EffectiveFrom,
                            EffectiveTo = funding.EffectiveTo,
                            MaxEmployerLevyCap = funding.MaxEmployerLevyCap
                        })
                };

                application.IsLocationMatch = !pledgeResponse.Locations.Any() || application.Locations.Any();
                application.IsSectorMatch = !pledgeResponse.Sectors.Any() || application.Sectors.Any(x => pledgeResponse.Sectors.Contains(x));
                application.IsJobRoleMatch = !pledgeResponse.JobRoles.Any() || pledgeResponse.JobRoles.Contains(application.Standard.Route);
                application.IsLevelMatch = !pledgeResponse.Levels.Any() || pledgeResponse.Levels.Contains(MapLevelIntToString(application.Standard.Level));
            }

            return new GetApplicationsQueryResult
            {
                Applications = applicationsResponse.Applications.Select(x => (GetApplicationsQueryResultBase.Application)x)
            };
        }

        public string MapLevelIntToString(int levelInt)
        {
            switch (levelInt)
            {
                case 2:
                    return "Level2";
                case 3:
                    return "Level3";
                case 4:
                    return "Level4";
                case 5:
                    return "Level5";
                case 6:
                    return "Level6";
                case 7:
                    return "Level7";
                default:
                    return "";
            }
        }
    }
}