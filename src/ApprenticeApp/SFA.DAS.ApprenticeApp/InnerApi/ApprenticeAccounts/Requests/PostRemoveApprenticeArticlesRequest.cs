using System;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests
{
    public class PostRemoveApprenticeArticlesRequest : IPostApiRequest<ApprenticeArticle>
    {
        private readonly Guid _id;
        private string _articleIdentifier;
        public string PostUrl => $"/apprentices/{_id}/removearticle/{_articleIdentifier}";
        public ApprenticeArticle Data { get; set; }

        public PostRemoveApprenticeArticlesRequest(Guid id, string articleIdentifier)
        {
            _id = id;
            _articleIdentifier = articleIdentifier;
        }
    }
}