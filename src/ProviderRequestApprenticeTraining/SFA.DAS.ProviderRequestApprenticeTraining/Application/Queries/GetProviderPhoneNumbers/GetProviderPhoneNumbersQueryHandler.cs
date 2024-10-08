using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.ProviderRequestApprenticeTraining.Extensions;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.Interfaces;

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
            var providerCourses = await _roatpCourseManagementApiClient
                .Get<List<ProviderCourse>>(new GetProviderCoursesRequest(request.Ukprn));

            var uniquePhoneNumbers = providerCourses?
                .Select(pc => pc.ContactUsPhoneNumber)
                .Where(phone => !string.IsNullOrEmpty(phone))
                .GroupBy(phone => phone.RemoveWhitespace())
                .Select(group => group.First())
                .Order()
                .ToList();

            return new GetProviderPhoneNumbersResult
            {
                PhoneNumbers = uniquePhoneNumbers ?? new List<string>()
            };
        }
    }
}
