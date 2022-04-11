using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Recruit.InnerApi.Responses
{
    public class GetAccountLegalEntityResponseItem
    {
        public List<Agreement> Agreements { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        public string AccountLegalEntityPublicHashedId { get; set; }
    }
    
}