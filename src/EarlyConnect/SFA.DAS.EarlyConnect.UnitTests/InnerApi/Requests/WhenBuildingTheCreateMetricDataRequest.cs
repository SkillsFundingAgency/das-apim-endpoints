using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheCreateMetricDataRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(MetricDataList metricsDataList)
        {
            var actual = new CreateMetricDataRequest(metricsDataList);

            actual.PostUrl.Should().Be("api/metrics-data");
        }
    }
}