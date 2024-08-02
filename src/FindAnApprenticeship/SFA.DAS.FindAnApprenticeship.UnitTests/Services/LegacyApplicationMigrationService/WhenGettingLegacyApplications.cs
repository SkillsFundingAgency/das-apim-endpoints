using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Services.LegacyApplicationMigrationService
{
    public class WhenGettingLegacyApplications
    {
        [Test, MoqAutoData]
        public async Task Then_Legacy_Applications_Are_Returned(
            string candidateEmailAddress,
            GetLegacyApplicationsByEmailApiResponse legacyApplicationsResponse,
            [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> mockLegacyApiClient,
            FindAnApprenticeship.Services.LegacyApplicationMigrationService service)
        {

            var legacyGetApplicationsRequest = new GetLegacyApplicationsByEmailApiRequest(candidateEmailAddress);
            mockLegacyApiClient
                .Setup(client => client.Get<GetLegacyApplicationsByEmailApiResponse>(
                    It.Is<GetLegacyApplicationsByEmailApiRequest>(r => r.GetUrl == legacyGetApplicationsRequest.GetUrl)))
                .ReturnsAsync(() => legacyApplicationsResponse);

            var actual = await service.GetLegacyApplications(candidateEmailAddress);

            actual.Should().NotBeNull();
            actual.Applications.Should().BeEquivalentTo(legacyApplicationsResponse.Applications);
        }

        [Test, MoqAutoData]
        public async Task Then_Legacy_Applications_Returns_Empty(
            string candidateEmailAddress,
            [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> mockLegacyApiClient,
            FindAnApprenticeship.Services.LegacyApplicationMigrationService service)
        {

            var legacyGetApplicationsRequest = new GetLegacyApplicationsByEmailApiRequest(candidateEmailAddress);
            mockLegacyApiClient
                .Setup(client => client.Get<GetLegacyApplicationsByEmailApiResponse>(
                    It.Is<GetLegacyApplicationsByEmailApiRequest>(r => r.GetUrl == legacyGetApplicationsRequest.GetUrl)))
                .ReturnsAsync((GetLegacyApplicationsByEmailApiResponse)null!);

            var actual = await service.GetLegacyApplications(candidateEmailAddress);

            actual.Should().NotBeNull();
            actual.Applications.Should().HaveCount(0);
        }
    }
}
