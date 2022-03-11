using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Responses
{
    public class GetStandardsListItem
    {
        public string StandardUId { get; set; }
        public string StandardReference { get; set; }
        public string StandardName { get; set; }
        public int LarsCode { get; set; }
    }
}
