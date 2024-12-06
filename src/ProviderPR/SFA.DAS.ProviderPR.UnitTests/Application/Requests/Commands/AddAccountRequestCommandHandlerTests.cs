using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.ProviderPR.Application.Requests.Commands.AddAccount;
using SFA.DAS.ProviderPR.Common;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Notifications.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using TeamMember = SFA.DAS.SharedOuterApi.InnerApi.Responses.GetAccountTeamMembersWhichReceiveNotificationsResponse.TeamMember;

namespace SFA.DAS.ProviderPR.UnitTests.Application.Requests.Commands;

public class AddAccountRequestCommandHandlerTests
{
    private Mock<IProviderRelationshipsApiRestClient> _providerRelationshipsApiRestClient;
    private Mock<IAccountsApiClient<AccountsConfiguration>> _accountsApiClient;

    [SetUp]
    public void SetUp()
    {
        _providerRelationshipsApiRestClient = new Mock<IProviderRelationshipsApiRestClient>();
        _accountsApiClient = new Mock<IAccountsApiClient<AccountsConfiguration>>();
    }

    [Test]
    [AutoData]
    public async Task Handle_AddAccountRequestRequest_NoTeamMembers_Successful(
        AddAccountRequestCommand command,
        AddAccountRequestCommandResult response
    )
    {
        _providerRelationshipsApiRestClient.Setup(x =>
            x.CreateAddAccountRequest(
                It.IsAny<AddAccountRequestCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(response);

        var apiResponse = new ApiResponse<List<TeamMember>>([], HttpStatusCode.OK, string.Empty);

        _accountsApiClient.Setup(x =>
            x.GetWithResponseCode<List<TeamMember>>(
                It.IsAny<GetAccountTeamMembersByInternalAccountIdRequest>()
            )
        ).ReturnsAsync(apiResponse);

        AddAccountRequestCommandHandler sut = new(_providerRelationshipsApiRestClient.Object, _accountsApiClient.Object);
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
    public async Task Handle_AddAccountRequestRequest_WithTeamMemberThatIsOwnerAndAccepted_AddAccountOwnerInvitationNotification(
        AddAccountRequestCommand command,
        AddAccountRequestCommandResult response,
        TeamMember teamMember
    )
    {
        teamMember.CanReceiveNotifications = true;
        teamMember.Role = nameof(Role.Owner);
        teamMember.Status = InvitationStatus.Accepted;
        command.EmployerContactEmail = null;

        _providerRelationshipsApiRestClient.Setup(x =>
            x.CreateAddAccountRequest(
                It.IsAny<AddAccountRequestCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(response);

        var apiResponse = new ApiResponse<List<TeamMember>>([teamMember], HttpStatusCode.OK, string.Empty);

        _accountsApiClient.Setup(x =>
            x.GetWithResponseCode<List<TeamMember>>(
                It.IsAny<GetAccountTeamMembersByInternalAccountIdRequest>()
            )
        ).ReturnsAsync(apiResponse);

        AddAccountRequestCommandHandler sut = new(_providerRelationshipsApiRestClient.Object, _accountsApiClient.Object);
        var result = await sut.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RequestId, Is.EqualTo(response.RequestId));
        });

        _providerRelationshipsApiRestClient.Verify(a =>
            a.PostNotifications(
                It.Is<PostNotificationsCommand>(c => c.Notifications[0].TemplateName == NotificationConstants.AddAccountOwnerInvitationTemplateName),
                CancellationToken.None
            ),
            Times.Once
        );
    }

    [Test]
    [AutoData]
    public async Task Handle_AddAccountRequest_TeamMemberAssociatedWithEmployerContactEmailAndAccountOwner_AddAccountInvitationEmailSent(
        AddAccountRequestCommand command,
        AddAccountRequestCommandResult response,
        TeamMember teamMember
    )
    {
        teamMember.CanReceiveNotifications = true;
        teamMember.Role = nameof(Role.Owner);
        teamMember.Status = InvitationStatus.Accepted;
        teamMember.Email = command.EmployerContactEmail;

        _providerRelationshipsApiRestClient.Setup(x =>
            x.CreateAddAccountRequest(
                It.IsAny<AddAccountRequestCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(response);

        var apiResponse = new ApiResponse<List<TeamMember>>([teamMember], HttpStatusCode.OK, string.Empty);

        _accountsApiClient.Setup(x =>
            x.GetWithResponseCode<List<TeamMember>>(
                It.IsAny<GetAccountTeamMembersByInternalAccountIdRequest>()
            )
        ).ReturnsAsync(apiResponse);

        AddAccountRequestCommandHandler sut = new(_providerRelationshipsApiRestClient.Object, _accountsApiClient.Object);
        var result = await sut.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RequestId, Is.EqualTo(response.RequestId));
        });
        
        _providerRelationshipsApiRestClient.Verify(a =>
            a.PostNotifications(
                It.Is<PostNotificationsCommand>(c => c.Notifications.Any(a => a.TemplateName == NotificationConstants.AddAccountInvitationTemplateName)),
                CancellationToken.None
            ),
            Times.Once
        );
    }

    [Test]
    [AutoData]
    public async Task Handle_AddAccountRequest_TeamMemberAssociatedWithEmployerContactEmailAndNotAccountOwner_AddAccountInformationEmailSent(
        AddAccountRequestCommand command,
        AddAccountRequestCommandResult response,
        TeamMember teamMember
    )
    {
        teamMember.CanReceiveNotifications = true;
        teamMember.Role = nameof(Role.Viewer);
        teamMember.Status = InvitationStatus.Accepted;
        teamMember.Email = command.EmployerContactEmail;

        _providerRelationshipsApiRestClient.Setup(x =>
            x.CreateAddAccountRequest(
                It.IsAny<AddAccountRequestCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(response);

        var apiResponse = new ApiResponse<List<TeamMember>>([teamMember], HttpStatusCode.OK, string.Empty);

        _accountsApiClient.Setup(x =>
            x.GetWithResponseCode<List<TeamMember>>(
                It.IsAny<GetAccountTeamMembersByInternalAccountIdRequest>()
            )
        ).ReturnsAsync(apiResponse);

        AddAccountRequestCommandHandler sut = new(_providerRelationshipsApiRestClient.Object, _accountsApiClient.Object);
        var result = await sut.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RequestId, Is.EqualTo(response.RequestId));
        });

        _providerRelationshipsApiRestClient.Verify(a =>
            a.PostNotifications(
                It.Is<PostNotificationsCommand>(c => c.Notifications.Any(a => a.TemplateName == NotificationConstants.AddAccountInformationTemplateName)),
                CancellationToken.None
            ),
            Times.Once
        );
    }

    [Test]
    [AutoData]
    public async Task Handle_AddAccountRequestRequest_TeamMemberAssociatedWithEmployerContactEmail_MultipleNotificationsNonOwner(
        AddAccountRequestCommand command,
        AddAccountRequestCommandResult response,
        TeamMember teamMember
    )
    {
        teamMember.CanReceiveNotifications = true;
        teamMember.Role = nameof(Role.Owner);
        teamMember.Status = InvitationStatus.Accepted;
        teamMember.Email = $"{command.EmployerContactEmail}1";

        _providerRelationshipsApiRestClient.Setup(x =>
            x.CreateAddAccountRequest(
                It.IsAny<AddAccountRequestCommand>(),
                CancellationToken.None
            )
        ).ReturnsAsync(response);

        var apiResponse = new ApiResponse<List<TeamMember>>([teamMember], HttpStatusCode.OK, string.Empty);

        _accountsApiClient.Setup(x =>
            x.GetWithResponseCode<List<TeamMember>>(
                It.IsAny<GetAccountTeamMembersByInternalAccountIdRequest>()
            )
        ).ReturnsAsync(apiResponse);

        AddAccountRequestCommandHandler sut = new(_providerRelationshipsApiRestClient.Object, _accountsApiClient.Object);
        var result = await sut.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RequestId, Is.EqualTo(response.RequestId));
        });

        _providerRelationshipsApiRestClient.Verify(a =>
            a.PostNotifications(
                It.Is<PostNotificationsCommand>(c => c.Notifications.Any(a => a.TemplateName == NotificationConstants.AddAccountOwnerInvitationTemplateName)),
                CancellationToken.None
            ),
            Times.Once
        );
    }
}
