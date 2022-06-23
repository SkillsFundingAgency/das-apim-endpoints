using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.InnerApi.Requests;

namespace SFA.DAS.EpaoRegister.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetEpaosRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(
            GetEpaosRequest actual)
        {
            actual.GetAllUrl.Should().Be("api/ao/assessment-organisations");
        }
    }
}