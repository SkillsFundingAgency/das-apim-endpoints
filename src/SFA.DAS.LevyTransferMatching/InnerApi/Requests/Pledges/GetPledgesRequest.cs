using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges
{
    public class GetPledgesRequest : IGetAllApiRequest
    {
        public string GetAllUrl => $"pledges";
    }
}
