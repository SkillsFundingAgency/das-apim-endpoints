using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerFeedback;

public class GetLatestEmployerFeedbackRequest(long accountId, Guid userRef) : IGetApiRequest
{
    public long AccountId { get; set; } = accountId;
    public Guid UserRef { get; set; } = userRef;

    public string GetUrl => $"api/employerfeedback?accountid={AccountId}&userref={UserRef}";
}