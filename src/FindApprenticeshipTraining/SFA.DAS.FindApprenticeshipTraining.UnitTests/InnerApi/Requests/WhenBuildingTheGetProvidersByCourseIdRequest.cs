using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using FluentAssertions;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetProvidersByCourseIdRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(int courseId, double latitude, double longitude)
        {
            var actual = new GetProvidersByCourseIdRequest(courseId, latitude, longitude);
        
            actual.GetUrl.Should().Be($"api/courses/{courseId}/providers?lat={latitude}&lon={longitude}");
        }
        
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built_With_No_Location(int courseId)
        {
            var actual = new GetProvidersByCourseIdRequest(courseId,null,null);
            actual.GetUrl.Should().Be($"api/courses/{courseId}/providers?lat=&lon=");
        }
    }
}
