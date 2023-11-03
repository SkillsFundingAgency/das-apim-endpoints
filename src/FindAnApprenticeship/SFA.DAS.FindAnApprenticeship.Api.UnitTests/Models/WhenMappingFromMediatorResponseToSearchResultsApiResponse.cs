using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchResults;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToSearchResultsApiResponse
    {
        [Test, MoqAutoData]
        public void Then_The_Fields_Are_Mapped(SearchResultsQueryResult source)
        {
            var actual = (SearchResultsApiResponse)source;

            actual.TotalApprenticeshipCount.Should().Be(source.TotalApprenticeshipCount);
        }
    }
}
