using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Candidate.GetCandidateDetails;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Candidate;

public class WhenHandlingGetCandidateDetailsQuery
{
    [Test, MoqAutoData]
    public async Task Then_Gets_CandidateDetails_From_Candidate_Api(
        GetCandidateDetailsQuery query,
        GetCandidateApiResponse candidateApiResponse,
        GetAddressApiResponse addressApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        GetCandidateDetailsQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<GetCandidateApiResponse>(It.IsAny<GetCandidateApiRequest>()))
            .ReturnsAsync(candidateApiResponse);

        mockApiClient
            .Setup(client => client.Get<GetAddressApiResponse>(It.IsAny<GetCandidateAddressApiRequest>()))
            .ReturnsAsync(addressApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using var scope = new AssertionScope();
        result.Email.Should().Be(candidateApiResponse.Email);
        result.Id.Should().Be(candidateApiResponse.Id);
        result.GovUkIdentifier.Should().Be(candidateApiResponse.GovUkIdentifier);
        result.FirstName.Should().Be(candidateApiResponse.FirstName);
        result.MiddleName.Should().Be(candidateApiResponse.MiddleName);
        result.LastName.Should().Be(candidateApiResponse.LastName);
        result.PhoneNumber.Should().Be(candidateApiResponse.PhoneNumber);
        result.Address.Should().BeEquivalentTo(addressApiResponse, options => options.Excluding(fil => fil.CandidateId));
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Null_When_CandidateDetails_Return_Null_From_Candidate_Api(
        GetCandidateDetailsQuery query,
        GetAddressApiResponse addressApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        GetCandidateDetailsQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<GetCandidateApiResponse>(It.IsAny<GetCandidateApiRequest>()))
            .ReturnsAsync((GetCandidateApiResponse)null!);

        mockApiClient
            .Setup(client => client.Get<GetAddressApiResponse>(It.IsAny<GetCandidateAddressApiRequest>()))
            .ReturnsAsync(addressApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using var scope = new AssertionScope();
        result.Should().BeNull();
    }
}