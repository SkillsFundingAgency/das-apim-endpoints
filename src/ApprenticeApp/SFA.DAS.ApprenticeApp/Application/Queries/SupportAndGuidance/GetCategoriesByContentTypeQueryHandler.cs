using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.Models.Contentful;
using SFA.DAS.ApprenticeApp.Services;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetCategoriesByContentTypeQueryHandler : IRequestHandler<GetCategoriesByContentTypeQuery, GetCategoriesByContentTypeQueryResult>
    {
        private readonly ContentService _contentService;

        public GetCategoriesByContentTypeQueryHandler(
            ContentService contentService
            )
        {
            _contentService = contentService;
        }

        public async Task<GetCategoriesByContentTypeQueryResult> Handle(GetCategoriesByContentTypeQuery request, CancellationToken cancellationToken)
        {
            var categoryPages = await _contentService.GetCategoriesByContentType<Page>(request.ContentType);

            return new GetCategoriesByContentTypeQueryResult
            {
                CategoryPages = categoryPages
            };
        }
    }
}
