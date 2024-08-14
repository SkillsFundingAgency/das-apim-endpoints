using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.ProviderPR.Application.Requests.Commands.AddAccount;
using SFA.DAS.ProviderPR.Application.Requests.Commands.CreatePermissions;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Notifications.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.SharedOuterApi.InnerApi.Responses.GetAccountTeamMembersWhichReceiveNotificationsResponse;

namespace SFA.DAS.ProviderPR.UnitTests.Application.Requests.Commands;

public class CreatePermissionRequestCommandHandlerTests
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
    public async Task Handle_CreatePermissionRequest_NoTeamMembers_Successful(
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

        _accountsApiClient.Setup(x =>
            x.GetAll<TeamMember>(
                It.IsAny<GetAccountTeamMembersByInternalAccountIdRequest>()
            )
        ).ReturnsAsync([]);

        CreatePermissionRequestCommandHandler sut = new(_providerRelationshipsApiRestClient.Object, _accountsApiClient.Object);
        var result = await sut.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RequestId, Is.EqualTo(response.RequestId));
        });

        _providerRelationshipsApiRestClient.Verify(a => a.PostNotifications(It.IsAny<PostNotificationsCommand>(), CancellationToken.None), Times.Never);
    }
}
