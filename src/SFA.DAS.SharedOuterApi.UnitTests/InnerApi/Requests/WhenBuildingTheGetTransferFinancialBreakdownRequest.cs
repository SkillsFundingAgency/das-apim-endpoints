using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetTransferFinancialBreakdownRequest
    {
        [Test, AutoData]
        public void And_When_AccountId_Supplied_Then_The_GetUrl_Is_Correctly_Built(long accountId)
        {
            var actual = new GetTransferFinancialBreakdownRequest(accountId: accountId);

            Assert.AreEqual(
                $"accounts/{accountId}/accountprojection/detail?numberOfMonths=12&startDate={DateTime.Now.Year}-04-06",
                actual.GetUrl);
        }
    }
}
