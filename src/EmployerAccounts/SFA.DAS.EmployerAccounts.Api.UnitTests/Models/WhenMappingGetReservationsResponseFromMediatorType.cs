using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Api.Models.Reservations;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Models
{
    public class WhenMappingGetReservationsResponseFromMediatorType
    {
        [Test]
        [AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetReservationsResponseListItem source)
        {
            //Arrange
            var actual = (ReservationsResponse)source;

            //Assert
            actual.Should().BeEquivalentTo(source);
        }
    }
}