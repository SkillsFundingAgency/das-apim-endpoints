using System.Collections.Generic;
using System.Linq;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Services;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services.EmployerAccountsServiceTests;

public class WhenGettingEmployerAccountUsers
{
    [Test, MoqAutoData]
    public async Task Then_If_The_Team_Members_Are_Retrieved(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetAccountTeamMembersResponse teamResponse,
        long accountId,
        EmployerAccountsService sut)
    {
        accountsApiClient
            .Setup(x => x.GetAll<GetAccountTeamMembersResponse>(
                It.Is<GetAccountTeamMembersRequest>(c => c.GetAllUrl.Contains($"accounts/internal/{accountId}/users"))))
            .ReturnsAsync(new List<GetAccountTeamMembersResponse> { teamResponse });

        var actual = (await sut.GetTeamMembers(accountId)).ToList();

        accountsApiClient
            .Verify(x => x.GetAll<GetAccountTeamMembersResponse>(It.Is<GetAccountTeamMembersRequest>(c => c.GetAllUrl.Contains($"accounts/internal/{accountId}/users")))
                , Times.Once);

        actual.First().Name.Should().Be(teamResponse.Name);
        actual.First().Status.Should().Be(teamResponse.Status);
        actual.First().Role.Should().Be(teamResponse.Role);
        actual.First().Email.Should().Be(teamResponse.Email);
        actual.First().UserRef.Should().Be(teamResponse.UserRef);
        actual.First().CanReceiveNotifications.Should().Be(teamResponse.CanReceiveNotifications);
    }
}