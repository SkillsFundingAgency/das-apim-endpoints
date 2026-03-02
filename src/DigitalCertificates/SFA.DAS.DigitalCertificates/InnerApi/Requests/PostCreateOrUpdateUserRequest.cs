using System;
using System.Collections.Generic;
using SFA.DAS.DigitalCertificates.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using static SFA.DAS.DigitalCertificates.InnerApi.Requests.PostCreateOrUpdateUserRequest;

namespace SFA.DAS.DigitalCertificates.InnerApi.Requests
{

    public class PostCreateOrUpdateUserRequest : IPostApiRequest<PostCreateOrUpdateUserRequestData>
    {
        public PostCreateOrUpdateUserRequestData Data { get; set; }

        public PostCreateOrUpdateUserRequest(PostCreateOrUpdateUserRequestData data)
        {
            Data = data;
        }

        public string PostUrl => $"api/users/identity";

        public class PostCreateOrUpdateUserRequestData
        {
            public required string GovUkIdentifier { get; set; }
            public required string EmailAddress { get; set; }
            public string PhoneNumber { get; set; }

            public required List<Name> Names { get; set; }
            public DateTime? DateOfBirth { get; set; }
        }
    }
}
