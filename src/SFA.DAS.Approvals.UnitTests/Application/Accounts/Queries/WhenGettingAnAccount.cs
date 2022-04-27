using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Accounts.Queries.GetAccountQuery;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.Accounts.Queries
{
    public class WhenGettingAnAccount
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_The_Account_Is_Returned(
            GetAccountQuery query,
            GetAccountResponse apiResponse,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> apiClient,
            GetAccountQueryHandler handler
        )
        {
            apiClient.Setup(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(x => x.HashedAccountId == query.HashedAccountId))).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}