using System.Collections.Generic;
using System.Linq;
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
    public class GetUserSavedArticlesQueryHandler : IRequestHandler<GetUserSavedArticlesQuery, GetUserSavedArticlesQueryResult>
    {
        private readonly ContentService _contentService;
        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _accountsApiClient;

        public GetUserSavedArticlesQueryHandler(
            ContentService contentService,
            IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> accountsApiClient
            )
        {
            _contentService = contentService;
            _accountsApiClient = accountsApiClient;
        }

        public async Task<GetUserSavedArticlesQueryResult> Handle(GetUserSavedArticlesQuery request, CancellationToken cancellationToken)
        {
            var result = await _accountsApiClient.Get<ApprenticeArticleCollection>(new GetApprenticeArticlesRequest(request.ApprenticeId));
            var apprenticeSavedArticles = await _accountsApiClient.Get<ApprenticeArticleCollection>(new GetApprenticeArticlesRequest(request.ApprenticeId));

            List<Page> articles = new();

            foreach (string entryId in result.ApprenticeArticles.Where(x => x.IsSaved == true).Select(x => x.EntryId))
            {
                var page = await _contentService.GetPageById(entryId);
                if (page != null)
                {
                    articles.Add(page);
                }
            }

            return new GetUserSavedArticlesQueryResult
            {
                Articles = articles,
                ApprenticeArticles = apprenticeSavedArticles
            };
        }
    }
}
