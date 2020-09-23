using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetUkprnsForStandardAndLocationResponse
    {
        public IEnumerable<int> Ukprns { get; set; }
    }
}