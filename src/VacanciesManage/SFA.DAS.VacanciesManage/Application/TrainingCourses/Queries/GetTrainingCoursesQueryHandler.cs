using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.VacanciesManage.InnerApi.Requests;
using SFA.DAS.VacanciesManage.InnerApi.Responses;

namespace SFA.DAS.VacanciesManage.Application.TrainingCourses.Queries;

public class GetTrainingCoursesQueryHandler(ICourseService courseService,
    IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient)
    : IRequestHandler<GetTrainingCoursesQuery, GetTrainingCoursesQueryResult>
{
    public async Task<GetTrainingCoursesQueryResult> Handle(GetTrainingCoursesQuery query, CancellationToken cancellationToken)
    {
        var courses = await courseService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));
        var standards = courses.Standards;
        if (query.Ukprn is null)
        {
            return new GetTrainingCoursesQueryResult { TrainingCourses = standards };
        }
        
        var providerCourseDetails = await roatpCourseManagementApiClient
            .Get<List<GetRoatpProviderAdditionalStandardsItem>>(new GetProviderAdditionalStandardsRequest(query.Ukprn.Value));
        var larsCodes = providerCourseDetails?.Select(x => x.LarsCode) ?? [];
        standards = standards.Where(x => larsCodes.Contains(x.LarsCode));

        return new GetTrainingCoursesQueryResult { TrainingCourses = standards };
    }
}