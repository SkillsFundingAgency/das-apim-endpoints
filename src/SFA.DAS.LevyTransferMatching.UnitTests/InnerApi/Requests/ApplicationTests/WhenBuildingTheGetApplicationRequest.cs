using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;

namespace SFA.DAS.LevyTransferMatching.UnitTests.InnerApi.Requests.ApplicationTests
{
    class WhenBuildingTheGetApplicationRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(
            int pledgeId,
            int applicationId)
        {
            var actual = new GetApplicationRequest(pledgeId, applicationId);

            Assert.AreEqual(
                 $"pledges/{pledgeId}/applications/{applicationId}",
                actual.GetUrl);
        }

        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(
            int applicationId)
        {
            var actual = new GetApplicationRequest(applicationId);

            Assert.AreEqual(
                 $"applications/{applicationId}",
                actual.GetUrl);
        }
    }
}