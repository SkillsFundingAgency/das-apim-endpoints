using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.FindEpao.Api.Models
{
    public class GetCourseEpaoStandardsListItem
    {
        public GetCourseEpaoStandardsListItem()
        {
            standardVersions = new List<StandardsListItem>();
        }

        public string Id { get; set; }
        public string organisationId { get; set; }
        public int standardCode { get; set; }
        public string standardReference { get; set; }
        public DateTime? dateStandardApprovedOnRegister { get; set; }
        public DateTime? effectiveFrom { get; set; }
        public DateTime? effectiveTo { get; set; }
        public List<StandardsListItem> standardVersions { get; set; }


        public class StandardsListItem
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
}