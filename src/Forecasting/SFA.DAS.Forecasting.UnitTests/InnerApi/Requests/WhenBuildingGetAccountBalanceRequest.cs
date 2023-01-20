using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Forecasting.InnerApi.Requests;

namespace SFA.DAS.Forecasting.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetAccountBalanceRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(string accountId)
        {
            var actual = new GetAccountBalanceRequest(accountId);

            actual.GetUrl.Should().Be($"api/accounts/balances?accountIds={accountId}");
        }
    }
}