using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetTransferFinancialBreakdownRequest
    {
        [Test, AutoData]
        public void And_When_AccountId_Supplied_Then_The_GetUrl_Is_Correctly_Built(long accountId, DateTime startDate)
        {
            var actual = new GetTransferFinancialBreakdownRequest(accountId: accountId, startDate: startDate);

            Assert.That(
                $"api/accounts/{accountId}/accountprojection/detail?numberOfMonths=12&startDate={startDate.StartOfAprilOfFinancialYear().ToString("yyyy-MM-dd")}",
                Is.EqualTo(actual.GetUrl));
        }
    }
}
