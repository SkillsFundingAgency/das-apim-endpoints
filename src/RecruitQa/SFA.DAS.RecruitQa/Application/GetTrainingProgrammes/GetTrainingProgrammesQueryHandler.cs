using MediatR;
using SFA.DAS.RecruitQa.Domain;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderCourses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.Application.GetTrainingProgrammes;

public class GetTrainingProgrammesQueryHandler(
    ICourseService courseService,
    IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpApiClient)
    : IRequestHandler<GetTrainingProgrammesQuery, GetTrainingProgrammesQueryResult>
{
    public async Task<GetTrainingProgrammesQueryResult> Handle(GetTrainingProgrammesQuery request, CancellationToken cancellationToken)
    {
        var standards = await courseService.GetActiveStandards<GetStandardsListResponse>("ActiveStandards");
        var allTrainingProgrammes = standards.Standards?
            .Select(item => (TrainingProgramme)item) ?? [];

        if (!request.Ukprn.HasValue)
        {
            return new GetTrainingProgrammesQueryResult(allTrainingProgrammes);
        }

        var response = await roatpApiClient.Get<GetProvidersListItem>(new GetProviderRequest(request.Ukprn.Value));
        if (response.ProviderTypeId == (int)ProviderType.Employer)
        {
            // employer providers should see all training courses
            return new GetTrainingProgrammesQueryResult(allTrainingProgrammes);
        }

        var providerCourses = await roatpApiClient.Get<List<ProviderCourse>>(new GetAllProviderCoursesRequest(request.Ukprn.Value));
        if (providerCourses is not { Count: > 0 })
        {
            return GetTrainingProgrammesQueryResult.Empty;
        }

        var providerLarsCodes = providerCourses.Select(c => c.LarsCode.ToString()).ToHashSet();
        var filteredCourses = allTrainingProgrammes.Where(p => providerLarsCodes.Contains(p.Id));
        return new GetTrainingProgrammesQueryResult(filteredCourses);
    }
}