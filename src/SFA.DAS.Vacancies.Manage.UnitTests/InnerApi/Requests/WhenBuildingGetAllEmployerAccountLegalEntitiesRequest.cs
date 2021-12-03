using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;

namespace SFA.DAS.Vacancies.Manage.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetAllEmployerAccountLegalEntitiesRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(string encodedAccountId)
        {
            var actual = new GetAllEmployerAccountLegalEntitiesRequest(encodedAccountId);

            actual.GetUrl.Should().Be($"api/accounts/{encodedAccountId}");
        }
    }
}