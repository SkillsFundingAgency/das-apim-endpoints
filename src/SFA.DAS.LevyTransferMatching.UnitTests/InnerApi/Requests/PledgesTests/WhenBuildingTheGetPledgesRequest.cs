using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges;

namespace SFA.DAS.LevyTransferMatching.UnitTests.InnerApi.Requests.PledgesTests
{
    public class WhenBuildingTheGetPledgesRequest
    {
        [Test, AutoData]
        public void And_No_AccountId_Supplied_Then_The_GetUrl_Is_Correctly_Built()
        {
            var actual = new GetPledgesRequest();

            Assert.AreEqual(
                $"pledges",
                actual.GetUrl);
        }

        [Test, AutoData]
        public void And_The_AccountId_Supplied_Then_The_GetUrl_Is_Correctly_Built(long accountId)
        {
            var actual = new GetPledgesRequest(accountId: accountId);

            Assert.AreEqual(
                $"pledges?accountId={accountId}",
                actual.GetUrl);
        }
    }
}