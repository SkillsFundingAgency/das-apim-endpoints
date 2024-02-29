using MediatR;
using SFA.DAS.Campaign.ExternalApi.Requests;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.Campaign.Models;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Campaign.Application.Queries.Panel
{
    public class GetPanelQueryHandler : IRequestHandler<GetPanelQuery, GetPanelQueryResult>
    {
        private readonly IReliableCacheStorageService _reliableCacheStorageService;
        private readonly IContentService _contentService;

        public GetPanelQueryHandler(
            IReliableCacheStorageService reliableCacheStorageService, IContentService contentService)
        {
            _reliableCacheStorageService = reliableCacheStorageService;
            _contentService = contentService;
        }
        public async Task<GetPanelQueryResult> Handle(GetPanelQuery request, CancellationToken cancellationToken)
        {
            var panel = await _reliableCacheStorageService.GetData<CmsContent>(
                new GetPanelRequest(request.Id),
                $"{request.Id}",
                _contentService.HasContent);

            var pageModel = new PanelModel().Build(panel);

            return new GetPanelQueryResult
            {
                Panel = pageModel
            };
        }
    }
}
