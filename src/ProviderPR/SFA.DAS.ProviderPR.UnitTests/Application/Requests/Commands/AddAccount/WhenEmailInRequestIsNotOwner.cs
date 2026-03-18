using System.Net;
using AutoFixture;
using Moq;
using SFA.DAS.ProviderPR.Application.Requests.Commands.AddAccount;
using SFA.DAS.ProviderPR.Common;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Notifications.Commands;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Types.Models;
using TeamMember = SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.EmployerAccounts.GetAccountTeamMembersWhichReceiveNotificationsResponse.TeamMember;

namespace SFA.DAS.ProviderPR.UnitTests.Application.Requests.Commands.AddAccount;

public class WhenEmailInRequestIsNotOwner
{
    private Mock<IProviderRelationshipsApiRestClient> _providerRelationshipsApiRestClient;
    private List<TeamMember> _otherOwnerMembers;
    private TeamMember _requestMember;
    private TeamMember _otherOwnerMemberWithNotificationTurnedOff;
    private AddAccountRequestCommandHandler sut;
    private AddAccountRequestCommand createRequestCommand;

    [SetUp]
    public void Setup()
    {
        Fixture fixture = new();

        createRequestCommand = fixture.Create<AddAccountRequestCommand>();
        var createRequestCommandResult = fixture.Create<AddAccountRequestCommandResult>();
        _providerRelationshipsApiRestClient = new();
        _providerRelationshipsApiRestClient.Setup(x => x.CreateAddAccountRequest(createRequestCommand, CancellationToken.None)).ReturnsAsync(createRequestCommandResult);

        _requestMember = fixture
            .Build<TeamMember>()
            .With(t => t.Email, createRequestCommand.EmployerContactEmail)
            .With(t => t.Role, nameof(Role.None))
            .With(t => t.Status, InvitationStatus.Accepted)
            .With(t => t.CanReceiveNotifications, true)
            .Create();
        _otherOwnerMembers = fixture
            .Build<TeamMember>()
            .With(t => t.Role, nameof(Role.Owner))
            .With(t => t.Status, InvitationStatus.Accepted)
            .With(t => t.CanReceiveNotifications, true)
            .CreateMany().ToList();
        _otherOwnerMemberWithNotificationTurnedOff = fixture
            .Build<TeamMember>()
            .With(t => t.Role, nameof(Role.Owner))
            .With(t => t.Status, InvitationStatus.Accepted)
            .With(t => t.CanReceiveNotifications, false)
            .Create();
        var allTeamMembers = new List<TeamMember>(_otherOwnerMembers);
        allTeamMembers.Add(_requestMember);
        var apiResponse = new ApiResponse<List<TeamMember>>(allTeamMembers, HttpStatusCode.OK, null);
        Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient = new();
        accountsApiClient.Setup(c => c.GetWithResponseCode<List<TeamMember>>(It.Is<GetAccountTeamMembersByInternalAccountIdRequest>(r => r.GetUrl.Contains(createRequestCommand.AccountId.ToString())))).ReturnsAsync(apiResponse);

        sut = new(_providerRelationshipsApiRestClient.Object, accountsApiClient.Object);

    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Handle_EmailInRequestIsNotOwner_NotificationsSentToAllOwners(bool isNotificationPreferenceOn)
    {
        _requestMember.CanReceiveNotifications = isNotificationPreferenceOn;

        await sut.Handle(createRequestCommand, CancellationToken.None);

        _providerRelationshipsApiRestClient.Verify(c => c.PostNotifications(It.Is<PostNotificationsCommand>(c => c.Notifications.Count(n => n.TemplateName == NotificationConstants.AddAccountOwnerInvitationTemplateName) == _otherOwnerMembers.Count), It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Handle_EmailInRequestIsNotOwnerAndSomeOwnersHaveNotificationTurnedOff_NotificationsNotSentToOwnersWithNotificationOff()
    {
        await sut.Handle(createRequestCommand, CancellationToken.None);

        _providerRelationshipsApiRestClient.Verify(c => c.PostNotifications(It.Is<PostNotificationsCommand>(c => !c.Notifications.Any(n => n.EmailAddress == _otherOwnerMemberWithNotificationTurnedOff.Email)), It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Handle_EmailInRequestIsNotOwner_InformationNotificationsSentToEmailInRequest()
    {
        await sut.Handle(createRequestCommand, CancellationToken.None);

        _providerRelationshipsApiRestClient.Verify(c => c.PostNotifications(It.Is<PostNotificationsCommand>(c => c.Notifications.Count(n => n.TemplateName == NotificationConstants.AddAccountInformationTemplateName) == 1), It.IsAny<CancellationToken>()));
    }
}
