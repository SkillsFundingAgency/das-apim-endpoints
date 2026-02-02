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

public class GetCourseCodesQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsApiClient,
    IInternalApiClient<LearnerDataInnerApiConfiguration> learnerDataApiClient)
        : IRequestHandler<GetCourseCodesQuery, GetCourseCodesResult>
{
    public async Task<GetCourseCodesResult> Handle(GetCourseCodesQuery request, CancellationToken cancellationToken)
    {
        var response = await commitmentsApiClient.GetWithResponseCode<GetCourseCodesResponse>(new GetAllTrainingProgrammesRequest());

        var courseCodes = await learnerDataApiClient.GetWithResponseCode<GetCourseCodesByUkprnResponse>(new GetCourseCodesByUkprnRequest(request.Ukprn));

        var codes = courseCodes.Body.CourseCodes.Select(t => t.ToString());

        var trainingProgrammesForUkprn = response.Body.TrainingProgrammes.Where(t => codes.Contains(t.CourseCode));

        return new GetCourseCodesResult
        {
            TrainingProgrammes = trainingProgrammesForUkprn
        };
    }
}