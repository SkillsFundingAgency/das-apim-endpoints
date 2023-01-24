using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Api.Models.Providers;
using SFA.DAS.EmployerFinance.Application.Queries.GetProvider;

namespace SFA.DAS.EmployerFinance.Api.UnitTests.Models
{
    public class WhenMappingGetProviderResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetProviderQueryResult source)
        {
            //Arrange
            var actual = (GetProviderResponse) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source);
        }
    }
}