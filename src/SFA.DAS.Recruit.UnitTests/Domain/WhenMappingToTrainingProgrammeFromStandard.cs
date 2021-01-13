using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.UnitTests.Domain
{
    public class WhenMappingToTrainingProgrammeFromStandard
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetStandardsListItem source)
        {
            //Arrange
            var actual = (TrainingProgramme) source;
            
            //Assert
            actual.Id.Should().Be(source.Id.ToString());
        }
    }
}