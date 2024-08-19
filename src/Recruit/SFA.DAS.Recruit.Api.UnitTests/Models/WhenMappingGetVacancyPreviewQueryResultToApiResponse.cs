using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Queries.GetVacancyPreview;

namespace SFA.DAS.Recruit.Api.UnitTests.Models;

public class WhenMappingGetVacancyPreviewQueryResultToApiResponse
{
    [Test, AutoData]
    public void Then_The_Values_Are_Mapped(GetVacancyPreviewQueryResult source)
    {
        var actual = (GetVacancyPreviewApiResponse)source;

        actual.Should().BeEquivalentTo(source.Course, options => options.ExcludingMissingMembers());
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