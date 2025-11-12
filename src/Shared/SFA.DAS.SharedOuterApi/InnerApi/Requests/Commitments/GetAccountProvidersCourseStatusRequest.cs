using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests
{
    public class GetAccountProvidersCourseStatusRequest : IGetApiRequest
    {
        public long AccountId { get; }

        public int CompletionLag { get;  }

        public int StartLag { get;  }

        public int NewStartWindow { get; }

        public GetAccountProvidersCourseStatusRequest(long accountId, int completionLag, int startLag, int newStartWindow)
        {
            AccountId = accountId;
            CompletionLag = completionLag;
            StartLag = startLag;
            NewStartWindow = newStartWindow;
        }

        public string GetUrl => $"/api/accounts/{AccountId}/status?completionlag={CompletionLag}&startlag={StartLag}&newstartwindow={NewStartWindow}";
    }
}
