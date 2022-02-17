using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Responses
{
    public class GetApprenticeshipResponse
    {
        public long Ukprn { get; set; }
        public string StandardReference { get; set; }
        public int LarsCode { get; set; }
        public string StandardUId { get; set; }
    }
}
