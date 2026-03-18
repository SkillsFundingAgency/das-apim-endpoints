using System.Net;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Types.Models.Roatp;
using SFA.DAS.SharedOuterApi.Types.Services;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services.TrainingProviderServiceTests;
public class GetProviderDetailsTests
{
    [Test, MoqAutoData]
    public async Task WhenNoProviderIsFound_ReturnsNull(
        int providerId,
        [Frozen] Mock<IInternalApiClient<TrainingProviderConfiguration>> client,
        TrainingProviderService sut)
    {
        client.Setup(x => x.GetWithResponseCode<OrganisationResponse>(It.IsAny<GetOrganisationRequest>()))
            .ReturnsAsync(new ApiResponse<OrganisationResponse>(null, HttpStatusCode.NotFound, null));

        var response = await sut.GetProviderDetails(providerId);

        response.Should().BeNull();
    }
    [Test, MoqAutoData]
    public async Task WhenAProviderIsFound_ReturnsProviderDetailsModel(
        int providerId,
        OrganisationResponse organisationResponse,
        [Frozen] Mock<IInternalApiClient<TrainingProviderConfiguration>> client,
        TrainingProviderService sut)
    {
        client.Setup(x => x.GetWithResponseCode<OrganisationResponse>(It.IsAny<GetOrganisationRequest>()))
            .ReturnsAsync(new ApiResponse<OrganisationResponse>(organisationResponse, HttpStatusCode.OK, null));

        var response = await sut.GetProviderDetails(providerId);

        response.Should().BeEquivalentTo((ProviderDetailsModel)organisationResponse);
    }
}
