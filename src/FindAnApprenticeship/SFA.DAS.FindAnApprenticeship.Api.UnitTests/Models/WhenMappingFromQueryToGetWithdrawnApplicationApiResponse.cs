using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.WithdrawApplication;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;

public class WhenMappingFromQueryToGetWithdrawnApplicationApiResponse
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(WithdrawApplicationQueryResult source)
    {
        var actual = (GetWithdrawnApplicationApiResponse)source;

        actual.Should().BeEquivalentTo(source);
    }
}