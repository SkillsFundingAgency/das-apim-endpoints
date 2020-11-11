using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindEpao.InnerApi.Requests;

namespace SFA.DAS.FindEpao.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetStandardsRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(
            GetStandardsRequest actual)
        {
            actual.GetUrl.Should().Be("api/courses/standards");
        }
    }
}