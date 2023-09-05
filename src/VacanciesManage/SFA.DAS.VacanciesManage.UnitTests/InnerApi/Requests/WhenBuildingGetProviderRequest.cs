using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.VacanciesManage.InnerApi.Requests;

namespace SFA.DAS.VacanciesManage.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetProviderRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(long ukprn)
        {
            var actual = new GetProviderRequest(ukprn);

            actual.GetUrl.Should().Be($"api/providers/{ukprn}");
        }
    }
}
