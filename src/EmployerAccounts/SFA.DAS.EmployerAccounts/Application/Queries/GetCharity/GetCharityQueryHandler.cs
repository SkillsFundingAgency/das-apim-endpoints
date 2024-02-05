using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetCharity
{
    public class GetCharityQueryHandler : IRequestHandler<GetCharityQuery, GetCharityResult>
    {
        private readonly ILogger<GetCharityQueryHandler> _logger;
        private readonly IReferenceDataApiClient<ReferenceDataApiConfiguration> _refDataApi;

        public GetCharityQueryHandler(ILogger<GetCharityQueryHandler> logger, IReferenceDataApiClient<ReferenceDataApiConfiguration> referenceDataApiClient)
        {
            _logger = logger;
            _refDataApi = referenceDataApiClient;
        }

        public async Task<GetCharityResult> Handle(GetCharityQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Searching for Charity with RegistrationNumber: {request.RegistrationNumber}");

            var response = await _refDataApi.GetWithResponseCode<GetCharityApiResponse>(new GetCharityRequest(request.RegistrationNumber));

            return response.Body;
        }
    }
}