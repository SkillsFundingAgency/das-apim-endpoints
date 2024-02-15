using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourse;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;
public class WhenHandlingGetDeleteTrainingCourseQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        GetDeleteTrainingCourseQuery query,
        GetDeleteTrainingCourseResponse trainingCourseApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetDeleteTrainingCourseQueryHandler handler)
    {
        var expectedGetTrainingCourseRequest = new GetDeleteTrainingCourseRequest(query.ApplicationId, query.CandidateId, query.TrainingCourseId);
        candidateApiClient
            .Setup(client => client.Get<GetDeleteTrainingCourseResponse>(
                It.Is<GetDeleteTrainingCourseRequest>(r => r.GetUrl == expectedGetTrainingCourseRequest.GetUrl)))
            .ReturnsAsync(trainingCourseApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo((GetDeleteTrainingCourseQueryResult)trainingCourseApiResponse);
    }
}
