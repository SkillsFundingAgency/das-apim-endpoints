using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetCourseEpaosStandardVersionsRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(
            GetCourseEpaosStandardVersionsRequest actual)
        {
            actual.GetUrl.Should().Be($"api/v1/standard-version/standards/epao/{actual.organisationId}/{actual.LarsCode}");
        }
    }
}