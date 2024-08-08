using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRequestApprenticeTraining.Extensions;
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
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;

        private readonly ILogger<GetProviderEmailAddressesQueryHandler> _logger;

        public GetProviderEmailAddressesQueryHandler(
            IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient,
            ILogger<GetProviderEmailAddressesQueryHandler> logger)
        {
            _roatpCourseManagementApiClient = roatpCourseManagementApiClient;
            _logger = logger;
        }

        public async Task<GetProviderEmailAddressesResult> Handle(GetProviderEmailAddressesQuery request, CancellationToken cancellationToken)
        {
            var providerCourses = await _roatpCourseManagementApiClient.
                Get<List<ProviderCourse>>(new GetProviderCoursesRequest(
                    request.Ukprn));

            var emailAddresses = (providerCourses?.Where(pc => !string.IsNullOrWhiteSpace(pc.ContactUsEmail))
                .Select(pc => pc.ContactUsEmail.RemoveWhitespace()) ?? Enumerable.Empty<string>())
                .Append(request.UserEmailAddress.RemoveWhitespace() ?? string.Empty)
                .Where(email => !string.IsNullOrWhiteSpace(email))
                .Distinct()
                .Order()
                .ToList();

            return new GetProviderEmailAddressesResult
            {
                EmailAddresses = emailAddresses,
            };
        }
    }
}
