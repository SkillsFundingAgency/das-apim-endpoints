using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests
{
    public class GetChangeOfProviderChainRequest : IGetApiRequest
    {
        public readonly long ApprenticeshipId;
        public byte Status { get; set; }

        public GetChangeOfProviderChainRequest(long apprenticeshipId)
        {
            ApprenticeshipId = apprenticeshipId;
        }

        public string GetUrl => $"api/apprenticeships/{ApprenticeshipId}/change-of-provider-chain";
    }
}
