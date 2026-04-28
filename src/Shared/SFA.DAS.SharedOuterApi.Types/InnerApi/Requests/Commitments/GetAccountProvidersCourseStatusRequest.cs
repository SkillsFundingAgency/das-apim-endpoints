using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Commitments;

public class GetAccountProvidersCourseStatusRequest(long accountId, int completionLag, int startLag, int newStartWindow)
    : IGetApiRequest
{
    public long AccountId { get; } = accountId;

    public int CompletionLag { get; } = completionLag;

    public int StartLag { get; } = startLag;

    public int NewStartWindow { get; } = newStartWindow;

    public string GetUrl => $"/api/accounts/{AccountId}/status?completionlag={CompletionLag}&startlag={StartLag}&newstartwindow={NewStartWindow}";
}