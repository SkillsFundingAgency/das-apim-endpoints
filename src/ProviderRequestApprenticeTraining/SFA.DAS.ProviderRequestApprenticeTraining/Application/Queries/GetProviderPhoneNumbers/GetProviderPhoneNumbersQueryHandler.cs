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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderPhoneNumbers
{
    public class GetProviderPhoneNumbersQueryHandler : IRequestHandler<GetProviderPhoneNumbersQuery, GetProviderPhoneNumbersResult>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;
        private readonly ILogger<GetProviderPhoneNumbersQueryHandler> _logger;

        public GetProviderPhoneNumbersQueryHandler(

            IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient,
            ILogger<GetProviderPhoneNumbersQueryHandler> logger)
        {
            _roatpCourseManagementApiClient = roatpCourseManagementApiClient;
            _logger = logger;
        }

        public async Task<GetProviderPhoneNumbersResult> Handle(GetProviderPhoneNumbersQuery request, CancellationToken cancellationToken)
        {
            var providerCourses = await _roatpCourseManagementApiClient.
                Get<List<ProviderCourse>>(new GetProviderCoursesRequest(
                    request.Ukprn));

            var phoneNumbers = (providerCourses?
                .Select(pc => pc.ContactUsPhoneNumber) ?? Enumerable.Empty<string>())
                .Select(phone => phone.RemoveWhitespace())
                .Where(phone => !string.IsNullOrEmpty(phone))
                .Distinct()
                .Order()
                .ToList();


            return new GetProviderPhoneNumbersResult
            {
                PhoneNumbers = phoneNumbers
            };
        }
    }
}
