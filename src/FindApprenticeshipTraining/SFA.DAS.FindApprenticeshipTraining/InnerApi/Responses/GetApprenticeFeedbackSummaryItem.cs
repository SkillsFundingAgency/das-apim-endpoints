using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetApprenticeFeedbackSummaryItem
    {
        public long Ukprn { get; set; }
        public int Stars { get; set; }
        public int ReviewCount { get; set; }
    }
}
