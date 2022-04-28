using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetApprenticeLearnerRequest : IGetApiRequest
    {
        public GetApprenticeLearnerRequest(long apprenticeCommitmentsId)
        {
            ApprenticeCommitmentsId = apprenticeCommitmentsId;
        }
        public long ApprenticeCommitmentsId { get; }
        public string GetUrl => $"api/v1/learnerdetails/{ApprenticeCommitmentsId}";

    }
}