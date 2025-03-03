using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class GetApprenticeArticlesRequest : IGetApiRequest
    {
        private readonly Guid _apprenticeId;

        public GetApprenticeArticlesRequest(Guid apprenticeId)
        {
            _apprenticeId = apprenticeId;
        }

        public string GetUrl => $"apprentices/{_apprenticeId}/articles";
    }
}
