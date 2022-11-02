using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetProviderByCourseAndUkPrnRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Build(int providerId, int courseId, double lat, double lon)
        {
            var actual = new GetProviderByCourseAndUkPrnRequest(providerId, courseId, lat,lon);

            actual.GetUrl.Should().Be($"api/courses/{courseId}/providers/{providerId}?lat={lat}&lon={lon}");
        }
        
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built_With_No_Location_Or_ShortListUserId(int providerId, int courseId)
        {
            var actual = new GetProviderByCourseAndUkPrnRequest(providerId, courseId);

            actual.GetUrl.Should().Be($"api/courses/{courseId}/providers/{providerId}?lat=&lon=");
        }
    }
}