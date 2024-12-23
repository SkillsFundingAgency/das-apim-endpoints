using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi
{
    [TestFixture]
    public class WhenBuildingGetCandidatesByActivityApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(string cutOffDateTime, int pageNumber, int pageSize)
        {
            var actual = new GetCandidatesByActivityApiRequest(cutOffDateTime, pageNumber, pageSize);

            actual.GetUrl.Should().Be($"api/candidates/GetCandidatesByActivity?cutOffDateTime={cutOffDateTime}&pageNumber={pageNumber}&pageSize={pageSize}");
        }
    }
}