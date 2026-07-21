using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetProviderByCourseAndUkprnRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Build(int providerId, int courseId, decimal lat, decimal lon)
        {
            var actual = new GetProviderByCourseAndUkprnRequest(providerId, courseId, lat,lon);

            actual.GetUrl.Should().Be($"api/courses/{courseId}/providers/{providerId}?latitude={lat}&longitude={lon}");
        }
        
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built_With_No_Location_Or_ShortListUserId(int providerId, int courseId)
        {
            var actual = new GetProviderByCourseAndUkprnRequest(providerId, courseId);

            actual.GetUrl.Should().Be($"api/courses/{courseId}/providers/{providerId}?latitude=&longitude=");
        }
    }
}
