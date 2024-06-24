using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.Models.Contentful;
using SFA.DAS.ApprenticeApp.Services;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetContentByEntityIdAndChildrenQueryHandler : IRequestHandler<GetContentByEntityIdAndChildrenQuery, GetContentByEntityIdAndChildrenQueryResult>
    {
        private readonly ContentService _contentService;

        public GetContentByEntityIdAndChildrenQueryHandler(
            ContentService contentService
            )
        {
            _contentService = contentService;
        }

        public async Task<GetContentByEntityIdAndChildrenQueryResult> Handle(GetContentByEntityIdAndChildrenQuery request, CancellationToken cancellationToken)
        {
            var parent = await _contentService.GetPageByIdWithChildren(request.EntryId);

            List<ApprenticeAppArticlePage> childArticles = new();

            foreach (var relatedContentItem in parent.IncludedEntries)
            {
                ApprenticeAppArticlePage item = relatedContentItem.Fields.ToObject<ApprenticeAppArticlePage>();
                item.Sys = relatedContentItem.SystemProperties;
                childArticles.Add(item);
            }

            return new GetContentByEntityIdAndChildrenQueryResult
            {
                Parent = parent,
                ChildArticles = childArticles
            };
        }
    }
}
