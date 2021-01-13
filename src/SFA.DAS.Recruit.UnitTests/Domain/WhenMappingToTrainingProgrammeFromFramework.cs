using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.UnitTests.Domain
{
    public class WhenMappingToTrainingProgrammeFromFramework
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetFrameworksListItem source)
        {
            //Arrange
            var actual = (TrainingProgramme) source;
            
            //Assert
            actual.Id.Should().Be(source.Id);
            actual.ApprenticeshipType.Should().Be(TrainingType.Framework);
            actual.Title.Should().Be(source.Title);
            actual.EffectiveFrom.Should().Be(source.EffectiveFrom);
            actual.EffectiveTo.Should().Be(source.EffectiveTo);
            //todo actual.ApprenticeshipLevel.Should().Be(source.Level);
            actual.Duration.Should().Be(source.Duration);
            actual.IsActive.Should().BeFalse();
            //todo actual.EducationLevelNumber.Should().Be(source.Level);
        }
    }
}