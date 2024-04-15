using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.PhoneNumber;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries;
public class WhenHandlingGetPhoneNumberQuery
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Candidate_PhoneNumber_From_Candidates_Api(
        GetPhoneNumberQuery query,
        GetCandidateApiResponse apiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        GetPhoneNumberQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<GetCandidateApiResponse>(
                It.Is<GetCandidateApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.PhoneNumber.Should().Be(apiResponse.PhoneNumber);
    }
}
