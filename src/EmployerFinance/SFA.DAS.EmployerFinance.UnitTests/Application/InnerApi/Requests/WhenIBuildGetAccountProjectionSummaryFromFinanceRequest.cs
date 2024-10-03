using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.InnerApi.Requests;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.InnerApi.Requests
{
    public class WhenIBuildGetAccountProjectionSummaryFromFinanceRequest
    {
        public class WhenBuildingTheGetProviderRequest
        {
            [Test, AutoData]
            public void Then_The_Url_Is_Correctly_Build(int id)
            {
                //Arrange
                var actual = new GetAccountProjectionSummaryFromFinanceRequest(id);

                //Assert
                actual.GetUrl.Should().Be($"/api/accounts/{id}/projection-summary");
            }
        }
    }
}
