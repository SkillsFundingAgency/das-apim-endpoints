using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.CheckAnswers;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.CreateAccount;
public class WhenHandlingGetCandidateCheckAnswersQuery
{
    [Test, MoqAutoData]
    public async Task Then_Result_Is_Mapped_From_Candidates_Api_Responses(
        GetCheckAnswersQuery query,
        GetCandidateApiResponse apiResponse,
        GetCandidateAddressApiResponse addressApiResponse,
        GetCandidatePreferencesApiResponse candidatePreferencesApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
        GetCheckAnswersQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<GetCandidateApiResponse>(
                It.Is<GetCandidateApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(apiResponse);

        mockApiClient
            .Setup(client => client.Get<GetCandidateAddressApiResponse>(
                It.Is<GetCandidateAddressApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(addressApiResponse);

        mockApiClient
            .Setup(client => client.Get<GetCandidatePreferencesApiResponse>(
                It.Is<GetCandidatePreferencesApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
            .ReturnsAsync(candidatePreferencesApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.FirstName.Should().Be(apiResponse.FirstName);
        result.LastName.Should().Be(apiResponse.LastName);
        result.MiddleNames.Should().Be(apiResponse.MiddleNames);
        result.DateOfBirth.Should().Be(apiResponse.DateOfBirth);
        result.AddressLine1.Should().Be(addressApiResponse.AddressLine1);
        result.AddressLine2.Should().Be(addressApiResponse.AddressLine2);
        result.Town.Should().Be(addressApiResponse.Town);
        result.County.Should().Be(addressApiResponse.County);
        result.Postcode.Should().Be(addressApiResponse.Postcode);

        var expectedPreferences = candidatePreferencesApiResponse.CandidatePreferences == null
            ? new List<GetCheckAnswersQueryResult.CandidatePreference>()
            : candidatePreferencesApiResponse.CandidatePreferences.Select(cp =>
                new GetCheckAnswersQueryResult.CandidatePreference
                {
                    PreferenceId = cp.PreferenceId,
                    PreferenceMeaning = cp.PreferenceMeaning,
                    PreferenceHint = cp.PreferenceHint,
                    ContactMethodsAndStatus = cp.ContactMethodsAndStatus.Select(c =>
                        new GetCheckAnswersQueryResult.ContactMethodStatus
                        {
                            ContactMethod = c.ContactMethod,
                            Status = c.Status
                        }).ToList()
                }).ToList();

        result.CandidatePreferences.Should().BeEquivalentTo(expectedPreferences);
    }
}
