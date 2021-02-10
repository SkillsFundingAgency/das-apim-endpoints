using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeCommitments.Apis.ApprenticeLoginApi
{
    public class SendInvitationRequest : IPostApiRequest
    {
        private readonly string clientId;

        public SendInvitationRequest(string clientId) =>
            this.clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));

        public string PostUrl => $"/invitations/{clientId}";
        public object Data { get; set; }
    }

    public class SendInvitationRequestData
    {
        public Guid SourceId { get; set; }
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string UserRedirect { get; set; }
        public string Callback { get; set; }
        public string ApprenticeshipName { get; set; }
        public object OrganisationName { get; set; }
    }

    public class SendInvitationResponse
    {
    }
}