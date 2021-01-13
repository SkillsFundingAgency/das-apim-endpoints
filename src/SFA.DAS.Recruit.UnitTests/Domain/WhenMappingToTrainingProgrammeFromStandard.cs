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
            source.Level = (int) ApprenticeshipLevel.Advanced;

            //Act
            var actual = (TrainingProgramme) source;
            
            //Assert
            actual.Id.Should().Be(source.Id.ToString());
            actual.ApprenticeshipType.Should().Be(TrainingType.Standard);
            actual.Title.Should().Be(source.Title);
            actual.EffectiveFrom.Should().Be(source.StandardDates.EffectiveFrom);
            actual.EffectiveTo.Should().Be(source.StandardDates.EffectiveTo);
            actual.ApprenticeshipLevel.Should().Be(ApprenticeshipLevelHelper.RemapFromInt(source.Level));
            actual.Duration.Should().Be(source.TypicalDuration);
            actual.IsActive.Should().Be(IsStandardActiveHelper.IsStandardActive(source));
            actual.EducationLevelNumber.Should().Be(source.Level);
        }
    }
}