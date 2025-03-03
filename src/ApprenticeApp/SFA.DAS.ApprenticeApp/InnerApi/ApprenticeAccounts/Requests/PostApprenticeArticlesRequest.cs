using System;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class PostApprenticeArticlesRequest : IPostApiRequest<ApprenticeArticle>
    {
        private readonly Guid _id;
        private string _articleIdentifier;
        public string PostUrl => $"/apprentices/{_id}/articles/{_articleIdentifier}";
        public ApprenticeArticle Data { get; set; }

        public PostApprenticeArticlesRequest(Guid id, string articleIdentifier)
        {
            _id = id;
            _articleIdentifier = articleIdentifier;
        }
    }
}