using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider
{
    public class GetTrainingCourseProviderQueryHandler : IRequestHandler<GetTrainingCourseProviderQuery, GetTrainingCourseProviderResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpV2ApiClient;
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly IShortlistService _shortlistService;
        private readonly ILocationLookupService _locationLookupService;

        public GetTrainingCourseProviderQueryHandler(
            ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient,
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> employerFeedbackApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            IShortlistService shortlistService,
            ILocationLookupService locationLookupService,
            IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpV2ApiClient)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _employerFeedbackApiClient = employerFeedbackApiClient;
            _coursesApiClient = coursesApiClient;
            _shortlistService = shortlistService;
            _locationLookupService = locationLookupService;
            _roatpV2ApiClient = roatpV2ApiClient;
        }
        public async Task<GetTrainingCourseProviderResult> Handle(GetTrainingCourseProviderQuery request, CancellationToken cancellationToken)
        {
            var locationTask = _locationLookupService.GetLocationInformation(request.Location, request.Lat, request.Lon);
            var courseTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.CourseId));

            await Task.WhenAll(locationTask, courseTask);

            var ukprnsCountTask = _roatpV2ApiClient.Get<GetTotalProvidersForStandardResponse>(
                new GetTotalProvidersForStandardRequest(request.CourseId));

            var providerCoursesTask = _roatpV2ApiClient.Get<List<GetProviderAdditionalStandardsItem>>(
                new GetProviderAdditionalStandardsRequest(request.ProviderId));

            var overallAchievementRatesTask = _roatpV2ApiClient.Get<GetOverallAchievementRateResponse>(
                new GetOverallAchievementRateRequest(courseTask.Result.SectorSubjectAreaTier2Description));

            var apprenticeFeedbackTask = _apprenticeFeedbackApiClient.GetWithResponseCode<GetApprenticeFeedbackResponse>(new GetApprenticeFeedbackDetailsRequest(request.ProviderId));
            var employerFeedbackTask = _employerFeedbackApiClient.GetWithResponseCode<GetEmployerFeedbackResponse>(new GetEmployerFeedbackDetailsRequest(request.ProviderId));

            var shortlistCountTask = _shortlistService.GetShortlistItemCount(request.ShortlistUserId);
            
            await Task.WhenAll(providerCoursesTask, ukprnsCountTask, overallAchievementRatesTask, shortlistCountTask, apprenticeFeedbackTask, employerFeedbackTask);

            var providerDetails = await GetProviderDetails(request.ProviderId, request.CourseId, locationTask.Result);

            if(providerDetails != null && apprenticeFeedbackTask.Result?.StatusCode == System.Net.HttpStatusCode.OK && apprenticeFeedbackTask.Result.Body != null)
            {
                providerDetails.ApprenticeFeedback = apprenticeFeedbackTask.Result.Body;
            }
            
            if(providerTask.Result != null && employerFeedbackTask.Result?.StatusCode == System.Net.HttpStatusCode.OK && employerFeedbackTask.Result.Body != null)
            {
                providerTask.Result.EmployerFeedback = employerFeedbackTask.Result.Body;
            }

            var additionalCourses = BuildAdditionalCoursesResponse(providerCoursesTask.Result);

            return new GetTrainingCourseProviderResult
            {
                ProviderStandard = providerDetails,
                Course = courseTask.Result,
                AdditionalCourses = additionalCourses,
                OverallAchievementRates = overallAchievementRatesTask.Result.OverallAchievementRates,
                TotalProviders = ukprnsCountTask.Result.ProvidersCount,
                TotalProvidersAtLocation = ukprnsCountTask.Result.ProvidersCount,
                Location = locationTask.Result,
                ShortlistItemCount = shortlistCountTask.Result
            };
        }

        private static IEnumerable<GetAdditionalCourseListItem> BuildAdditionalCoursesResponse(List<GetProviderAdditionalStandardsItem> providerCourses)
        {
            return providerCourses
                .Select(course => new GetAdditionalCourseListItem
                {
                    Id = course.LarsCode,
                    Level = course.Level,
                    Title = course.CourseName
                })
                .OrderBy(c => c.Title)
                .ToList();
        }

        private async Task<GetProviderStandardItem> GetProviderDetails(int providerId, int courseId, LocationItem locationItem)
        {
            var request = new GetProviderByCourseAndUkPrnRequest(providerId, courseId, locationItem?.GeoPoint?.FirstOrDefault(), locationItem?.GeoPoint?.LastOrDefault());
            var apiResponse = await _courseDeliveryApiClient.Get<GetProviderDetailsForCourse>(request); 

            if (apiResponse != null && apiResponse.ProviderHeadOfficeDistanceInMiles == 0 && locationItem != null)
            {
                if (apiResponse != null)
                {
                    //provider found without location
                    GetProviderStandardItem providerDetails = new();
                    providerDetails.DeliveryTypes = new List<GetDeliveryTypeItem>
                    {
                        new GetDeliveryTypeItem
                        {
                            DeliveryModes = "NotFound"
                        }
                    };
                    return providerDetails;
                }
            }

            if (apiResponse == null) return null;

            var result = new GetProviderStandardItem()
            {
                Ukprn = apiResponse.Ukprn,
                Name = apiResponse.Name,
                TradingName = apiResponse.TradingName,
                MarketingInfo = apiResponse.MarketingInfo,
                StandardInfoUrl = apiResponse.StandardInfoUrl,
                Email = apiResponse.Email,
                Phone = apiResponse.Phone,
                StandardId = apiResponse.LarsCode,
                //ShortlistId
                AchievementRates = apiResponse.AchievementRates
            };

            result.ProviderAddress = new GetProviderStandardItemAddress
            {
                Address1 = apiResponse.Address1,
                Address2 = apiResponse.Address2,
                Address3 = apiResponse.Address3,
                Address4 = apiResponse.Address4,
                Town = apiResponse.Town,
                Postcode = apiResponse.Postcode,
                DistanceInMiles = apiResponse.ProviderHeadOfficeDistanceInMiles ?? 0
            };

            return result;
        }
    }
}