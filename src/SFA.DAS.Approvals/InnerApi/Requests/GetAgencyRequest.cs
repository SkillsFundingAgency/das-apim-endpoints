using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetAgencyRequest : IGetApiRequest
    {
        public int LegalEntityId { get; }

        public GetAgencyRequest(int legalEntityId)
        {
            LegalEntityId = legalEntityId;
        }
        public string GetUrl => $"agencies/{LegalEntityId}";
    }
}
