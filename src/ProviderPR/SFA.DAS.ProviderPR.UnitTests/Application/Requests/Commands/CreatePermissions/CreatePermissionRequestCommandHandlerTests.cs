using System.Net;
using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.ProviderPR.Application.Requests.Commands.CreatePermissions;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Notifications.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using TeamMember = SFA.DAS.SharedOuterApi.InnerApi.Responses.GetAccountTeamMembersWhichReceiveNotificationsResponse.TeamMember;

namespace SFA.DAS.ProviderPR.UnitTests.Application.Requests.Commands.CreatePermissions;

public class CreatePermissionRequestCommandHandlerTests
{
    private static Mock<IProviderRelationshipsApiRestClient> _providerRelationshipsApiRestClient;
    private static Mock<IAccountsApiClient<AccountsConfiguration>> _accountsApiClient;

    [SetUp]
    public void SetUp()
    {
        _providerRelationshipsApiRestClient = new Mock<IProviderRelationshipsApiRestClient>();
        _accountsApiClient = new Mock<IAccountsApiClient<AccountsConfiguration>>();
    }

    [Test]
    [AutoData]
    public async Task Handle_CreatePermissionRequest_EmptyTeamMembers_SendsNoNotifications(
        CreatePermissionRequestCommand command,
        CreatePermissionRequestCommandResult response
    )
    {
        _providerRelationshipsApiRestClient.Setup(x =>
            x.CreatePermissionsRequest(
                It.IsAny<CreatePermissionRequestCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(response);

        var apiResponse = new ApiResponse<List<TeamMember>>([], HttpStatusCode.OK, string.Empty);

        _accountsApiClient.Setup(x =>
            x.GetWithResponseCode<List<TeamMember>>(
                It.IsAny<GetAccountTeamMembersByInternalAccountIdRequest>()
            )
        ).ReturnsAsync(apiResponse);

        CreatePermissionRequestCommandHandler sut = CreateHandler();
        var result = await sut.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RequestId, Is.EqualTo(response.RequestId));
        });

        _providerRelationshipsApiRestClient.Verify(a => a.PostNotifications(It.IsAny<PostNotificationsCommand>(), CancellationToken.None), Times.Never);
    }

    [Test]
    [AutoData]
    public async Task Handle_CreatePermissionRequest_AcceptedAccountOwner_SendsNotification(
        CreatePermissionRequestCommand command,
        CreatePermissionRequestCommandResult response,
        TeamMember teamMember
    )
    {
        teamMember.Role = nameof(Role.Owner);
        teamMember.Status = InvitationStatus.Accepted;
        teamMember.CanReceiveNotifications = true;

        _providerRelationshipsApiRestClient.Setup(x =>
            x.CreatePermissionsRequest(
                It.IsAny<CreatePermissionRequestCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(response);

        var apiResponse = new ApiResponse<List<TeamMember>>([teamMember], HttpStatusCode.OK, string.Empty);

        _accountsApiClient.Setup(x =>
            x.GetWithResponseCode<List<TeamMember>>(
                It.IsAny<GetAccountTeamMembersByInternalAccountIdRequest>()
            )
        ).ReturnsAsync(apiResponse);

        CreatePermissionRequestCommandHandler sut = CreateHandler();
        var result = await sut.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RequestId, Is.EqualTo(response.RequestId));
        });

        _providerRelationshipsApiRestClient.Verify(a => a.PostNotifications(It.IsAny<PostNotificationsCommand>(), CancellationToken.None), Times.Once);
    }

    [Test]
    [AutoData]
    public async Task Handle_CreatePermissionRequest_NonOwnerTeamMember_SendsNoNotifications(
        CreatePermissionRequestCommand command,
        CreatePermissionRequestCommandResult response,
        TeamMember teamMember
    )
    {
        teamMember.Role = "DefaultMember";
        teamMember.Status = InvitationStatus.Accepted;
        teamMember.CanReceiveNotifications = true;

        _providerRelationshipsApiRestClient.Setup(x =>
            x.CreatePermissionsRequest(
                It.IsAny<CreatePermissionRequestCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(response);

        var apiResponse = new ApiResponse<List<TeamMember>>([teamMember], HttpStatusCode.OK, string.Empty);

        _accountsApiClient.Setup(x =>
            x.GetWithResponseCode<List<TeamMember>>(
                It.IsAny<GetAccountTeamMembersByInternalAccountIdRequest>()
            )
        ).ReturnsAsync(apiResponse);

        CreatePermissionRequestCommandHandler sut = CreateHandler();
        var result = await sut.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RequestId, Is.EqualTo(response.RequestId));
        });

        _providerRelationshipsApiRestClient.Verify(a => a.PostNotifications(It.IsAny<PostNotificationsCommand>(), CancellationToken.None), Times.Never);
    }

    [Test]
    [AutoData]
    public async Task Handle_CreatePermissionRequest_PendingTeamMember_SendsNoNotifications(
        CreatePermissionRequestCommand command,
        CreatePermissionRequestCommandResult response,
        TeamMember teamMember
    )
    {
        teamMember.Role = Role.Owner.ToString("D");
        teamMember.Status = InvitationStatus.Pending;
        teamMember.CanReceiveNotifications = true;

        _providerRelationshipsApiRestClient.Setup(x =>
            x.CreatePermissionsRequest(
                It.IsAny<CreatePermissionRequestCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(response);

        var apiResponse = new ApiResponse<List<TeamMember>>([teamMember], HttpStatusCode.OK, string.Empty);

        _accountsApiClient.Setup(x =>
            x.GetWithResponseCode<List<TeamMember>>(
                It.IsAny<GetAccountTeamMembersByInternalAccountIdRequest>()
            )
        ).ReturnsAsync(apiResponse);

        CreatePermissionRequestCommandHandler sut = CreateHandler();
        var result = await sut.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RequestId, Is.EqualTo(response.RequestId));
        });

        _providerRelationshipsApiRestClient.Verify(a => a.PostNotifications(It.IsAny<PostNotificationsCommand>(), CancellationToken.None), Times.Never);
    }

    [Test]
    [AutoData]
    public async Task Handle_CreatePermissionRequest_CannotReceiveNotificationsTeamMember_SendsNoNotifications(
        CreatePermissionRequestCommand command,
        CreatePermissionRequestCommandResult response,
        TeamMember teamMember
    )
    {
        teamMember.Role = Role.Owner.ToString("D");
        teamMember.Status = InvitationStatus.Accepted;
        teamMember.CanReceiveNotifications = false;

        _providerRelationshipsApiRestClient.Setup(x =>
            x.CreatePermissionsRequest(
                It.IsAny<CreatePermissionRequestCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(response);

        var apiResponse = new ApiResponse<List<TeamMember>>([teamMember], HttpStatusCode.OK, string.Empty);

        _accountsApiClient.Setup(x =>
            x.GetWithResponseCode<List<TeamMember>>(
                It.IsAny<GetAccountTeamMembersByInternalAccountIdRequest>()
            )
        ).ReturnsAsync(apiResponse);

        CreatePermissionRequestCommandHandler sut = CreateHandler();
        var result = await sut.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RequestId, Is.EqualTo(response.RequestId));
        });

        _providerRelationshipsApiRestClient.Verify(a => a.PostNotifications(It.IsAny<PostNotificationsCommand>(), CancellationToken.None), Times.Never);
    }

    private static CreatePermissionRequestCommandHandler CreateHandler()
    {
        return new(_providerRelationshipsApiRestClient.Object, _accountsApiClient.Object);
    }
}
