using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindEpao.InnerApi.Requests;

namespace SFA.DAS.FindEpao.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetCourseEpaosRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(
            GetCourseEpaosRequest actual)
        {
            actual.GetAllUrl.Should().Be($"api/v1/standards/{actual.CourseId}/organisations");
        }
    }
}