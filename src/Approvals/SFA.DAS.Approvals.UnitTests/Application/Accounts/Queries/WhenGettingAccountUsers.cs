using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Accounts.Queries.GetAccountUsersQuery;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.Accounts.Queries
{
    public class WhenGettingAccountUsers
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_The_AccountUsers_Are_Returned(
            GetAccountUsersQuery query,
            GetAccountUsersResponse apiResponse,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> apiClient,
            GetAccountUsersQueryHandler handler
        )
        {
            apiClient.Setup(x => x.Get<GetAccountUsersResponse>(It.Is<GetAccountUsersRequest>(x => x.HashedAccountId == query.HashedAccountId))).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}