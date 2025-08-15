using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetTrainingProgrammes;

public class GetTrainingProgrammesQueryHandler(
    ICourseService courseService,
    IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpApiClient,
    ILogger<GetTrainingProgrammesQueryHandler> _logger)
    : IRequestHandler<GetTrainingProgrammesQuery, GetTrainingProgrammesQueryResult>
{
    public async Task<GetTrainingProgrammesQueryResult> Handle(GetTrainingProgrammesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching active standards");

        var standards = await courseService.GetActiveStandards<GetStandardsListResponse>("ActiveStandards");
        var allTrainingProgrammes = standards.Standards?
            .Where(c => request.IncludeFoundationApprenticeships ||
                        c.ApprenticeshipType.Equals("Apprenticeship", StringComparison.CurrentCultureIgnoreCase))
            .Select(item => (TrainingProgramme)item)
            .ToList() ?? [];

        if (!request.Ukprn.HasValue)
        {
            _logger.LogInformation("Returning {Count} unfiltered training programmes", allTrainingProgrammes.Count);
            return new GetTrainingProgrammesQueryResult
            {
                TrainingProgrammes = allTrainingProgrammes
            };
        }

        _logger.LogInformation("Filtering training programmes for UKPRN {Ukprn}", request.Ukprn.Value);

        var providerCourses = await roatpApiClient.Get<List<ProviderCourse>>(new GetAllProviderCoursesRequest(request.Ukprn.Value ));

        if (providerCourses == null || !providerCourses.Any())
        {
            _logger.LogInformation("No provider courses found for UKPRN {ukprn}", request.Ukprn.Value);
            return new GetTrainingProgrammesQueryResult
            {
                TrainingProgrammes = allTrainingProgrammes
            };
        }

        var providerLarsCodes = providerCourses.Select(c => c.LarsCode.ToString()).ToHashSet();

        var filteredCourses = allTrainingProgrammes
            .Where(p => providerLarsCodes.Contains(p.Id))
            .ToList();

        _logger.LogInformation("Returning {Count} training programmes", filteredCourses.Count);

        return new GetTrainingProgrammesQueryResult
        {
            TrainingProgrammes = filteredCourses
        };
    }
}