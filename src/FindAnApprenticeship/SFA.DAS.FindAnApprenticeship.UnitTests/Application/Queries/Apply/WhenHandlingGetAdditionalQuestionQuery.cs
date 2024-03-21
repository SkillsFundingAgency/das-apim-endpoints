using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAdditionalQuestion;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;
public class WhenHandlingGetAdditionalQuestionQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        GetAdditionalQuestionQuery query,
        GetAdditionalQuestionApiResponse questionResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetAdditionalQuestionQueryHandler handler)
    {
        var expectedRequest = new GetAdditionalQuestionApiRequest(query.ApplicationId, query.CandidateId, query.Id);

        candidateApiClient
            .Setup(client => client.Get<GetAdditionalQuestionApiResponse>(
                It.Is<GetAdditionalQuestionApiRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(questionResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<GetAdditionalQuestionQueryResult>();
            candidateApiClient.Verify(p => p.Get<GetAdditionalQuestionApiResponse>(It.Is<GetAdditionalQuestionApiRequest>(x => x.GetUrl == expectedRequest.GetUrl)), Times.Once);
            result.QuestionText.Should().BeEquivalentTo(questionResponse.QuestionText);
        }
    }
}
