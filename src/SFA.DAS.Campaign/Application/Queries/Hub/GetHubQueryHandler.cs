using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Application.Queries.Articles;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.Hub
{
    public class GetHubQueryHandler : IRequestHandler<GetHubQuery, GetHubQueryResult>
    {
        private readonly IReliableCacheStorageService _reliableCacheStorageService;

        public GetHubQueryHandler(
            IReliableCacheStorageService reliableCacheStorageService)
        {
            _reliableCacheStorageService = reliableCacheStorageService;
        }

        public async Task<GetHubQueryResult> Handle(GetHubQuery request, CancellationToken cancellationToken)
        {
            var article = await _reliableCacheStorageService.GetData<CmsContent>(new GetHubEntriesRequest(request.Hub.ToTitleCase()), $"{request.Hub.ToTitleCase()}_hub");

            var pageModel = new CmsPageModel().Build(article);

            return new GetHubQueryResult
            {
                PageModel = pageModel
            };
        }
    }
}
