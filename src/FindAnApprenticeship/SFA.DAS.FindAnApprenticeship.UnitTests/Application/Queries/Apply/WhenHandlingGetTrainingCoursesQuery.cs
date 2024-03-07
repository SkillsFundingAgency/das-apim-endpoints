using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.TrainingCourses;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;
public class WhenHandlingGetTrainingCoursesQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        GetTrainingCoursesQuery query,
        GetTrainingCoursesApiResponse trainingCoursesApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetTrainingCoursesQueryHandler handler)
    {
        var expectedGetTrainingCoursesRequest = new GetTrainingCoursesApiRequest(query.ApplicationId, query.CandidateId);
        candidateApiClient
            .Setup(client => client.Get<GetTrainingCoursesApiResponse>(
                It.Is<GetTrainingCoursesApiRequest>(r => r.GetUrl == expectedGetTrainingCoursesRequest.GetUrl)))
            .ReturnsAsync(trainingCoursesApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo((GetTrainingCoursesQueryResult)trainingCoursesApiResponse);
    }
}
