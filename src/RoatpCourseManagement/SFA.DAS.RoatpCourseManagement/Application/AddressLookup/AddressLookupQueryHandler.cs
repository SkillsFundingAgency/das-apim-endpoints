using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.AddressLookup
{
    public class AddressLookupQueryHandler : IRequestHandler<AddresssLookupQuery, AddresssLookupQueryResponse>
    {
        private readonly ILocationLookupService _locationLookupService;
        private readonly ILogger<AddressLookupQueryHandler> _logger;

        public AddressLookupQueryHandler(ILocationLookupService locationLookupService, ILogger<AddressLookupQueryHandler> logger)
        {
            _locationLookupService = locationLookupService;
            _logger = logger;
        }

        public async Task<AddresssLookupQueryResponse> Handle(AddresssLookupQuery request, CancellationToken cancellationToken)
        {
            var result = await _locationLookupService.GetExactMatchAddresses(request.Postcode);
            if (result == null)
            {
                _logger.LogWarning($"Address lookup did not respond with success code for postcode: {request.Postcode}.");
                return null;
            }
            _logger.LogInformation($"Found {result.Addresses.Count()} addresses for postcode: {request.Postcode}");
            var response = new AddresssLookupQueryResponse();
            response.Addresses = result.Addresses.Select(a => (AddressItem)a);
            return response;
        }
    }
}
