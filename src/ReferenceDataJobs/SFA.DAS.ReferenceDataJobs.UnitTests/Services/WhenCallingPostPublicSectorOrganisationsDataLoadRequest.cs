using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using FluentAssertions;
using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PublicSectorOrganisations;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.ReferenceDataJobs.UnitTests.Services;

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