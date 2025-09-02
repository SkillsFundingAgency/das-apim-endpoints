using MediatR;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetTrainingProgrammes;

public class GetTrainingProgrammesQueryHandler(
    ICourseService courseService,
    IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpApiClient)
    : IRequestHandler<GetTrainingProgrammesQuery, GetTrainingProgrammesQueryResult>
{
    public async Task<GetTrainingProgrammesQueryResult> Handle(GetTrainingProgrammesQuery request, CancellationToken cancellationToken)
    {
        var standards = await courseService.GetActiveStandards<GetStandardsListResponse>("ActiveStandards");
        var allTrainingProgrammes = standards.Standards?
            .Select(item => (TrainingProgramme)item)
            .ToList() ?? [];

        if (request.Ukprn.HasValue)
        {
            var providerCourses = await roatpApiClient.Get<List<ProviderCourse>>(new GetAllProviderCoursesRequest(request.Ukprn.Value));

            if (providerCourses == null || !providerCourses.Any())
            {
                return new GetTrainingProgrammesQueryResult
                {
                    TrainingProgrammes = []
                };
            }

            var providerLarsCodes = providerCourses.Select(c => c.LarsCode.ToString()).ToHashSet();

            var filteredCourses = allTrainingProgrammes
                .Where(p => providerLarsCodes.Contains(p.Id))
                .ToList();

            return new GetTrainingProgrammesQueryResult
            {
                TrainingProgrammes = filteredCourses
            };
        }

        return new GetTrainingProgrammesQueryResult
        {
            TrainingProgrammes = allTrainingProgrammes
        };
    }
}