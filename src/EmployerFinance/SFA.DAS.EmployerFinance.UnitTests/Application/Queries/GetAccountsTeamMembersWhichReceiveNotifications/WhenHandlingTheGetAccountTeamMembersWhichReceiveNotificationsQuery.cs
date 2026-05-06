using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetAccountTeamMembersWhichReceiveNotifications;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.EmployerAccounts;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetPledges
{
    public class WhenHandlingTheGetAccountTeamMembersWhichReceiveNotificationsQuery
    {
        [Test, MoqAutoData]
        public async Task And_AccountId_Specified_Then_AccountTeamMembers_Returned(
            long accountId,
            GetAccountTeamMembersWhichReceiveNotificationsResponse response,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            GetAccountTeamMembersWhichReceiveNotificationsQueryHandler handler)
        {
            GetAccountTeamMembersWhichReceiveNotificationsQuery query = new GetAccountTeamMembersWhichReceiveNotificationsQuery()
            {
                AccountId = accountId,
            };

            accountsApiClient
                .Setup(x => x.Get<GetAccountTeamMembersWhichReceiveNotificationsResponse>(It.IsAny<GetAccountTeamMembersWhichReceiveNotificationsRequest>()))
                .ReturnsAsync(response);

            var results = await handler.Handle(query, CancellationToken.None);

            results.Should().BeEquivalentTo(response);
        }
    }
}