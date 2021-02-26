using System.Collections.Generic;

namespace SFA.DAS.Assessors.Api.Models
{
    public class GetStandardOptionsResponse
    {
        public IEnumerable<GetStandardOptionsItem> Standards { get; set; }
    }
}
