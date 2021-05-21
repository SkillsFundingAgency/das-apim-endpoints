using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.InnerApi.Requests;

namespace SFA.DAS.EmployerDemand.UnitTests.InnerApi
{
    public class WhenCreatingGetCourseProviderDemandsRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Built_Correctly(int ukprn, int courseId)
        {
            //Act
            var actual = new GetCourseProviderDemandsRequest(ukprn, courseId);
            
            //Assert
            actual.GetUrl.Should().Be($"api/Demand/providers/{ukprn}/courses/{courseId}?lat=&lon=&radius=");
        }

        [Test, AutoData]
        public void Then_The_Request_Is_Build_With_Location_Information(int ukprn, int courseId, double? lat, double? lon, int? radius)
        {
            //Act
            var actual = new GetCourseProviderDemandsRequest(ukprn, courseId, lat, lon, radius);
            
            //Assert
            actual.GetUrl.Should().Be($"api/Demand/providers/{ukprn}/courses/{courseId}?lat={lat}&lon={lon}&radius={radius}");
        }
    }
}