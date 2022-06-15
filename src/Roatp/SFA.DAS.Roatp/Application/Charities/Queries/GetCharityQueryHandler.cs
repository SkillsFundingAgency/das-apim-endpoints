using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.Application.Charities.Queries
{
    public class GetCharityQueryHandler : IRequestHandler<GetCharityQuery, GetCharityResult>
    {
        private readonly ICharitiesApiClient<CharitiesApiConfiguration> _charityClient;
        private readonly ILogger<GetCharityQueryHandler> _logger;

        public GetCharityQueryHandler(ICharitiesApiClient<CharitiesApiConfiguration> charityClient, ILogger<GetCharityQueryHandler> logger)
        {
            _charityClient = charityClient;
            _logger = logger;
        }
        public async Task<GetCharityResult> Handle(GetCharityQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get charity request received for registration number {registrationNumber}", request.RegistrationNumber);
            try
            {
                var charity = await _charityClient.Get<GetCharityResponse>(new GetCharityRequest(request.RegistrationNumber));
                return new GetCharityResult(charity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred trying to retrieve charity for registration number {request.RegistrationNumber}");
                throw;
            }
        }
    }
}
