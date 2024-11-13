using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests
{
    public record GetSavedSearchUnsubscribeApiRequest (Guid SearchId) : IGetApiRequest
    {
        private readonly Guid _search_id = SearchId;
        
        public string GetUrl => $"saved-searches/{_search_id}/unsubscribe";
    }
}
