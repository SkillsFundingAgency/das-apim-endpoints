using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetAccountTeamMembersWhichReceiveNotifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

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