using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
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

            var organisation = await _refDataApi.Get<GetLatestDetailsApiResponse>(new GetLatestDetailsRequest(request.Identifier, request.OrganisationType));

            return new GetLatestDetailsResult
            {
                OrganisationDetail = organisation
            };
        }
    }
}
