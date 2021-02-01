using System;
using Newtonsoft.Json;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Apis.ApprenticeLoginApi
{
    public class SendInvitationRequest : IPostApiRequest
    {
        public string PostUrl => $"/invitations/{ClientId}";
        public string ClientId { get; set; }
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