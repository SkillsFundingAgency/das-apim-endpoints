using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateAdditionalQuestionTwo;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;
public class WhenHandlingGetCandidateAdditionalQuestionTwoQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        GetCandidateAdditionalQuestionTwoQuery query,
        GetCandidateAdditionalQuestionTwoApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetCandidateAdditionalQuestionTwoQueryHandler handler)
    {
        var expectedApiRequest = new GetCandidateAdditionalQuestionTwoApiRequest(query.ApplicationId, query.CandidateId);
        candidateApiClient
            .Setup(client => client.Get<GetCandidateAdditionalQuestionTwoApiResponse>(
                It.Is<GetCandidateAdditionalQuestionTwoApiRequest>(r => r.GetUrl == expectedApiRequest.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo((GetCandidateAdditionalQuestionTwoQueryResult)apiResponse);
    }
}
