using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndexLocation
{
    public class IndexLocationQueryHandler : IRequestHandler<IndexLocationQuery, IndexLocationResult>
    {
        private readonly ILocationLookupService _locationLookupService;

        public IndexLocationQueryHandler(ILocationLookupService locationLookupService)
        {
            _locationLookupService = locationLookupService;
        }
        public async Task<IndexLocationResult> Handle(IndexLocationQuery request, CancellationToken cancellationToken)
        {
            var result = await _locationLookupService.GetLocationInformation(request.LocationSearchTerm, 0, 0, false);

            return new IndexLocationResult()
            {
                LocationItem = result
            };
        }
    }
}