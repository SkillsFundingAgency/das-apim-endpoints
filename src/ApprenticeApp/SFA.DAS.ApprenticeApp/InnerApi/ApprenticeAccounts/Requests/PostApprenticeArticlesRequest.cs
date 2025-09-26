using System;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class PostApprenticeArticlesRequest : IPostApiRequest<ApprenticeArticle>
    {
        private readonly Guid _id;
        private string _articleIdentifier;
        private string _articleTitle;
        public string PostUrl => $"/apprentices/{_id}/articles/{_articleIdentifier}/title/{_articleTitle}";
        public ApprenticeArticle Data { get; set; }

        public PostApprenticeArticlesRequest(Guid id, string articleIdentifier, string articleTitle)
        {
            _id = id;
            _articleIdentifier = articleIdentifier;
            _articleTitle = articleTitle;
        }
    }
}