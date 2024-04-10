using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;

public class WhenMappingFromCommandResponseToCandidateResponse
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(PutCandidateCommandResult source)
    {
        var actual = (CandidateResponse)source;

        actual.Should().BeEquivalentTo(source, options => options.Excluding(c => c.Id));
    }
}