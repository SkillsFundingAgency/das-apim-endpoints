using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerAccounts.Exceptions;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails
{
    public class GetLatestDetailsQueryHandler : IRequestHandler<GetLatestDetailsQuery, GetLatestDetailsResult>
    {
        private readonly ILogger<GetLatestDetailsQueryHandler> _logger;
        private readonly IReferenceDataApiClient<ReferenceDataApiConfiguration> _refDataApi;

        public GetLatestDetailsQueryHandler(ILogger<GetLatestDetailsQueryHandler> logger, IReferenceDataApiClient<ReferenceDataApiConfiguration> referenceDataApiClient)
        {
            _logger = logger;
            _refDataApi = referenceDataApiClient;
        }

        public async Task<GetLatestDetailsResult> Handle(GetLatestDetailsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Searching for Organisation with Identifier: {request.Identifier}");

            var response = await _refDataApi.GetWithResponseCode<GetLatestDetailsApiResponse>(new GetLatestDetailsRequest(request.Identifier, request.OrganisationType));

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new OrganisationNotFoundException(request.OrganisationType, request.Identifier);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new InvalidGetOrganisationRequest(response.ErrorContent);
            }

            return new GetLatestDetailsResult
            {
                OrganisationDetail = response.Body
            };
        }
    }
}
