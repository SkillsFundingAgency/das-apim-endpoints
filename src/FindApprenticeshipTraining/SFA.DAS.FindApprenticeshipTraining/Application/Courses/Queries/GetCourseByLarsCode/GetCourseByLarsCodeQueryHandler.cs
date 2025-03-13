using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseByLarsCode;

public sealed class GetCourseByLarsCodeQueryHandler(
    ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient,
    IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient
) : IRequestHandler<GetCourseByLarsCodeQuery, GetCourseByLarsCodeQueryResult>
{
    public const string KsbsSkillsType = "Skill";
    public const string KsbsKnowledgeType = "Knowledge";
    public const string KsbsBehaviorType = "Behaviour";

    public async Task<GetCourseByLarsCodeQueryResult> Handle(GetCourseByLarsCodeQuery query, CancellationToken cancellationToken)
    {
        var coursesApiStandardResponse = await _coursesApiClient.GetWithResponseCode<StandardDetailResponse>(
            new GetStandardDetailsByIdRequest(
                query.LarsCode.ToString()
            )
        );

        coursesApiStandardResponse.EnsureSuccessStatusCode();

        StandardDetailResponse standardDetails = coursesApiStandardResponse.Body;

        ApprenticeshipFunding apprenticeshipFunding = standardDetails.ApprenticeshipFunding?.Count > 0 ?
            standardDetails.ApprenticeshipFunding[standardDetails.ApprenticeshipFunding.Count - 1] :
            null;

        var courseTrainingProvidersCountResponse =
            await _roatpCourseManagementApiClient.GetWithResponseCode<GetCourseTrainingProvidersCountResponse>(
                new GetCourseTrainingProvidersCountRequest(
                    [query.LarsCode],
                    query.Distance,
                    query.Lat,
                    query.Lon
                )
        );

        courseTrainingProvidersCountResponse.EnsureSuccessStatusCode();

        CourseTrainingProviderCountModel trainingCourseCountDetails = 
            courseTrainingProvidersCountResponse.Body.Courses.Count > 0 ?
                courseTrainingProvidersCountResponse.Body.Courses[0] : 
                null;

        GetCourseByLarsCodeQueryResult result = standardDetails;

        result.MaxFunding = apprenticeshipFunding?.MaxEmployerLevyCap ?? 0;
        result.TypicalDuration = apprenticeshipFunding?.Duration ?? 0;

        result.ProvidersCountWithinDistance = trainingCourseCountDetails?.ProvidersCount ?? 0;
        result.TotalProvidersCount = trainingCourseCountDetails?.TotalProvidersCount ?? 0;

        result.Skills = standardDetails.Ksbs.Where(a => a.Type == KsbsSkillsType).Select(a => a.Description).ToArray();
        result.Knowledge = standardDetails.Ksbs.Where(a => a.Type == KsbsKnowledgeType).Select(a => a.Description).ToArray();
        result.Behaviours = standardDetails.Ksbs.Where(a => a.Type == KsbsBehaviorType).Select(a => a.Description).ToArray();

        return result;
    }
}

