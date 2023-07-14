using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetProvider;

namespace SFA.DAS.Recruit.Api.UnitTests.Models
{
    public class WhenMappingGetTrainingProviderResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetProviderQueryResult source)
        {
            //Arrange
            var actual = (GetTrainingProviderResponse) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(x => x.ProviderType.Id));

            actual.ProviderType.Id.Should().Be(source.ProviderType.Id);
        }
    }
}