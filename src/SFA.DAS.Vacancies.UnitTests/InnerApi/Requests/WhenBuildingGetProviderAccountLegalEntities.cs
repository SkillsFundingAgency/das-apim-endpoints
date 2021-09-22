using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.InnerApi.Requests;

namespace SFA.DAS.Vacancies.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetProviderAccountLegalEntities
    {
        [Test, AutoData]
        public async Task Then_The_Request_Is_Correctly_Build(int ukprn)
        {
            var actual = new GetProviderAccountLegalEntities(ukprn);

            actual.GetUrl.Should().Be($"/accountproviderlegalentities?ukprn={ukprn}");
        }
    }
}