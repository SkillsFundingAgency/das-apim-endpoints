using System.Collections.Generic;

namespace SFA.DAS.FindEpao.InnerApi.Responses
{
    public class GetStandardsExtendedListResponse : List<GetStandardsExtendedListItem>
    {
        public List<GetStandardsExtendedListItem> StandardVersions { get; set; }
    }
}