using System.Collections.Generic;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Api.Models
{
    public class GetRedirectsResponse
    {
        public List<RedirectModel> Redirects { get; set; }
    }
}
