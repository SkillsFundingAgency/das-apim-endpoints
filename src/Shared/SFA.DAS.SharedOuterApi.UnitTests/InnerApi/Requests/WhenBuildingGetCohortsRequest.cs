using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetCohortsRequest
    {
        [Test]
        [TestCase(null, null, "api/cohorts")]
        [TestCase(123, 456, "api/cohorts?accountId=123&providerId=456")]
        [TestCase(123, null, "api/cohorts?accountId=123")]
        [TestCase(null, 456, "api/cohorts?providerId=456")]
        public void Then_The_Request_Is_Correctly_Build(int? accountId, int? providerId, string expectedURL)
        {
            var actual = new GetCohortsRequest
            {
                AccountId = accountId,
                ProviderId = providerId
            };

            actual.GetUrl.Should().Be(expectedURL);
        }
    }
}
