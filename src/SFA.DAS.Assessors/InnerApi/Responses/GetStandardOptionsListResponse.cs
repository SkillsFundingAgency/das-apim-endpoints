using System.Collections.Generic;

namespace SFA.DAS.Assessors.InnerApi.Responses
{
    public class GetStandardOptionsListResponse
    {
        public IEnumerable<GetStandardOptionsListItem> StandardOptions { get; set; }
    }
}
