using SFA.DAS.ApprenticeApp.Models;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress
{
    public class GetKsbsByApprenticeshipIdQueryResult
    {
        public List<ApprenticeKsbProgressData> KSBProgresses { get; set; }
    }
}
