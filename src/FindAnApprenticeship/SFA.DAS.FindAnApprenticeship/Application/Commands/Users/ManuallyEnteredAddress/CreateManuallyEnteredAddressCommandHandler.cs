using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.ManuallyEnteredAddress;
public class CreateManuallyEnteredAddressCommandHandler : IRequestHandler<CreateManuallyEnteredAddressCommand, Unit>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;
    private readonly ILocationLookupService _locationLookupService;

    public CreateManuallyEnteredAddressCommandHandler(
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient, 
        ILocationLookupService locationLookupService)
    {
        _candidateApiClient = candidateApiClient;
        _locationLookupService = locationLookupService;
    }

    public async Task<Unit> Handle(CreateManuallyEnteredAddressCommand request, CancellationToken cancellationToken)
    {
        var geoPoint = await _locationLookupService.GetLocationInformation(request.Postcode, default, default);

        var postData = new PutCandidateAddressApiRequestData
        {
            Email = request.Email,
            AddressLine1 = request.AddressLine1,
            AddressLine2 = request.AddressLine2,
            AddressLine3 = request.TownOrCity,
            AddressLine4 = request.County,
            Latitude = geoPoint.GeoPoint[0],
            Longitude = geoPoint.GeoPoint[1],
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
