using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.TrainingCourses.Queries;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.TrainingCourses.Queries;

public class WhenGettingCourseCodesByUkprn
{
    [Test, MoqAutoData]
    public async Task Then_Gets_CourseCodes_By_Ukprn(
            GetCourseCodesQuery query,
            ApiResponse<GetCourseCodesResponse> apiResponse,
            ApiResponse<GetCourseCodesByUkprnResponse> ukprnResponse,
            GetAllTrainingProgrammesRequest request,
            [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> mockLearnerApiClient,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApiClient,
            GetCourseCodesQueryHandler handler)
    {
        SetCourseCodes(apiResponse, ukprnResponse);

        mockLearnerApiClient
            .Setup(c => c.GetWithResponseCode<GetCourseCodesByUkprnResponse>(It.Is<GetCourseCodesByUkprnRequest>(t => t.Ukprn == query.Ukprn)))
            .ReturnsAsync(ukprnResponse);

        mockCommitmentsApiClient.Setup(s => s.GetWithResponseCode<GetCourseCodesResponse>(It.Is<IGetApiRequest>(c => c.GetUrl.Equals(request.GetUrl)))).
            ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.TrainingProgrammes.Count().Should().Be(ukprnResponse.Body.CourseCodes.Count);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_No_CourseCodes_By_Ukprn(
            GetCourseCodesQuery query,
            ApiResponse<GetCourseCodesResponse> apiResponse,
            GetAllTrainingProgrammesRequest request,
            [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> mockLearnerApiClient,
            [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockCommitmentsApiClient,
            GetCourseCodesQueryHandler handler)
    {
        var ukprnResponse = new ApiResponse<GetCourseCodesByUkprnResponse>(new GetCourseCodesByUkprnResponse() { CourseCodes = new System.Collections.Generic.List<int>() }, System.Net.HttpStatusCode.OK, "");

        SetCourseCodes(apiResponse, ukprnResponse);

        mockLearnerApiClient
            .Setup(c => c.GetWithResponseCode<GetCourseCodesByUkprnResponse>(It.Is<GetCourseCodesByUkprnRequest>(t => t.Ukprn == query.Ukprn)))
            .ReturnsAsync(ukprnResponse);

        mockCommitmentsApiClient.Setup(s => s.GetWithResponseCode<GetCourseCodesResponse>(It.Is<IGetApiRequest>(c => c.GetUrl.Equals(request.GetUrl)))).
            ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.TrainingProgrammes.Count().Should().Be(ukprnResponse.Body.CourseCodes.Count);
    }

    private static void SetCourseCodes(ApiResponse<GetCourseCodesResponse> apiResponse, ApiResponse<GetCourseCodesByUkprnResponse> ukprnResponse)
    {
        for (int i = 0; i < ukprnResponse.Body.CourseCodes.Count; i++)
        {
            apiResponse.Body.TrainingProgrammes.ToList()[i].CourseCode = ukprnResponse.Body.CourseCodes.ToList()[i].ToString();
        }
    }
}