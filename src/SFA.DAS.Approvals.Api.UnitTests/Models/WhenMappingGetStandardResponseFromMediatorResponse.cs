using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.UnitTests.Models
{
    public class WhenMappingGetStandardResponseFromMediatorResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetStandardsListItem source)
        {
            //Act
            var actual = (GetStandardResponse) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(c=>c.StandardDates)
                .Excluding(c=>c.TypicalDuration)
                .Excluding(c=>c.IsActive)
            );
            actual.EffectiveFrom.Should().Be(source.StandardDates.EffectiveFrom);
            actual.EffectiveTo.Should().Be(source.StandardDates.EffectiveTo);
            actual.LastDateForNewStarts.Should().Be(source.StandardDates.LastDateStarts);
            actual.Duration.Should().Be(source.TypicalDuration);

        }
    }
}