using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterestsLocation
{
    public class BrowseByInterestsLocationQueryHandler : IRequestHandler<BrowseByInterestsLocationQuery, BrowseByInterestsLocationQueryResult>
    {
        private readonly ILocationLookupService _locationLookupService;

        public BrowseByInterestsLocationQueryHandler(ILocationLookupService locationLookupService)
        {
            _locationLookupService = locationLookupService;
        }
        public async Task<BrowseByInterestsLocationQueryResult> Handle(BrowseByInterestsLocationQuery request, CancellationToken cancellationToken)
        {
            var result = await _locationLookupService.GetLocationInformation(request.LocationSearchTerm, 0, 0, false);

            return new BrowseByInterestsLocationQueryResult
            {
                LocationItem = result
            };
        }
    }
}