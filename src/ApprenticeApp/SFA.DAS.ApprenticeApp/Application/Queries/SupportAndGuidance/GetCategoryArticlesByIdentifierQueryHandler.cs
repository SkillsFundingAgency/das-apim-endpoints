using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.ApprenticeApp.Models.Contentful;
using SFA.DAS.ApprenticeApp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetCategoryArticlesByIdentifierQueryHandler : IRequestHandler<GetCategoryArticlesByIdentifierQuery, GetCategoryArticlesByIdentifierQueryResult>
    {
        private readonly ContentService _contentService;
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _accountsApiClient;

        public GetCategoryArticlesByIdentifierQueryHandler(
            ContentService contentService,
            IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient
            )
        {
            _contentService = contentService;
            _accountsApiClient = accountsApiClient;
        }

        public async Task<GetCategoryArticlesByIdentifierQueryResult> Handle(GetCategoryArticlesByIdentifierQuery request, CancellationToken cancellationToken)
        {
            var categoryPage = await _contentService.GetCategoryArticlesByIdentifier(request.Slug);

            var apprenticeSavedArticles = new ApprenticeArticleCollection();

            if (request.Id != null)
            {
                apprenticeSavedArticles = await _accountsApiClient.Get<ApprenticeArticleCollection>(new GetApprenticeArticlesRequest(request.Id.Value));
            }

            List<ApprenticeAppArticlePage> articles = new();

            if (categoryPage != null)
            {
                foreach (var childPage in categoryPage.IncludedEntries)
                {
                    ApprenticeAppArticlePage p = childPage.Fields.ToObject<ApprenticeAppArticlePage>();
                    p.Id = childPage.SystemProperties.Id;
                    p.ParentPageEntityId = categoryPage.SystemProperties.Id;
                    articles.Add(p);
                }
            }

            return new GetCategoryArticlesByIdentifierQueryResult
            {
                CategoryPage = categoryPage,
                Articles = articles,
                ApprenticeArticles = apprenticeSavedArticles
            };
        }
    }
}