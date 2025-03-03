using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderWebsite
{
    public class GetProviderWebsiteQueryHandler : IRequestHandler<GetProviderWebsiteQuery, GetProviderWebsiteResult>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;
        private readonly ILogger<GetProviderWebsiteQueryHandler> _logger;

        public GetProviderWebsiteQueryHandler(
            IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient,
            ILogger<GetProviderWebsiteQueryHandler> logger)
        {
            _roatpCourseManagementApiClient = roatpCourseManagementApiClient;
            _logger = logger;
        }

        public async Task<GetProviderWebsiteResult> Handle(GetProviderWebsiteQuery request, CancellationToken cancellationToken)
        { 
            var providerSummary = await _roatpCourseManagementApiClient.
                Get<GetProviderSummaryResponse>(new GetRoatpProviderRequest(
                    Convert.ToInt32(request.Ukprn)));

            return new GetProviderWebsiteResult
            {
                Website = providerSummary.ContactUrl
            };
        }
    }
}
