using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;

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

        var actual = (await sut.GetAccountUsers(accountId)).ToList();

        accountsApiClient
            .Verify(x => x.GetAll<GetAccountTeamMembersResponse>(It.Is<GetAccountTeamMembersRequest>(c => c.GetAllUrl.Contains($"accounts/internal/{accountId}/users")))
                , Times.Once);

        actual.First().Role.Should().Be(teamResponse.Role);
        actual.First().Email.Should().Be(teamResponse.Email);
        actual.First().UserRef.Should().Be(teamResponse.UserRef);
        actual.First().CanReceiveNotifications.Should().Be(teamResponse.CanReceiveNotifications);
    }
}