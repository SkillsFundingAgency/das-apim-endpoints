using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetCandidateSkillsAndStrengths;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;
public class WhenHandlingGetCandidateSkillsAndStrengthsQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        GetCandidateSkillsAndStrengthsQuery query,
        GetApplicationApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetCandidateSkillsAndStrengthsQueryHandler handler)
    {
        var expectedApiRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        var response = new ApiResponse<GetApplicationApiResponse>(apiResponse, HttpStatusCode.OK, "");
        candidateApiClient
            .Setup(client => client.GetWithResponseCode<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedApiRequest.GetUrl)))
            .ReturnsAsync(response);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Strengths.Should().BeEquivalentTo(response.Body.Strengths);
    }
}
