using System.Net;
using AutoFixture;
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

namespace SFA.DAS.ProviderPR.UnitTests.Application.Requests.Commands.AddAccount;

public class WhenEmailInRequestIsNotOwner
{
    private Mock<IProviderRelationshipsApiRestClient> _providerRelationshipsApiRestClient;
    private List<TeamMember> _otherOwnerMembers;
    private TeamMember _requestMember;

    [SetUp]
    public async Task Setup()
    {
        Fixture fixture = new();

        var createRequestCommand = fixture.Create<AddAccountRequestCommand>();
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
        var allTeamMembers = new List<TeamMember>(_otherOwnerMembers);
        allTeamMembers.Add(_requestMember);
        var apiResponse = new ApiResponse<List<TeamMember>>(allTeamMembers, HttpStatusCode.OK, null);
        Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient = new();
        accountsApiClient.Setup(c => c.GetWithResponseCode<List<TeamMember>>(It.Is<GetAccountTeamMembersByInternalAccountIdRequest>(r => r.GetUrl.Contains(createRequestCommand.AccountId.ToString())))).ReturnsAsync(apiResponse);

        AddAccountRequestCommandHandler sut = new(_providerRelationshipsApiRestClient.Object, accountsApiClient.Object);

        await sut.Handle(createRequestCommand, CancellationToken.None);
    }

    [Test]
    public void Handle_EmailInRequestIsNotOwner_NotificationsSentToAllOwners()
    {
        _providerRelationshipsApiRestClient.Verify(c => c.PostNotifications(It.Is<PostNotificationsCommand>(c => c.Notifications.Count(n => n.TemplateName == NotificationConstants.AddAccountInformationTemplateName) == 1), It.IsAny<CancellationToken>()));
    }

    [Test]
    public void Handle_EmailInRequestIsNotOwner_InformationNotificationsSentToEmailInRequest()
    {
        _providerRelationshipsApiRestClient.Verify(c => c.PostNotifications(It.Is<PostNotificationsCommand>(c => c.Notifications.Count(n => n.TemplateName == NotificationConstants.AddAccountOwnerInvitationTemplateName) == _otherOwnerMembers.Count), It.IsAny<CancellationToken>()));
    }
}
