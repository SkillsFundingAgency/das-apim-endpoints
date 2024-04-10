using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Accounts;

namespace SFA.DAS.LevyTransferMatching.UnitTests.InnerApi.Requests.AccountsTests
{
    public class WhenBuildingTheGetAccountRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(string encodedAccountId)
        {
            var actual = new GetAccountRequest(encodedAccountId);

            Assert.That(actual.GetUrl, Is.EqualTo($"api/accounts/{encodedAccountId}"));
        }
    }
}