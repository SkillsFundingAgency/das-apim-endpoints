using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Forecasting.InnerApi.Requests;

namespace SFA.DAS.Forecasting.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostAccountBalanceRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(string accountId)
        {
            var actual = new PostAccountBalanceRequest(accountId);

            actual.PostUrl.Should().Be($"api/accounts/balances");
            actual.Data.Should().BeEquivalentTo(new List<string>{accountId});
        }
    }
}