using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.Services;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetContentQueryHandler : IRequestHandler<GetContentQuery, GetContentQueryResult>
    {
        private readonly ContentService _contentService;

        public GetContentQueryHandler(
            ContentService contentService
            )
        {
            _contentService = contentService;
        }

        public async Task<GetContentQueryResult> Handle(GetContentQuery request, CancellationToken cancellationToken)
        {
            var item = await _contentService.GetPageById(request.EntryId);

            return new GetContentQueryResult
            {
                Item = item
            };
        }
    }
}