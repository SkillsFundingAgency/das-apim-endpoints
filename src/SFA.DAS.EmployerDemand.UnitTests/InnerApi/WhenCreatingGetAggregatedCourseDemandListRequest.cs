using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.InnerApi.Requests;

namespace SFA.DAS.EmployerDemand.UnitTests.InnerApi
{
    public class WhenCreatingGetAggregatedCourseDemandListRequest
    {
        [Test, AutoData]
        public void Then_Sets_Url_Correctly(GetAggregatedCourseDemandListRequest request)
        {
            request.GetUrl.Should().Be($"api/demand/aggregated/providers/{request.Ukprn}?courseId={request.CourseId}");
        }
    }
}