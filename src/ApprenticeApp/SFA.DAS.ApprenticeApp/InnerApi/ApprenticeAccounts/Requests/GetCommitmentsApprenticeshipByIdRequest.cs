using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class GetCommitmentsApprenticeshipByIdRequest : IGetApiRequest
    {
        private long _apprenticeshipId;

        public GetCommitmentsApprenticeshipByIdRequest(long apprenticeshipId)
        {
            _apprenticeshipId = apprenticeshipId; 
        }

        public string GetUrl => $"/api/apprenticeships/{_apprenticeshipId}";
    }
}
