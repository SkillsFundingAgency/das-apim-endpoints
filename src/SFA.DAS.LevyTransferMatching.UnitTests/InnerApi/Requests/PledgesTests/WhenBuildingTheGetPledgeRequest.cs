using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges;

namespace SFA.DAS.LevyTransferMatching.UnitTests.InnerApi.Requests.PledgesTests
{
    public class WhenBuildingTheGetPledgeRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(int id)
        {
            var actual = new GetPledgeRequest(id);

            Assert.AreEqual(
                $"pledges/{id}",
                actual.GetUrl);
        }
    }
}