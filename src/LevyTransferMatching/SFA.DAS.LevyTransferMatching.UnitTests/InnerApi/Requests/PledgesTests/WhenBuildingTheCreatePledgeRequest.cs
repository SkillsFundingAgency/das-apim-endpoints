using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges;

namespace SFA.DAS.LevyTransferMatching.UnitTests.InnerApi.Requests.PledgesTests
{
    public class WhenBuildingTheCreatePledgeRequest
    {
        [Test, AutoData]
        public void Then_The_PostUrl_Is_Correctly_Built(long accountId)
        {
            var actual = new CreatePledgeRequest(accountId, new CreatePledgeRequest.CreatePledgeRequestData());

            Assert.That(actual.PostUrl, Is.EqualTo($"accounts/{accountId}/pledges"));
        }
    }
}