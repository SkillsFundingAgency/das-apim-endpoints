using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi
{
    public class GetCommitmentsPingRequest : IGetApiRequest
    {
        public string GetUrl => "api/ping";
    }
}