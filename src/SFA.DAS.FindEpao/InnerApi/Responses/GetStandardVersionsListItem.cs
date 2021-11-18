using System;
using System.Collections.Generic;

namespace SFA.DAS.FindEpao.InnerApi.Responses
{
    public class GetStandardVersionsListItem
    {

        public GetStandardVersionsListItem()
        {
            standardVersions = new List<GetStandardsExtendedListItem>();
        }

        public string Id { get; set; }
        public string organisationId { get; set; }
        public int standardCode { get; set; }
        public string standardReference { get; set; }
        public DateTime? dateStandardApprovedOnRegister { get; set; }
        public DateTime? effectiveFrom { get; set; }
        public DateTime? effectiveTo { get; set; }
        public List<GetStandardsExtendedListItem> standardVersions { get; set; }

    }

}