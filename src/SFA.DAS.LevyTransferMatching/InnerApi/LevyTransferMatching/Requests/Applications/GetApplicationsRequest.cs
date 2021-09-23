using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests.Applications
{
    public class GetApplicationsRequest : IGetApiRequest
    {
        public long AccountId { get; set; }
        public string GetUrl => $"accounts/{AccountId}/applications";
    }
}
