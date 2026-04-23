using System.Net;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.PublicSectorOrganisations;
using SFA.DAS.SharedOuterApi.Types.Services;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services;

public class WhenCallingPostPublicSectorOrganisationsDataLoadRequest
{
    [Test, AutoData]
    public async Task Then_post_endpoint_is_called_correctly2(
        [Frozen] Mock<IInternalApiClient<PublicSectorOrganisationApiConfiguration>> mockApiClient,
        PostPublicSectorOrganisationsDataLoadRequest request,
        bool includeResponse
    )
    {
        var responseFromApi = new ApiResponse<object>(null, HttpStatusCode.OK, null);
        mockApiClient.Setup(x => x.PostWithResponseCode<object>(request, includeResponse)).ReturnsAsync(responseFromApi);

        var sut = new PublicSectorOrganisationApiClient(mockApiClient.Object);

        var response = await sut.PostWithResponseCode<object>(request, includeResponse);

        response.Should().BeEquivalentTo(responseFromApi);
    }
}