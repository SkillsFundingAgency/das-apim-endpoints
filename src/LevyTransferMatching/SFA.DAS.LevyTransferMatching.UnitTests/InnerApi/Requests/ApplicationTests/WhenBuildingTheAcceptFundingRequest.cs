using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;

namespace SFA.DAS.LevyTransferMatching.UnitTests.InnerApi.Requests.ApplicationTests
{
    public class WhenBuildingTheAcceptFundingRequest
    {
        [Test, AutoData]
        public void Then_The_PostUrl_Is_Correctly_Built(
            int applicationId,
            long accountId,
            AcceptFundingRequestData data)
        {
            var actual = new AcceptFundingRequest(applicationId, accountId, data);

            Assert.That(actual.PostUrl, Is.EqualTo($"/accounts/{accountId}/applications/{applicationId}/accept-funding"));
        }
    }
}