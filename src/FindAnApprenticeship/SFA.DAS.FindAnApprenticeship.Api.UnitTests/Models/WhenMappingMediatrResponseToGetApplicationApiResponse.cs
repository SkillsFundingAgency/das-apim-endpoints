using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication;
using static SFA.DAS.FindAnApprenticeship.Api.Models.Applications.GetApplicationApiResponse;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;

public class WhenMappingMediatrResponseToGetApplicationApiResponse
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(GetApplicationQueryResult source)
    {
        var actual = (GetApplicationApiResponse)source;

        actual.Candidate.Should().BeEquivalentTo(source.CandidateDetails);
    }


    [Test, AutoData]
    public void Then_Candidate_The_Fields_Are_Mapped_To_CandidateDetailsSection(GetApplicationQueryResult.Candidate source)
    {
        var actual = (CandidateDetailsSection)source;

        actual.Should().BeEquivalentTo(source);
    }
}