using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Apis.CommitmentsV2InnerApi
{
    public class GetApprenticeshipDetailsRequest : IGetApiRequest
    {
        private readonly long _apprenticeshipId;

        public GetApprenticeshipDetailsRequest(long apprenticeshipId)
        {
            _apprenticeshipId = apprenticeshipId;
        }

        public string GetUrl => $"api/apprenticeships/{_apprenticeshipId}";
    }
}