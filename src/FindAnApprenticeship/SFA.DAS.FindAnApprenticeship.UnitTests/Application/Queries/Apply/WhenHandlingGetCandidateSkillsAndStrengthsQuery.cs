using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateSkillsAndStrengths;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;
public class WhenHandlingGetCandidateSkillsAndStrengthsQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        GetCandidateSkillsAndStrengthsQuery query,
        GetCandidateSkillsAndStrengthsItemApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetCandidateSkillsAndStrengthsQueryHandler handler)
    {
        var expectedApiRequest = new GetCandidateSkillsAndStrengthsItemApiRequest(query.ApplicationId, query.CandidateId);
        candidateApiClient
            .Setup(client => client.Get<GetCandidateSkillsAndStrengthsItemApiResponse>(
                It.Is<GetCandidateSkillsAndStrengthsItemApiRequest>(r => r.GetUrl == expectedApiRequest.GetUrl)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().BeEquivalentTo((GetCandidateSkillsAndStrengthsQueryResult)apiResponse);
    }
}
