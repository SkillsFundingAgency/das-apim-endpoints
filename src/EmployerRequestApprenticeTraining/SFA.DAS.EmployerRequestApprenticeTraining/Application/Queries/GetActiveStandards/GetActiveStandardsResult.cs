using System.Collections.Generic;
using Standard = SFA.DAS.SharedOuterApi.Models.RequestApprenticeTraining.Standard;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetActiveStandards
{
    public class GetActiveStandardsResult
    {
        public List<Standard> Standards { get; set; }
    }
}
