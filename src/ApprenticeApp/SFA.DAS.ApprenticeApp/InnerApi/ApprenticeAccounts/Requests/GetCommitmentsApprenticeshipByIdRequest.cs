using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class GetCommitmentsApprenticeshipByIdRequest : IGetApiRequest
    {
        private readonly long _apprenticeshipId;

        public GetCommitmentsApprenticeshipByIdRequest(long apprenticeshipId)
        {
            _apprenticeshipId = apprenticeshipId; 
        }

        public string GetUrl => $"/api/apprenticeships/{_apprenticeshipId}";
    }
}
