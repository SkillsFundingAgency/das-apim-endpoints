using SFA.DAS.Apim.Shared.Interfaces;
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

        public string PostUrl => $"api/users";

        public class PostCreateOrUpdateUserRequestData
        {
            public required string GovUkIdentifier { get; set; }
            public required string EmailAddress { get; set; }
            public string PhoneNumber { get; set; }
        }
    }
}
