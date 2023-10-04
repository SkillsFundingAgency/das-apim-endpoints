using SFA.DAS.ApprenticeAan.Application.Model;

namespace SFA.DAS.ApprenticeAan.Application.Regions.Queries.GetRegions
{
    public class GetRegionsQueryResult
    {
        public List<Region> Regions { get; set; } = new();
    }
}