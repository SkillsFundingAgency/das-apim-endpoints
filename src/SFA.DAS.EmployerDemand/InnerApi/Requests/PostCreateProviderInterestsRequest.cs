using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.InnerApi.Requests
{
    public class PostCreateProviderInterestsRequest : IPostApiRequest
    {
        private readonly Guid _id;

        public PostCreateProviderInterestsRequest(CreateProviderInterestsData data)
        {
            Data = data;
            _id = data.Id;
        }

        public string PostUrl => $"api/providerinterest/{_id}";
        public object Data { get; set; }
    }

    public class CreateProviderInterestsData
    {
        public Guid Id { get; set; }
        public IEnumerable<Guid> EmployerDemandIds { get; set; }
        public int Ukprn { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
    }
}