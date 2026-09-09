using SFA.DAS.SharedOuterApi.Types.Models.RequestApprenticeTraining;
using System.Collections.Generic;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetRegions
{
    public class GetRegionsResult
    {
        public List<Region> Regions { get; set; }
    }
}