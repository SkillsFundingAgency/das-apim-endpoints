using System.Collections.Generic;
using Standard = SFA.DAS.SharedOuterApi.Types.Models.RequestApprenticeTraining.Standard;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLatestStandards
{
    public class GetLatestStandardsResult
    {
        public List<Standard> Standards { get; set; }
    }
}
