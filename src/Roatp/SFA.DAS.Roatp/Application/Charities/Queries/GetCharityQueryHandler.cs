using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Roatp.Infrastructure;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.Application.Charities.Queries;

public class GetCharityQueryHandler : IRequestHandler<GetCharityQuery, GetCharityResult>
{
    private readonly ICharitiesRestApiClient _charityClient;
    private readonly ILogger<GetCharityQueryHandler> _logger;

    public GetCharityQueryHandler(ICharitiesRestApiClient charityClient, ILogger<GetCharityQueryHandler> logger)
    {
        _charityClient = charityClient;
        _logger = logger;
    }
    public async Task<GetCharityResult> Handle(GetCharityQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get charity request received for registration number {registrationNumber}", request.RegistrationNumber);

        var response = await _charityClient.GetCharities(request.RegistrationNumber, cancellationToken);
        switch (response.ResponseMessage.StatusCode)
        {
            case HttpStatusCode.OK:
                {
                    return new GetCharityResult(response.GetContent());
                }
            case HttpStatusCode.NotFound:
                return null;
            default:
                throw new InvalidOperationException(
                    $"Invalid operation occurred trying to retrieve charity for registration number {request.RegistrationNumber}");
        }
    }
}
