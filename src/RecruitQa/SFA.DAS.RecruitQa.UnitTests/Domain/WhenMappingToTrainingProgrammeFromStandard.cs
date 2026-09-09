using SFA.DAS.RecruitQa.Domain;
using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.UnitTests.Domain;

public class WhenMappingToTrainingProgrammeFromStandard
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Correctly_Mapped(GetStandardsListItem source)
    {
        //Arrange
        source.Level = (int) ApprenticeshipLevel.Advanced;
        source.ApprenticeshipType = TrainingType.Standard.ToString();

        //Act
        var actual = (TrainingProgramme) source;
            
        //Assert
        actual.Id.Should().Be(source.LarsCode.ToString());
        actual.ApprenticeshipType.Should().Be(TrainingType.Standard);
        actual.Title.Should().Be(source.Title);
        actual.EffectiveFrom.Should().Be(source.StandardDates.EffectiveFrom);
        actual.EffectiveTo.Should().Be(source.StandardDates.EffectiveTo);
        actual.LastDateStarts.Should().Be(source.StandardDates.LastDateStarts);
        actual.ApprenticeshipLevel.Should().Be(ApprenticeshipLevelMapper.RemapFromInt(source.Level));
        actual.Duration.Should().Be(source.TypicalDuration);
        actual.IsActive.Should().Be(source.IsActive);
        actual.EducationLevelNumber.Should().Be(source.Level);
        actual.FrameworkCode.Should().Be(0);
        actual.SectorCode.Should().Be(source.SectorCode);
        actual.Ssa1.Should().Be(0);
    }
}