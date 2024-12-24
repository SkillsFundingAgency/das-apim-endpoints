using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi
{
    [TestFixture]
    public class WhenBuildingGetInactiveCandidatesApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(string cutOffDateTime, int pageNumber, int pageSize)
        {
            var actual = new GetInactiveCandidatesApiRequest(cutOffDateTime, pageNumber, pageSize);

            actual.GetUrl.Should().Be($"api/candidates/GetInactiveCandidates?cutOffDateTime={cutOffDateTime}&pageNumber={pageNumber}&pageSize={pageSize}");
        }
    }
}