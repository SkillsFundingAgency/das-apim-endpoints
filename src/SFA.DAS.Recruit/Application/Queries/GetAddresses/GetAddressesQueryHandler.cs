using MediatR;
using SFA.DAS.Recruit.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetAddresses
{
    public class GetAddressesQueryHandler : IRequestHandler<GetAddressesQuery, GetAddressesQueryResult>
    {
        private readonly ILocationApiClient<LocationApiConfiguration> _locationApiClient;
        private readonly RecruitConfiguration _config;

        public GetAddressesQueryHandler(ILocationApiClient<LocationApiConfiguration> locationApiClient, RecruitConfiguration config)
        {
            _locationApiClient = locationApiClient;
            _config = config;
        }

        public async Task<GetAddressesQueryResult> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Query)) throw new ArgumentException($"Query is required", nameof(GetAddressesQuery.Query));

           // var addressesResponse = await _locationApiClient.Get<GetAddressesListResponse>(new GetAddressesQueryRequest(request.Query, _config.LocationsApiMinMatch));

            return new GetAddressesQueryResult(new GetAddressesListResponse 
            { 
                Addresses = new List<GetAddressesListItem> {
                    new GetAddressesListItem { County = "GB", House = "bb", Latitude = 123, Locality = "11", Longitude = 321, Match = 1, Postcode = "mk42 0uu", PostTown = "Bedford", Street = "Halifax Road", Uprn = "222" } ,
                        new GetAddressesListItem { County = "GB", House = "12", Latitude = 123, Locality = "11", Longitude = 321, Match = 1, Postcode = "mk42 0uu", PostTown = "Bedford", Street = "Halifax Road", Uprn = "222" },
                            new GetAddressesListItem { County = "GB", House = "13", Latitude = 123, Locality = "11", Longitude = 321, Match = 1, Postcode = "mk42 0uu", PostTown = "Bedford", Street = "Halifax Road", Uprn = "222" }

                }
            });
        }
    }
}
