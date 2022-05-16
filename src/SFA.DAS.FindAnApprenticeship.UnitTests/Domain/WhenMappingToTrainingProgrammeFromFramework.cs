using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Domain
{
    public class WhenMappingToTrainingProgrammeFromFramework
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetFrameworksListItem source)
        {
            //Arrange
            source.Level = (int) ApprenticeshipLevel.Advanced;

            //Act
            var actual = (TrainingProgramme) source;
            
            //Assert
            actual.Id.Should().Be(source.Id);
            actual.ApprenticeshipType.Should().Be(TrainingType.Framework);
            actual.Title.Should().Be(source.Title);
            actual.EffectiveFrom.Should().Be(source.EffectiveFrom);
            actual.EffectiveTo.Should().Be(source.EffectiveTo);
            actual.ApprenticeshipLevel.Should().Be(ApprenticeshipLevelMapper.RemapFromInt(source.Level));
            actual.Duration.Should().Be(source.Duration);
            actual.IsActive.Should().BeFalse();
            actual.EducationLevelNumber.Should().Be(source.Level);
            actual.FrameworkCode.Should().Be(source.FrameworkCode);
            actual.SectorCode.Should().Be(0);
            actual.Ssa1.Should().Be(source.Ssa1);
            actual.Route.Should().BeNullOrEmpty();
        }
    }
}