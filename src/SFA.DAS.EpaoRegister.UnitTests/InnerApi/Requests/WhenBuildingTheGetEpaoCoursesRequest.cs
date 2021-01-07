using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.InnerApi.Requests;

namespace SFA.DAS.EpaoRegister.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetEpaoCoursesRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(
            GetEpaoCoursesRequest actual)
        {
            actual.GetAllUrl.Should().Be($"api/ao/assessment-organisations/{actual.EpaoId}/standards");
        }
    }
}