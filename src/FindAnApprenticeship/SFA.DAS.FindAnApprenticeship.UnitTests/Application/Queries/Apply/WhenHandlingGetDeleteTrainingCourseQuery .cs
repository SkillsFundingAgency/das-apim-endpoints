using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.DeleteTrainingCourse;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;

public class WhenHandlingGetDeleteTrainingCourseQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        GetDeleteTrainingCourseQuery query,
        GetTrainingCourseApiResponse trainingCourseApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetDeleteTrainingCourseQueryHandler handler)
    {
        var expectedGetTrainingCourseRequest = new GetTrainingCourseApiRequest(query.ApplicationId, query.CandidateId, query.TrainingCourseId);
        candidateApiClient
            .Setup(client => client.Get<GetTrainingCourseApiResponse>(
                It.Is<GetTrainingCourseApiRequest>(r => r.GetUrl == expectedGetTrainingCourseRequest.GetUrl)))
            .ReturnsAsync(trainingCourseApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo(GetDeleteTrainingCourseQueryResult.From(trainingCourseApiResponse));
    }
}
