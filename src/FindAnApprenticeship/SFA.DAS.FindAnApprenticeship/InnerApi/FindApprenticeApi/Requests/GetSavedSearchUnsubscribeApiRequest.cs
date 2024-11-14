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
        public string GetUrl => $"api/savedsearches/{SearchId}";
    }
}
