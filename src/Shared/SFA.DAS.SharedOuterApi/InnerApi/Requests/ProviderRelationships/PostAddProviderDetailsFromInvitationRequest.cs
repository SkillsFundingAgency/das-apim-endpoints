using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships
{
    public class PostAddProviderDetailsFromInvitationRequest : IPostApiRequest
    {
        public long AccountId { get; set; }
        public long Ukprn { get; set; }
        public object Data { get; set; } = null;

        public PostAddProviderDetailsFromInvitationRequest(long accountId, long ukprn, string correlationId,
            string userRef, string email, string firstName, string lastName)
        {
            AccountId = accountId;
            Data = new AddProviderDetailsFromInvitationRequest
            {
                Ukprn = ukprn,
                CorrelationId = correlationId,
                UserRef = userRef,
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };
        }

        public string PostUrl => $"accounts/{AccountId}/providers/invitation";

        private class AddProviderDetailsFromInvitationRequest
        {
            public string CorrelationId { get; set; }
            public string UserRef { get; set; }
            public long Ukprn { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
