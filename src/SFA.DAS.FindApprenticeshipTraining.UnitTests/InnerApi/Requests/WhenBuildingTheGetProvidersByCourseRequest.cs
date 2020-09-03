using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetProvidersByCourseRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(string baseUrl, int courseId, double latitude, double longitude, int sortOrder)
        {
            var actual = new GetProvidersByCourseRequest(courseId, latitude, longitude, sortOrder)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should().Be($"{baseUrl}api/courses/{courseId}/providers?lat={latitude}&lon={longitude}&sortOrder={sortOrder}");
        }

        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built_With_No_Location(string baseUrl, int courseId)
        {
            var actual = new GetProvidersByCourseRequest(courseId)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should().Be($"{baseUrl}api/courses/{courseId}/providers?lat=&lon=&sortOrder=0");
        }
    }
}