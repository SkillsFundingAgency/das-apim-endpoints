using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.Commitments
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