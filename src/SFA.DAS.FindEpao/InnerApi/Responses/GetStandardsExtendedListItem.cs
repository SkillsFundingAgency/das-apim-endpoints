using System;
using System.Collections.Generic;

namespace SFA.DAS.FindEpao.InnerApi.Responses
{
    public class GetStandardsExtendedListItem
    {
        public string StandardUId { get; set; }
        public string Title { get; set; }
        public int LarsCode { get; set; }
        public string Version { get; set; }
        public int Level { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public DateTime? DateVersionApproved { get; set; }
        public string Status { get; set; }

    }

}