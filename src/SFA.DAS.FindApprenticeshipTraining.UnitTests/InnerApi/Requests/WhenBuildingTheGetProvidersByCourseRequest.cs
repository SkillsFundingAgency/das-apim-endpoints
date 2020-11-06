using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetProvidersByCourseRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(int courseId, double latitude, double longitude, int sortOrder, string sectorSubjectArea)
        {
            var actual = new GetProvidersByCourseRequest(courseId, sectorSubjectArea, latitude, longitude, sortOrder);

            actual.GetUrl.Should().Be($"api/courses/{courseId}/providers?lat={latitude}&lon={longitude}&sortOrder={sortOrder}&sectorSubjectArea={sectorSubjectArea}");
        }

        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built_With_No_Location(int courseId, string sectorSubjectArea)
        {
            var actual = new GetProvidersByCourseRequest(courseId, sectorSubjectArea);

            actual.GetUrl.Should().Be($"api/courses/{courseId}/providers?lat=&lon=&sortOrder=0&sectorSubjectArea={sectorSubjectArea}");
        }
    }
}