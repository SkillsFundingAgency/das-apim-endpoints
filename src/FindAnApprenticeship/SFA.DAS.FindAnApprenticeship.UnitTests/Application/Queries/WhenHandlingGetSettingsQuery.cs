using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetSettings;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries
{
    [TestFixture]
    public class WhenHandlingGetSettingsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Result_Is_Mapped_From_Candidates_Api_Responses(
         GetSettingsQuery query,
         GetCandidateApiResponse apiResponse,
         GetCandidatePreferencesApiResponse candidatePreferencesApiResponse,
         [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
         GetSettingsQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetCandidateApiResponse>(
                    It.Is<GetCandidateApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
                .ReturnsAsync(apiResponse);

            mockApiClient
                .Setup(client => client.Get<GetCandidatePreferencesApiResponse>(
                    It.Is<GetCandidatePreferencesApiRequest>(c => c.GetUrl.Contains(query.CandidateId.ToString()))))
                .ReturnsAsync(candidatePreferencesApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.FirstName.Should().Be(apiResponse.FirstName);
            result.LastName.Should().Be(apiResponse.LastName);
            result.MiddleNames.Should().Be(apiResponse.MiddleNames);
            result.DateOfBirth.Should().Be(apiResponse.DateOfBirth);
            result.AddressLine1.Should().Be(apiResponse.Address.AddressLine1);
            result.AddressLine2.Should().Be(apiResponse.Address.AddressLine2);
            result.Town.Should().Be(apiResponse.Address.Town);
            result.County.Should().Be(apiResponse.Address.County);
            result.Postcode.Should().Be(apiResponse.Address.Postcode);

            var expectedPreferences = candidatePreferencesApiResponse.CandidatePreferences == null
                ? new List<GetSettingsQueryResult.CandidatePreference>()
                : candidatePreferencesApiResponse.CandidatePreferences.Select(cp =>
                    new GetSettingsQueryResult.CandidatePreference
                    {
                        PreferenceId = cp.PreferenceId,
                        PreferenceMeaning = cp.PreferenceMeaning,
                        PreferenceHint = cp.PreferenceHint,
                        ContactMethodsAndStatus = cp.ContactMethodsAndStatus.Select(c =>
                            new GetSettingsQueryResult.ContactMethodStatus
                            {
                                ContactMethod = c.ContactMethod,
                                Status = c.Status
                            }).ToList()
                    }).ToList();

            result.CandidatePreferences.Should().BeEquivalentTo(expectedPreferences);
        }
    }
}
