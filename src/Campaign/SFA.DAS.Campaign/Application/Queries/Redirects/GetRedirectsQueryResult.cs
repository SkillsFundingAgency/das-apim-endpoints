using System.Collections.Generic;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Application.Queries.Redirects
{
    public class GetRedirectsQueryResult
    {
        public List<RedirectModel> Redirects { get; set; }
    }
}
