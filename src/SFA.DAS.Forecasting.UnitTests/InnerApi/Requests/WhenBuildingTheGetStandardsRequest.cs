using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Forecasting.InnerApi.Requests;

namespace SFA.DAS.Forecasting.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetStandardsRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Build()
        {
            var actual = new GetStandardsRequest();

            actual.GetUrl.Should().Be("api/courses/standards");

        }
    }
}