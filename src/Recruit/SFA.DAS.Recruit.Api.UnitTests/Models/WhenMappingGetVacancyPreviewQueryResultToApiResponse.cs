using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetVacancyPreview;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.Api.UnitTests.Models;

public class WhenMappingGetVacancyPreviewQueryResultToApiResponse
{
    [Test, AutoData]
    public void Then_The_Values_Are_Mapped(GetVacancyPreviewQueryResult source)
    {
        source.Course.Level = 2;
        var actual = (GetVacancyPreviewApiResponse)source;

        actual.Should().BeEquivalentTo(source.Course, options => options.ExcludingMissingMembers());
        actual.EducationLevelNumber.Should().Be(source.Course.Level);
        actual.ApprenticeshipLevel.Should().Be(ApprenticeshipLevel.Intermediate);
    }

    [Test, AutoData]
    public void Then_If_Null_Then_Null_Is_Returned()
    {
        var source = new GetVacancyPreviewQueryResult
        {
            Course = null
        };
        
        var actual = (GetVacancyPreviewApiResponse)source;
        
        actual.Should().BeNull();
    }
}