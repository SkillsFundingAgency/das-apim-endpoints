using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Services;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Accounts;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Services.AccountsServiceTests
{
    public class WhenCallingGetAccount
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_The_Account(
            string encodedAccountId,
            Account apiResponse,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> mockAccountsApiClient,
            AccountsService accountsService)
        {
            mockAccountsApiClient
                .Setup(x => x.Get<Account>(It.Is<GetAccountRequest>(y => y.GetUrl.Contains(encodedAccountId))))
                .ReturnsAsync(apiResponse);

            var actual = await accountsService.GetAccount(encodedAccountId);

            Assert.AreEqual(actual, apiResponse);
        }
    }
}