using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetUkprnsForStandardAndLocationResponse
    {
        public IEnumerable<int> UkprnsByStandard { get; set; }
        public IEnumerable<int> UkprnsByStandardAndLocation { get; set; }
    }
}