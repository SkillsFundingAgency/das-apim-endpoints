using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceDataJobs.Application.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PublicSectorOrganisations;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ReferenceDataJobs.UnitTests.Application;

[TestFixture]
public class WhenHandlingStartDataLoadsCommand
{
    [Test, MoqAutoData]
    public async Task Then_call_is_successful(
        StartDataLoadsCommand command,
        [Frozen] Mock<IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration>> mockPSApiClient,
        [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> mockEdApiClient,
        StartDataLoadsCommandHandler sut)
    {
        var responseFromApi = new ApiResponse<object>(null, HttpStatusCode.NoContent, null);
        mockPSApiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<PostPublicSectorOrganisationsDataLoadRequest>(),
                It.IsAny<bool>())).ReturnsAsync(responseFromApi);

        mockEdApiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<PostEducationOrganisationsDataLoadRequest>(),
                It.IsAny<bool>())).ReturnsAsync(responseFromApi);

        await sut.Handle(command, CancellationToken.None);

        //mockPSApiClient.Verify(x =>
        //    x.PostWithResponseCode<object>(It.IsAny<PostPublicSectorOrganisationsDataLoadRequest>(), It.IsAny<bool>()));
        mockEdApiClient.Verify(x =>
            x.PostWithResponseCode<object>(It.IsAny<PostEducationOrganisationsDataLoadRequest>(), It.IsAny<bool>()));
    }

    [Test, MoqAutoData]
    public async Task Then_call_to_public_sector_import_is_unsuccessful(
        StartDataLoadsCommand command,
        [Frozen] Mock<IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration>> mockApiClient,
        [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> mockEdApiClient,
        [Greedy] StartDataLoadsCommandHandler sut)
    {
        var goodResponseFromApi = new ApiResponse<object>(null, HttpStatusCode.NoContent, null);
        var badResponseFromApi = new ApiResponse<object>(null, HttpStatusCode.InternalServerError, "test exception");

        mockApiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<PostPublicSectorOrganisationsDataLoadRequest>(),
                It.IsAny<bool>())).ReturnsAsync(badResponseFromApi);

        mockEdApiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<PostEducationOrganisationsDataLoadRequest>(),
            It.IsAny<bool>())).ReturnsAsync(goodResponseFromApi);

        try
        {
            await sut.Handle(command, CancellationToken.None);
            Assert.Fail("Should not reach this point");
        }
        catch (ApplicationException ex)
        {
            ex.Message.Should().StartWith("Public Sector orgs Import \r\n" + "test exception");
        }
    }

    [Test, MoqAutoData]
    public async Task Then_call_isThen_call_to_education_import_is_unsuccessful_unsuccessful(
        StartDataLoadsCommand command,
        [Frozen] Mock<IPublicSectorOrganisationApiClient<PublicSectorOrganisationApiConfiguration>> mockApiClient,
        [Frozen] Mock<IEducationalOrganisationApiClient<EducationalOrganisationApiConfiguration>> mockEdApiClient,
        [Greedy] StartDataLoadsCommandHandler sut)
    {
        var goodResponseFromApi = new ApiResponse<object>(null, HttpStatusCode.NoContent, null);
        var badResponseFromApi = new ApiResponse<object>(null, HttpStatusCode.InternalServerError, "test exception");

        mockApiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<PostPublicSectorOrganisationsDataLoadRequest>(),
            It.IsAny<bool>())).ReturnsAsync(goodResponseFromApi);

        mockEdApiClient.Setup(x => x.PostWithResponseCode<object>(It.IsAny<PostEducationOrganisationsDataLoadRequest>(),
            It.IsAny<bool>())).ReturnsAsync(badResponseFromApi);

        try
        {
            await sut.Handle(command, CancellationToken.None);
            Assert.Fail("Should not reach this point");
        }
        catch (ApplicationException ex)
        {
            ex.Message.Should().StartWith("Education orgs Import \r\n" + "test exception");
        }
    }
}