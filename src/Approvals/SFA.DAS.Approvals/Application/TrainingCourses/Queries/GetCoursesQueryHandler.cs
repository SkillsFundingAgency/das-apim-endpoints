using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries;

public class GetCoursesQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient,
    IInternalApiClient<LearnerDataInnerApiConfiguration> learnerDataApiClient)
        : IRequestHandler<GetCoursesQuery, GetCoursesResult>
{
    public async Task<GetCoursesResult> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        var response = await commitmentsApiClient.GetWithResponseCode<GetCoursesResponse>(new GetAllTrainingProgrammesRequest());

        var courseCodes = await learnerDataApiClient.GetWithResponseCode<GetCourseCodesByUkprnResponse>(new GetCourseCodesByUkprnRequest(request.Ukprn));

        var trainingProgrammesForUkprn = response.Body.TrainingProgrammes.Where(t => courseCodes.Body.CourseCodes.ToString().Contains(t.CourseCode));

        return new GetCoursesResult
        {
            TrainingProgrammes = trainingProgrammesForUkprn
        };
    }
}