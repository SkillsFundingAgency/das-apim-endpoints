using System;
using System.Collections.Generic;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.DigitalCertificates.Models;
using static SFA.DAS.DigitalCertificates.InnerApi.Requests.PostUpdateUserIdentityRequest;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{

    public class PostUpdateUserIdentityRequest : IPostApiRequest<PostUpdateUserIdentityRequestData>
    {
        public PostUpdateUserIdentityRequestData Data { get; set; }

        public string PostUrl { get; set; }

        public PostUpdateUserIdentityRequest(PostUpdateUserIdentityRequestData data, Guid userId)
        {
            Data = data;
            PostUrl = $"api/users/{userId}/identity";
        }

        public class PostUpdateUserIdentityRequestData
        {
            public required List<Name> Names { get; set; }
            public DateTime? DateOfBirth { get; set; }
        }
    }
}
