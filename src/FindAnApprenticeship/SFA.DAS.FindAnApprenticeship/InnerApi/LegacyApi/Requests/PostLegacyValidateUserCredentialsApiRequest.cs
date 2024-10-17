using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests
{
    public class PostLegacyValidateUserCredentialsApiRequest(PostLegacyValidateUserCredentialsApiRequestBody body) : IPostApiRequest
    {
        public string PostUrl => "api/user/validate-credentials";
        public object Data { get; set; }  = body;
    }

    public class PostLegacyValidateUserCredentialsApiRequestBody
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
