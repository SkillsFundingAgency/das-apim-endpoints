using System.Collections.Generic;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class GetApprenticeshipEmailOverlapResponse
    {
        public IEnumerable<ApprenticeshipEmailOverlap> ApprenticeshipEmailOverlaps { get; set; }
    }

    public class ApprenticeshipEmailOverlap
    {
        public long Id { get; set; }

        public string ErrorMessage { get; set; }
    }
}
