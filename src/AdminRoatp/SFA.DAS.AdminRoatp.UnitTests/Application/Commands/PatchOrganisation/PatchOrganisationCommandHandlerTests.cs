using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using SFA.DAS.AdminRoatp.Application.Commands.CreateProvider;
using SFA.DAS.AdminRoatp.Application.Commands.PatchOrganisation;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
using SFA.DAS.AdminRoatp.Infrastructure;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Commands.PatchOrganisation;
public class PatchOrganisationCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_CallsApiClientPatchOrganisation_NotChangingProviderTypeToMain(
        [Frozen] Mock<IRoatpServiceRestApiClient> roatpServiceRestApiClientMock,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpV2ApiClientMock,
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpServiceApiClient,
        [Greedy] PatchOrganisationCommandHandler sut,
        string userId,
        string userName,
        CancellationToken cancellationToken
        )
    {
        int ukprn = 12345678;
        var patchDoc = new JsonPatchDocument<PatchOrganisationModel>();
        patchDoc.Replace(o => o.ProviderType, ProviderType.Employer);
        roatpServiceRestApiClientMock.Setup(api => api.PatchOrganisation(ukprn, userId, patchDoc, cancellationToken)).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));
        var command = new PatchOrganisationCommand(ukprn, userId, userName, patchDoc);

        await sut.Handle(command, cancellationToken);

        roatpServiceRestApiClientMock.Verify(api => api.PatchOrganisation(ukprn, userId, patchDoc, cancellationToken), Times.Once);
        roatpV2ApiClientMock.Verify(api => api.GetWithResponseCode<OrganisationResponse>(It.IsAny<GetOrganisationRequest>()), Times.Never);
        roatpServiceApiClient.Verify(api => api.PostWithResponseCode<int>(It.IsAny<PostProviderRequest>(), It.IsAny<bool>()), Times.Never);
    }

    [TestCase(HttpStatusCode.NotFound)]
    [TestCase(HttpStatusCode.BadRequest)]
    [TestCase(HttpStatusCode.InternalServerError)]
    public async Task Handle_ReturnsUnsuccessfulResponseCodeAsReceivedFromInnerApi(HttpStatusCode expected)
    {
        // Arrange
        var roatpServiceRestApiClientMock = new Mock<IRoatpServiceRestApiClient>();
        var roatpV2ApiClientMock = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();
        var roatpServiceApiClient = new Mock<IRoatpServiceApiClient<RoatpConfiguration>>();

        int ukprn = 12345678;
        string userId = Guid.NewGuid().ToString();
        string userName = Guid.NewGuid().ToString();
        CancellationToken cancellationToken = new();
        var patchDoc = new JsonPatchDocument<PatchOrganisationModel>();
        patchDoc.Replace(o => o.ProviderType, ProviderType.Main);
        var command = new PatchOrganisationCommand(ukprn, userId, userName, patchDoc);

        roatpServiceRestApiClientMock.Setup(api => api.PatchOrganisation(ukprn, userId, patchDoc, cancellationToken)).ReturnsAsync(new HttpResponseMessage(expected));

        var handler = new PatchOrganisationCommandHandler(roatpServiceRestApiClientMock.Object, roatpV2ApiClientMock.Object, roatpServiceApiClient.Object);

        // Act
        var actual = await handler.Handle(command, CancellationToken.None);
        // Assert
        actual.Should().Be(expected);
        roatpServiceRestApiClientMock.Verify(api => api.PatchOrganisation(ukprn, userId, patchDoc, cancellationToken), Times.Once);
        roatpV2ApiClientMock.Verify(api => api.GetWithResponseCode<OrganisationResponse>(It.IsAny<GetOrganisationRequest>()), Times.Never);
        roatpServiceApiClient.Verify(api => api.PostWithResponseCode<int>(It.IsAny<PostProviderRequest>(), It.IsAny<bool>()), Times.Never);
    }

    [Test, MoqInlineAutoData]
    public async Task Handle_CallsApiClientPatchOrganisation_ChangingProviderTypeToMain(
        [Frozen] Mock<IRoatpServiceRestApiClient> roatpServiceRestApiClientMock,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpV2ApiClientMock,
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpServiceApiClientMock,
        [Greedy] PatchOrganisationCommandHandler sut,
        GetOrganisationQueryResult result,
        OrganisationResponse organisationResponse,
        CancellationToken cancellationToken
    )
    {
        int ukprn = 12345678;
        string userId = Guid.NewGuid().ToString();
        string userName = Guid.NewGuid().ToString();
        organisationResponse.Ukprn = ukprn;

        var patchDoc = new JsonPatchDocument<PatchOrganisationModel>();
        patchDoc.Replace(o => o.ProviderType, ProviderType.Main);
        roatpServiceRestApiClientMock.Setup(api => api.PatchOrganisation(ukprn, userId, patchDoc, cancellationToken)).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.NoContent));
        roatpServiceApiClientMock.Setup(a =>
            a.GetWithResponseCode<OrganisationResponse>(It.IsAny<GetOrganisationRequest>())).ReturnsAsync(
                        new ApiResponse<OrganisationResponse>(organisationResponse, HttpStatusCode.OK, ""));

        var postProviderApiResponse = new ApiResponse<int>(1, HttpStatusCode.Created, string.Empty);
        roatpV2ApiClientMock.Setup(c => c.PostWithResponseCode<int>(It.IsAny<PostProviderRequest>(
        ), true)).ReturnsAsync(postProviderApiResponse);

        var command = new PatchOrganisationCommand(ukprn, userId, userName, patchDoc);

        await sut.Handle(command, cancellationToken);

        roatpServiceRestApiClientMock.Verify(api => api.PatchOrganisation(ukprn, userId, patchDoc, cancellationToken), Times.Once);
        roatpServiceApiClientMock.Verify(api => api.GetWithResponseCode<OrganisationResponse>(It.Is<GetOrganisationRequest>(x => x.Ukprn == ukprn)), Times.Once);
        roatpV2ApiClientMock.Verify(api => api.PostWithResponseCode<int>(It.Is<PostProviderRequest>(
            x => ((CreateProviderModel)x.Data).Ukprn == ukprn
                 && ((CreateProviderModel)x.Data).LegalName == organisationResponse.LegalName
                 && ((CreateProviderModel)x.Data).TradingName == organisationResponse.TradingName
                 && ((CreateProviderModel)x.Data).UserId == userId
                 && ((CreateProviderModel)x.Data).UserDisplayName == userName
                 && x.UserDisplayName == userName
                 && x.UserId == userId
            ), true), Times.Once);
    }
}
