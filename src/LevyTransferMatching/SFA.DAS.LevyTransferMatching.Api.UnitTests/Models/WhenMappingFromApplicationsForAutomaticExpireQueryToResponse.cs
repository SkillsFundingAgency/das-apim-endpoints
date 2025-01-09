using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Queries.Functions;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Models;

public class WhenMappingFromApplicationsForAutomaticExpireQueryToResponse
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Correctly_Mapped(ApplicationsForAutomaticExpireResult source)
    {
        var actual = (ApplicationsForAutomaticExpireResponse)source;

        actual.ApplicationIdsToExpire.Should().BeEquivalentTo(source.ApplicationIdsToExpire);
    }
}