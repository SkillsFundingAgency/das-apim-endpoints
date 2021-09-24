using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.InnerApi.Requests;

namespace SFA.DAS.Vacancies.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetEmployerAccountLegalEntities
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(string encodedAccountId)
        {
            var actual = new GetEmployerAccountLegalEntitiesRequest(encodedAccountId);

            actual.GetUrl.Should().Be($"api/accounts/{encodedAccountId}/legalentities");
        }
    }
}