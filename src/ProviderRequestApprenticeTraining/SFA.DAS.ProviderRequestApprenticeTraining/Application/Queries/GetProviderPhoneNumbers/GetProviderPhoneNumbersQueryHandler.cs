﻿using MediatR;
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
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _providerCoursesApiClient;
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;

        private readonly ILogger<GetProviderPhoneNumbersQueryHandler> _logger;

        public GetProviderPhoneNumbersQueryHandler(
            IProviderCoursesApiClient<ProviderCoursesApiConfiguration> providerCoursesApiClient,
            IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient,
            ILogger<GetProviderPhoneNumbersQueryHandler> logger)
        {
            _providerCoursesApiClient = providerCoursesApiClient;
            _roatpCourseManagementApiClient = roatpCourseManagementApiClient;
            _logger = logger;
        }

        public async Task<GetProviderPhoneNumbersResult> Handle(GetProviderPhoneNumbersQuery request, CancellationToken cancellationToken)
        {
            var providerCourses = await _providerCoursesApiClient.
                Get<List<ProviderCourse>>(new GetProviderCoursesRequest(
                    request.Ukprn));

            var providerSummary = await _roatpCourseManagementApiClient.
                Get<GetProviderSummaryResponse>(new GetRoatpProviderRequest(
                    Convert.ToInt32(request.Ukprn)));

            var phoneNumbers = (providerCourses?.Where(pc => !string.IsNullOrWhiteSpace(pc.ContactUsPhoneNumber))
                .Select(pc => pc.ContactUsPhoneNumber.RemoveWhitespace()) ?? Enumerable.Empty<string>())
                .Append(providerSummary?.Phone?.RemoveWhitespace() ?? string.Empty)
                .Where(phone => !string.IsNullOrWhiteSpace(phone))
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
