using System.Collections.Generic;

namespace SFA.DAS.Assessors.InnerApi.Responses
{
    public class GetStandardOptionsListItem
    {
        public string StandardUid { get; set; }
        public int LarsCode { get; set; }
        public string IfateReferenceNumber { get; set; }
        public List<string> Options { get; set; }
    }
}
