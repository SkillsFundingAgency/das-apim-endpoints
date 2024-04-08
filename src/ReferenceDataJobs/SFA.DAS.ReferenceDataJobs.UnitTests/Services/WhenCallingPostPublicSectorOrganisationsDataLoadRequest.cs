using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.ReferenceDataJobs.Configuration;
using SFA.DAS.ReferenceDataJobs.InnerApi.Requests;
using SFA.DAS.ReferenceDataJobs.Services;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using FluentAssertions;
using Moq;

namespace SFA.DAS.ReferenceDataJobs.UnitTests.Services;

public class WhenCallingPostPublicSectorOrganisationsDataLoadRequest
{
    [Test, AutoData]
    public async Task Then_post_endpoint_is_called_correctly2(
        [Frozen] Mock<IInternalApiClient<PublicSectorOrganisationsApiConfiguration>> mockApiClient,
        PostPublicSectorOrganisationsDataLoadRequest request,
        bool includeResponse
        )
    {
        var responseFromApi = new ApiResponse<object>(null, HttpStatusCode.OK, null);
        mockApiClient.Setup(x => x.PostWithResponseCode<object>(request, includeResponse)).ReturnsAsync(responseFromApi);

        var sut = new PublicSectorOrganisationsApiClient(mockApiClient.Object);

        var response = await sut.PostWithResponseCode<object>(request, includeResponse);

        response.Should().BeEquivalentTo(responseFromApi);
    }
}