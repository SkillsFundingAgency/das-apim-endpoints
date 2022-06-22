using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetAgencyRequest : IGetApiRequest
    {
        public long LegalEntityId { get; }

        public GetAgencyRequest(long legalEntityId)
        {
            LegalEntityId = legalEntityId;
        }
        public string GetUrl => $"agencies/{LegalEntityId}";
    }
}
