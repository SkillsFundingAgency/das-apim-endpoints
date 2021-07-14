using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Application.Queries.Hub;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Campaign.Application.Queries.LandingPage
{
    public class GetLandingPageQueryHandler : IRequestHandler<GetLandingPageQuery, GetLandingPageQueryResult>
    {
        private readonly IReliableCacheStorageService _reliableCacheStorageService;

        public GetLandingPageQueryHandler(
            IReliableCacheStorageService reliableCacheStorageService)
        {
            _reliableCacheStorageService = reliableCacheStorageService;
        }

        public async Task<GetLandingPageQueryResult> Handle(GetLandingPageQuery request, CancellationToken cancellationToken)
        {
            var landingPage = await _reliableCacheStorageService.GetData<CmsContent>(new GetLandingPageRequest(request.Hub.ToTitleCase(), request.Slug), $"{request.Hub.ToTitleCase()}_{request.Slug}_landingPage");

            var pageModel = new LandingPageModel().Build(landingPage);

            return new GetLandingPageQueryResult
            {
                PageModel = pageModel
            };
        }
    }
}
