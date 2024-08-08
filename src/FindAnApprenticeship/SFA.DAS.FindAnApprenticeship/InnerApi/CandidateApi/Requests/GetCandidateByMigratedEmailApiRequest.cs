using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

public class GetCandidateByMigratedEmailApiRequest(string email) : IGetApiRequest
{
    public string GetUrl => $"api/candidates/migrated/email/{email}";
}