using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetEpaoRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(
            GetEpaoRequest actual)
        {
            actual.GetUrl.Should().Be($"api/ao/assessment-organisations/{actual.EpaoId}");
        }
    }
}