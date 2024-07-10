using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePreferences;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;
public class WhenHandlingGetCandidatePreferencesQuery
{
    [Test, MoqAutoData]
    public async Task Then_Gets_CandidatePreferences_From_Candidates_Api(
        GetCandidatePreferencesQuery query,
        GetCandidatePreferencesApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        GetCandidatePreferencesQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<GetCandidatePreferencesApiResponse>(
                It.Is<GetCandidatePreferencesApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.CandidatePreferences.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task And_No_CandidatePreferences_Returned_Then_Empty_List(
        GetCandidatePreferencesQuery query,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        GetCandidatePreferencesQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<GetCandidatePreferencesApiResponse>(
                It.Is<GetCandidatePreferencesApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(() => null);

        var result = await handler.Handle(query, CancellationToken.None);

        result.CandidatePreferences.Should().BeNullOrEmpty();
    }
}
