using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests
{
    public record GetSavedSearchApiRequest (Guid SearchId) : IGetApiRequest
    {
        public string GetUrl => $"api/savedsearches/{SearchId}";
    }
}
