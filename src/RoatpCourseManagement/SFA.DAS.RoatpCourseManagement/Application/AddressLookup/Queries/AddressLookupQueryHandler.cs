﻿using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.AddressLookup.Queries
{
    public class AddressLookupQueryHandler : IRequestHandler<AddresssLookupQuery, AddresssLookupQueryResult>
    {
        private readonly ILocationLookupService _locationLookupService;
        private readonly ILogger<AddressLookupQueryHandler> _logger;

        public AddressLookupQueryHandler(ILocationLookupService locationLookupService, ILogger<AddressLookupQueryHandler> logger)
        {
            _locationLookupService = locationLookupService;
            _logger = logger;
        }

        public async Task<AddresssLookupQueryResult> Handle(AddresssLookupQuery request, CancellationToken cancellationToken)
        {
            var result = await _locationLookupService.GetExactMatchAddresses(request.Postcode);
            if (result == null)
            {
                _logger.LogWarning($"Invalid postcode: {request.Postcode}.");
                return null;
            }
            _logger.LogInformation($"Found {result.Addresses.Count()} addresses for postcode: {request.Postcode}");
            var response = new AddresssLookupQueryResult();
            response.Addresses = result.Addresses.Select(a => (AddressItem)a).ToList();
            return response;
        }
    }
}
