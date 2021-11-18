using System;
using System.Collections.Generic;

namespace SFA.DAS.FindEpao.InnerApi.Responses
{
    public class GetStandardsExtendedListItem
    {
        public string standardUId { get; set; }
        public string title { get; set; }
        public int larsCode { get; set; }
        public string version { get; set; }
        public DateTime? effectiveFrom { get; set; }
        public DateTime? effectiveTo { get; set; }
        public DateTime? dateVersionApproved { get; set; }
        public string status { get; set; }

    }

}