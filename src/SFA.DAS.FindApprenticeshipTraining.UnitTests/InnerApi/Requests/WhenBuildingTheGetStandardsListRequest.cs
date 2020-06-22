using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetStandardsListRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(
            string keyword,
            string baseUrl)
        {
            var actual = new GetStandardsListRequest{BaseUrl = baseUrl, Keyword = keyword};

            actual.BaseUrl.Should().Be(baseUrl);
            actual.GetUrl.Should().Be($"{baseUrl}api/courses/standards?keyword={keyword}");
        }
    }
}
