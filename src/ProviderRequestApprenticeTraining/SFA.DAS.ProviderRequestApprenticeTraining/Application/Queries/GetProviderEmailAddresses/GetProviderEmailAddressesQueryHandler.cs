using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmailAddresses
{
    public class GetProviderEmailAddressesQueryHandler : IRequestHandler<GetProviderEmailAddressesQuery, GetProviderEmailAddressesResult>
    {
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _providerCoursesApiClient;
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;

        private readonly ILogger<GetProviderEmailAddressesQueryHandler> _logger;

        public GetProviderEmailAddressesQueryHandler(
            IProviderCoursesApiClient<ProviderCoursesApiConfiguration> providerCoursesApiClient,
            IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient,
            ILogger<GetProviderEmailAddressesQueryHandler> logger)
        {
            _providerCoursesApiClient = providerCoursesApiClient;
            _roatpCourseManagementApiClient = roatpCourseManagementApiClient;
            _logger = logger;
        }

        public async Task<GetProviderEmailAddressesResult> Handle(GetProviderEmailAddressesQuery request, CancellationToken cancellationToken)
        {
            var providerCourses = await _providerCoursesApiClient.
                Get<List<ProviderCourse>>(new GetProviderCoursesRequest(
                    request.Ukprn));

            var providerSummary = await _roatpCourseManagementApiClient.
                Get<GetProviderSummaryResponse>(new GetRoatpProviderRequest(
                    Convert.ToInt32(request.Ukprn)));

            return new GetProviderEmailAddressesResult
            {
                EmailAddresses = providerCourses
                .Select(pc => pc.ContactUsEmail)
                .Append(providerSummary.Email)
                .Distinct()
                .Order()
                .ToList()
            };
        }
    }
}
