using SFA.DAS.Assessors.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Assessors.Application.Queries.GetStandardOptions
{
    public class GetStandardOptionsResult
    {
        public IEnumerable<GetStandardOptionsListItem> Standards { get; set; }
    }
}
