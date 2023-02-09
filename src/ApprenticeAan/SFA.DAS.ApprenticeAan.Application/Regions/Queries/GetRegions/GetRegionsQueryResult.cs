using SFA.DAS.ApprenticeAan.Application.Entities;

namespace SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions
{
    public class GetRegionsQueryResult
    {
        public List<Region> Regions { get; set; } = new();

        public static implicit operator GetRegionsQueryResult(List<Region> regions) => new()
        {
            Regions = regions
        };
    }
}