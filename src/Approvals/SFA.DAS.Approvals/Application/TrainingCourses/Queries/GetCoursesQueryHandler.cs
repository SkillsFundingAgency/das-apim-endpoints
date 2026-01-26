using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.Courses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.TrainingCourses.Queries;

public class GetCoursesQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient)
        : IRequestHandler<GetCoursesQuery, GetCoursesResult>
{
    public async Task<GetCoursesResult> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        var response = await commitmentsApiClient.GetWithResponseCode<GetCoursesResponse>(new GetAllTrainingProgrammesRequest());

        return new GetCoursesResult
        {
            TrainingProgrammes = response.Body.TrainingProgrammes
        };
    }
}
