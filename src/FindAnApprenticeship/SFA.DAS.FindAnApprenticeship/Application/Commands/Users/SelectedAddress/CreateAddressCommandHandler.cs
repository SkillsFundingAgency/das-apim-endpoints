﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.Address;
public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, Unit>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;
    private readonly ILocationLookupService _locationLookupService;

    public CreateAddressCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient, ILocationLookupService locationLookupService)
    {
        _candidateApiClient = candidateApiClient;
        _locationLookupService = locationLookupService;
    }

    public async Task<Unit> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var geoPoint = await _locationLookupService.GetLocationInformation(request.Postcode, default, default);

        var postData = new PutCandidateAddressApiRequestData
        {
            Uprn = request.Uprn,
            Email = request.Email,
            AddressLine1 = request.AddressLine1,
            AddressLine2 = request.AddressLine2,
            AddressLine3 = request.AddressLine3,
            AddressLine4 = request.AddressLine4,
            Latitude = geoPoint is not null ? geoPoint.GeoPoint[0] : 0,
            Longitude = geoPoint is not null ? geoPoint.GeoPoint[1] : 0,
            Postcode = request.Postcode
        };

        var postRequest = new PutCandidateAddressApiRequest(request.CandidateId, postData);

        var response = await _candidateApiClient.PutWithResponseCode<PostCandidateAddressApiResponse>(postRequest);

        if ((int)response.StatusCode > 300)
        {
            throw new InvalidOperationException();
        }

        return Unit.Value;
    }
}
