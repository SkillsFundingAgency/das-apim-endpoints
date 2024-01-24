using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetMetricsDataByLepsCodeRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string lepsCode)
        {
            var actual = new GetMetricsDataByLepsCodeRequest(lepsCode);

            actual.GetUrl.Should().Be($"api/metrics-data/{lepsCode}");
        }
    }
}