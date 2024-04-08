using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ReferenceDataJobs.Application.Commands;
using SFA.DAS.ReferenceDataJobs.Configuration;
using SFA.DAS.ReferenceDataJobs.InnerApi.Requests;
using SFA.DAS.ReferenceDataJobs.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ReferenceDataJobs.UnitTests.Application;

[TestFixture]
public class WhenHandlingStartDataLoadsCommand
{
    [Test, MoqAutoData]
    public async Task Then_call_is_successful(
        StartDataLoadsCommand command,
        [Frozen] Mock<IPublicSectorOrganisationsApiClient<PublicSectorOrganisationsApiConfiguration>> mockApiClient,
        StartDataLoadsCommandHandler sut)
    {
        var responseFromApi = new ApiResponse<object>(null, HttpStatusCode.OK, null);
        mockApiClient
            .Setup(x => x.PostWithResponseCode<object>(It.IsAny<PostPublicSectorOrganisationsDataLoadRequest>(),
                It.IsAny<bool>())).ReturnsAsync(responseFromApi);

        await sut.Handle(command, CancellationToken.None);

        mockApiClient.Verify(x =>
            x.PostWithResponseCode<object>(It.IsAny<PostPublicSectorOrganisationsDataLoadRequest>(), It.IsAny<bool>()));
    }

    [Test, MoqAutoData]
    public async Task Then_call_is_unsuccessful(
        StartDataLoadsCommand command,
        [Frozen] Mock<IPublicSectorOrganisationsApiClient<PublicSectorOrganisationsApiConfiguration>> mockApiClient,
        [Greedy] StartDataLoadsCommandHandler sut)
    {
        mockApiClient
            .Setup(x => x.PostWithResponseCode<object>(It.IsAny<PostPublicSectorOrganisationsDataLoadRequest>(),
                It.IsAny<bool>())).ThrowsAsync(new ApplicationException("test"));
        try
        {
            await sut.Handle(command, CancellationToken.None);
            Assert.Fail("Should not reach this point");
        }
        catch (ApplicationException ex)
        {
            ex.Message.Should().Be("test");
        }

        mockApiClient.Verify(x =>
            x.PostWithResponseCode<object>(It.IsAny<PostPublicSectorOrganisationsDataLoadRequest>(), It.IsAny<bool>()));
    }
}