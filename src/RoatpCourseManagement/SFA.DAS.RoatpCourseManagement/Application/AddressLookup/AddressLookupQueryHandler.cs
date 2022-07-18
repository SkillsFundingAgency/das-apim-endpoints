using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.AddressLookup
{
    public class AddressLookupQueryHandler : IRequestHandler<AddresssLookupQuery, GetAddressesListResponse>
    {
        private readonly ILocationLookupService _locationLookupService;

        public AddressLookupQueryHandler(ILocationLookupService locationLookupService)
        {
            _locationLookupService = locationLookupService;
        }

        public Task<GetAddressesListResponse> Handle(AddresssLookupQuery request, CancellationToken cancellationToken)
        {
            return _locationLookupService.GetExactMatchAddresses(request.Postcode);
        }
    }
}
