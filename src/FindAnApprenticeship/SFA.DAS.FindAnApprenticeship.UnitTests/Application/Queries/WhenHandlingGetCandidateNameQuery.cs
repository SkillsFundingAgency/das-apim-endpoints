using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateName;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;
public class WhenHandlingGetCandidateNameQuery
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Candidate_Name_From_Candidates_Api(
        GetCandidateNameQuery query,
        GetCandidateApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        GetCandidateNameQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<GetCandidateApiResponse>(
                It.Is<GetCandidateApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.FirstName.Should().Be(apiResponse.FirstName);
    }

    [Test, MoqAutoData]
    public async Task And_No_Name_Returned_Then_Name_Set_To_Null(
        GetCandidateNameQuery query,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        GetCandidateNameQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<GetCandidateApiResponse>(
                It.Is<GetCandidateApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(() => null);

        var result = await handler.Handle(query, CancellationToken.None);

        result.FirstName.Should().BeNull();
    }
}
