using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.Entities;
using SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Regions.Queries.GetRegions
{
    public class GetRegionsQueryResultTests
    {
        [Test]
        [MoqAutoData]
        public void Result_PopulatesGetRegionResult(List<Region> regions)
        {
            var result = (GetRegionsQueryResult)regions;

            result.Regions.Should().BeEquivalentTo(regions);
        }
    }
}