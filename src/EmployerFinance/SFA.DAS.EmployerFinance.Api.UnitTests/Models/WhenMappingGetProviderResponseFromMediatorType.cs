using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Api.Models;
using SFA.DAS.EmployerFinance.InnerApi.Responses;

namespace SFA.DAS.EmployerFinance.Api.UnitTests.Models
{
    public class WhenMappingGetProviderResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetProvidersListItem source)
        {
            //Arrange
            var actual = (ProviderResponse) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source);
        }
    }
}