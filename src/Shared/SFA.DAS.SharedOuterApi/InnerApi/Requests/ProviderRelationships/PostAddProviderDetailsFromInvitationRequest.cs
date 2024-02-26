using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships
{
    public class PostAddProviderDetailsFromInvitationRequest : IPostApiRequest
    {
        public long AccountId { get; set; }
        public object Data { get; set; } = null;

        public PostAddProviderDetailsFromInvitationRequest(long accountId, string correlationId, string userRef)
        {
            AccountId = accountId;
            Data = new AddProviderDetailsFromInvitationRequest
            {
                CorrelationId = correlationId, 
                UserRef = userRef
            };
        }

        public string PostUrl => $"applications/{AccountId}/providers/invitation";

        private class AddProviderDetailsFromInvitationRequest
        {
            public string CorrelationId { get; set; }
            public string UserRef { get; set; }
        }
    }   
}
