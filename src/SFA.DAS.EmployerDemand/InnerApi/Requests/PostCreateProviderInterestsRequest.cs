using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class PostCreateProviderInterestsRequest : IPostApiRequest
    {
        public PostCreateProviderInterestsRequest(CreateProviderInterestsData data)
        {
            Data = data;
        }

        public string PostUrl => "api/providerinterest/create";
        public object Data { get; set; }
    }

    public class CreateProviderInterestsData
    {
        public IEnumerable<Guid> EmployerDemandIds { get; set; }
        public int Ukprn { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
    }
}