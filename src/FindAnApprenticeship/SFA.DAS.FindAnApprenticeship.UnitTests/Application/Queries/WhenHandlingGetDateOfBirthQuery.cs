using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetDateOfBirth;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;
public class WhenHandlingGetDateOfBirthQuery
{
    [Test, MoqAutoData]
    public async Task Then_Gets_CandidatePreferences_From_Candidates_Api(
        GetDateOfBirthQuery query,
        GetCandidateDateOfBirthApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        GetDateOfBirthQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<GetCandidateDateOfBirthApiResponse>(
                It.Is<GetCandidateDateOfBirthApiRequest>(c => c.GetUrl.Contains(query.GovUkIdentifier))))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.DateOfBirth.Should().Be(apiResponse.DateOfBirth);
    }

    [Test, MoqAutoData]
    public async Task And_No_CandidatePreferences_Returned_Then_Empty_List(
        GetDateOfBirthQuery query,
        GetCandidateDateOfBirthApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        GetDateOfBirthQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<GetCandidateDateOfBirthApiResponse>(
                It.Is<GetCandidateDateOfBirthApiRequest>(c => c.GetUrl.Contains(query.GovUkIdentifier))))
            .ReturnsAsync(() => null);

        var result = await handler.Handle(query, CancellationToken.None);

        result.DateOfBirth.Should().BeNull();
    }
}
