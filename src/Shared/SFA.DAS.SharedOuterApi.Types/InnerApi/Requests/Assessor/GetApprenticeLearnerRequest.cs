using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Assessor;

public class GetApprenticeLearnerRequest(long apprenticeCommitmentsId) : IGetApiRequest
{
    public long ApprenticeCommitmentsId { get; } = apprenticeCommitmentsId;
    public string GetUrl => $"api/v1/learnerdetails/{ApprenticeCommitmentsId}";

}