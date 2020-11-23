using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.InnerApi.Requests;

namespace SFA.DAS.EpaoRegister.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetEpaoRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(
            GetEpaoRequest actual)
        {
            actual.GetAllUrl.Should().Be($"api/v1/organisationsearch/organisations?searchTerm={actual.EpaoId}&pageSize=10&pageIndex=1");
        }
    }
}