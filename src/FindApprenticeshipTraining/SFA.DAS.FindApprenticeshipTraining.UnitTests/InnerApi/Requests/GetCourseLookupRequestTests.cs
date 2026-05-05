using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class GetCourseLookupRequestTests
    {
        [Test, AutoData]
        public void GetUrl_GivenLarsCode_ReturnsExpectedPath(string larsCode)
        {
            var req = new GetCourseLookupRequest(larsCode);
            req.GetUrl.Should().Be($"api/courses/lookup/{larsCode}");
        }
    }
}