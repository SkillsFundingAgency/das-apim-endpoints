using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndex;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;

public class WhenMappingFromMediatrResponseToSearchIndexApiResponse
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(SearchIndexQueryResult source)
    {
        var actual = (SearchIndexApiResponse)source;

        actual.Should().BeEquivalentTo(source);
    }
}