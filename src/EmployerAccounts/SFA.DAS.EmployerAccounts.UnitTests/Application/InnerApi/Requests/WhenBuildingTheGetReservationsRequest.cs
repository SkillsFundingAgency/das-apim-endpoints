using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.InnerApi.Requests;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.InnerApi.Requests
{
    public class WhenBuildingTheGetReservationsRequest
    {
        [Test]
        [AutoData]
        public void Then_The_Url_Is_Correctly_Build(long id)
        {
            //Arrange
            var actual = new GetReservationsRequest(id);

            //Assert
            actual.GetAllUrl.Should().Be($"/api/accounts/{id}/reservations");
        }
    }
}