using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetProvidersByCourseRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(string baseUrl, int courseId)
        {
            var actual = new GetProvidersByCourseRequest(courseId)
            {
                BaseUrl = baseUrl
            };

            actual.GetUrl.Should().Be($"{baseUrl}api/courses/{courseId}/providers?age=4&level=1");
        }
    }
}