using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRegistrations
{
    public class GetInvitationRequest : IGetApiRequest
    {
        public string CorrelationId { get; set; }
        public GetInvitationRequest(string correlationId)
        {
            CorrelationId = correlationId;
        }

        public string GetUrl => $"api/invitations/{CorrelationId}";
    }
}
