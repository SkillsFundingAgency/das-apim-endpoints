using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Models;

public class WhenMappingFromApplicationsForAutomaticDeclineQueryToResponse
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Correctly_Mapped(ApplicationsForAutomaticDeclineResult source)
    {
        var actual = (ApplicationsForAutomaticDeclineResponse)source;

        actual.ApplicationIdsToDecline.Should().BeEquivalentTo(source.ApplicationIdsToDecline);
    }
}
