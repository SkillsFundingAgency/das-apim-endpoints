using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.InnerApi.Requests;

namespace SFA.DAS.EmployerDemand.UnitTests.InnerApi
{
    public class WhenCreatingGetProviderCourseInformationRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(int ukprn, int courseId)
        {
            //Act
            var actual = new GetProviderCourseInformationRequest(ukprn, courseId);
            
            //Assert
            actual.GetUrl.Should().Be($"api/courses/{courseId}/providers/{ukprn}");
        }
    }
}