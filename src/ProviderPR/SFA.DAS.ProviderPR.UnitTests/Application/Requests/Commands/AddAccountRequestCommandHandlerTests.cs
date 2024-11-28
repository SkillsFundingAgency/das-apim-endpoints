using System.Net;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
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
using TeamMember = SFA.DAS.SharedOuterApi.InnerApi.Responses.GetAccountTeamMembersWhichReceiveNotificationsResponse.TeamMember;

namespace SFA.DAS.ProviderPR.UnitTests.Application.Requests.Commands;


public class AddAccountRequestCommandHandlerTests
{
    private Mock<IProviderRelationshipsApiRestClient> _providerRelationshipsApiRestClient;
    private Mock<IAccountsApiClient<AccountsConfiguration>> _accountsApiClient;
    private AddAccountRequestCommand _command;
    private AddAccountRequestCommandResult _response;

    [SetUp]
    public void SetUp()
    {
        Fixture fixture = new();
        _command = fixture.Create<AddAccountRequestCommand>();
        _response = fixture.Create<AddAccountRequestCommandResult>();

        _providerRelationshipsApiRestClient = new Mock<IProviderRelationshipsApiRestClient>();
        _accountsApiClient = new Mock<IAccountsApiClient<AccountsConfiguration>>();
        _providerRelationshipsApiRestClient.Setup(x => x.CreateAddAccountRequest(_command, CancellationToken.None)).ReturnsAsync(_response);
    }

    [Test]
    public async Task Handle_SendsAddAccountRequest()
    {
        var apiResponse = new ApiResponse<List<TeamMember>>([], HttpStatusCode.OK, string.Empty);
        _accountsApiClient.Setup(x => x.GetWithResponseCode<List<TeamMember>>(It.Is<GetAccountTeamMembersByInternalAccountIdRequest>(r => r.GetUrl.Contains(_command.AccountId.ToString())))).ReturnsAsync(apiResponse);

        AddAccountRequestCommandHandler sut = new(_providerRelationshipsApiRestClient.Object, _accountsApiClient.Object);

        var result = await sut.Handle(_command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RequestId, Is.EqualTo(_response.RequestId));
        });

        _accountsApiClient.VerifyAll();
    }

    [Test]
    public async Task Handle_ErrorGettingTeamMembers_ThrowsException()
    {
        var apiResponse = new ApiResponse<List<TeamMember>>([], HttpStatusCode.BadRequest, string.Empty);
        _accountsApiClient.Setup(x => x.GetWithResponseCode<List<TeamMember>>(It.Is<GetAccountTeamMembersByInternalAccountIdRequest>(r => r.GetUrl.Contains(_command.AccountId.ToString())))).ReturnsAsync(apiResponse);

        AddAccountRequestCommandHandler sut = new(_providerRelationshipsApiRestClient.Object, _accountsApiClient.Object);

        Func<Task> action = () => sut.Handle(_command, CancellationToken.None);

        await action.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test]
    public async Task Handle_NoTeamMembers_NoNotificationsSent()
    {
        var apiResponse = new ApiResponse<List<TeamMember>>([], HttpStatusCode.OK, string.Empty);
        _accountsApiClient.Setup(x => x.GetWithResponseCode<List<TeamMember>>(It.Is<GetAccountTeamMembersByInternalAccountIdRequest>(r => r.GetUrl.Contains(_command.AccountId.ToString())))).ReturnsAsync(apiResponse);

        AddAccountRequestCommandHandler sut = new(_providerRelationshipsApiRestClient.Object, _accountsApiClient.Object);

        await sut.Handle(_command, CancellationToken.None);

        _accountsApiClient.VerifyAll();
        _providerRelationshipsApiRestClient.Verify(a => a.PostNotifications(It.IsAny<PostNotificationsCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase("not_a_member@email.com")]
    public async Task Handle_NoEmailInRequestOrEmailNotInTeam_SendNotificationsToAllAccountOwners(string? email)
    {
        Fixture fixture = new();

        _command.EmployerContactEmail = fixture.Create<string>();

        var allOwnerMembers = fixture
            .Build<TeamMember>()
            .With(t => t.Role, nameof(Role.Owner))
            .With(t => t.Status, InvitationStatus.Accepted)
            .With(t => t.CanReceiveNotifications, true)
            .CreateMany().ToList();

        var apiResponse = new ApiResponse<List<TeamMember>>(allOwnerMembers, HttpStatusCode.OK, string.Empty);
        _accountsApiClient.Setup(x => x.GetWithResponseCode<List<TeamMember>>(It.Is<GetAccountTeamMembersByInternalAccountIdRequest>(r => r.GetUrl.Contains(_command.AccountId.ToString())))).ReturnsAsync(apiResponse);

        AddAccountRequestCommandHandler sut = new(_providerRelationshipsApiRestClient.Object, _accountsApiClient.Object);

        await sut.Handle(_command, CancellationToken.None);

        _providerRelationshipsApiRestClient.Verify(c => c.PostNotifications(It.Is<PostNotificationsCommand>(c => c.Notifications.Count(n => n.TemplateName == NotificationConstants.AddAccountOwnerInvitationTemplateName) == allOwnerMembers.Count), It.IsAny<CancellationToken>()));
    }

    [Test, AutoData]
    public async Task Handle_EmailInRequestIsOwnerAndHasNotificationsTurnedOn_InviteNotificationSentToEmailInRequest(List<TeamMember> teamMembers)
    {
        TeamMember requestedTeamMember = new()
        {
            Email = _command.EmployerContactEmail,
            CanReceiveNotifications = true,
            Role = nameof(Role.Owner),
            Status = InvitationStatus.Accepted
        };
        teamMembers.Add(requestedTeamMember);

        var apiResponse = new ApiResponse<List<TeamMember>>(teamMembers, HttpStatusCode.OK, string.Empty);
        _accountsApiClient.Setup(x => x.GetWithResponseCode<List<TeamMember>>(It.IsAny<GetAccountTeamMembersByInternalAccountIdRequest>())).ReturnsAsync(apiResponse);

        AddAccountRequestCommandHandler sut = new(_providerRelationshipsApiRestClient.Object, _accountsApiClient.Object);
        await sut.Handle(_command, CancellationToken.None);

        _providerRelationshipsApiRestClient.Verify(a =>
            a.PostNotifications(
                It.Is<PostNotificationsCommand>(c =>
                    c.Notifications.Count == 1
                    && c.Notifications[0].TemplateName == NotificationConstants.AddAccountInvitationTemplateName
                    && c.Notifications[0].RequestId == _response.RequestId
                    && c.Notifications[0].EmailAddress == _command.EmployerContactEmail),
                CancellationToken.None
            ),
            Times.Once
        );
    }

    [Test, AutoData]
    public async Task Handle_EmailInRequestIsOwnerAndNotificationsTurnedOff_NoNotificationsSent(List<TeamMember> teamMembers)
    {
        TeamMember requestedTeamMember = new()
        {
            Email = _command.EmployerContactEmail,
            CanReceiveNotifications = false,
            Role = nameof(Role.Owner),
            Status = InvitationStatus.Accepted
        };
        teamMembers.Add(requestedTeamMember);

        var apiResponse = new ApiResponse<List<TeamMember>>(teamMembers, HttpStatusCode.OK, string.Empty);
        _accountsApiClient.Setup(x => x.GetWithResponseCode<List<TeamMember>>(It.IsAny<GetAccountTeamMembersByInternalAccountIdRequest>())).ReturnsAsync(apiResponse);

        AddAccountRequestCommandHandler sut = new(_providerRelationshipsApiRestClient.Object, _accountsApiClient.Object);

        await sut.Handle(_command, CancellationToken.None);

        _providerRelationshipsApiRestClient.Verify(a => a.PostNotifications(It.IsAny<PostNotificationsCommand>(), CancellationToken.None), Times.Never);
    }
}
