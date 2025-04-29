using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider
{
    public class GetTrainingCourseProviderQueryHandler : IRequestHandler<GetTrainingCourseProviderQuery, GetTrainingCourseProviderResult>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly IShortlistApiClient<ShortlistApiConfiguration> _shortlistApiClient;
        private readonly ILocationLookupService _locationLookupService;

        public GetTrainingCourseProviderQueryHandler(
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> employerFeedbackApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            IShortlistApiClient<ShortlistApiConfiguration> shortlistApiClient,
            ILocationLookupService locationLookupService,
            IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient)
        {
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _employerFeedbackApiClient = employerFeedbackApiClient;
            _coursesApiClient = coursesApiClient;
            _shortlistApiClient = shortlistApiClient;
            _locationLookupService = locationLookupService;
            _roatpCourseManagementApiClient = roatpCourseManagementApiClient;
        }

        public async Task<GetTrainingCourseProviderResult> Handle(GetTrainingCourseProviderQuery request, CancellationToken cancellationToken)
        {
            var locationTask = _locationLookupService.GetLocationInformation(request.Location, request.Lat, request.Lon);
            var courseTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.CourseId));

            await Task.WhenAll(locationTask, courseTask);

            var ukprnsCountTask = _roatpCourseManagementApiClient.Get<GetTotalProvidersForStandardResponse>(
                new GetTotalProvidersForStandardRequest(request.CourseId));

            var providerCoursesTask = _roatpCourseManagementApiClient.Get<List<GetProviderAdditionalStandardsItem>>(
                new GetProviderAdditionalStandardsRequest(request.ProviderId));

            var overallAchievementRatesTask = _roatpCourseManagementApiClient.Get<GetOverallAchievementRateResponse>(
                new GetOverallAchievementRateRequest(courseTask.Result.SectorSubjectAreaTier1));

            var apprenticeFeedbackTask = _apprenticeFeedbackApiClient.GetWithResponseCode<GetApprenticeFeedbackResponse>(new GetApprenticeFeedbackDetailsRequest(request.ProviderId));
            var employerFeedbackTask = _employerFeedbackApiClient.GetWithResponseCode<GetEmployerFeedbackResponse>(new GetEmployerFeedbackDetailsRequest(request.ProviderId));

            var shortlistTask = request.ShortlistUserId.HasValue
                ? _shortlistApiClient.Get<List<ShortlistItem>>(new GetShortlistForUserRequest(request.ShortlistUserId.Value))
                : Task.FromResult(new List<ShortlistItem>());

            await Task.WhenAll(providerCoursesTask, ukprnsCountTask, overallAchievementRatesTask, shortlistTask, apprenticeFeedbackTask, employerFeedbackTask);

            var providerDetails = await GetProviderDetails(request.ProviderId, request.CourseId, locationTask.Result, shortlistTask.Result);

            if (providerDetails != null && apprenticeFeedbackTask.Result?.StatusCode == System.Net.HttpStatusCode.OK && apprenticeFeedbackTask.Result.Body != null)
            {
                providerDetails.ApprenticeFeedback = apprenticeFeedbackTask.Result.Body;
            }

            if (providerDetails != null && employerFeedbackTask.Result?.StatusCode == System.Net.HttpStatusCode.OK && employerFeedbackTask.Result.Body != null)
            {
                providerDetails.EmployerFeedback = employerFeedbackTask.Result.Body;
            }


            var additionalCourses = BuildAdditionalCoursesResponse(providerCoursesTask.Result
                .Where(x => x.IsApprovedByRegulator != false || string.IsNullOrEmpty(x.ApprovalBody)).ToList());

            return new GetTrainingCourseProviderResult
            {
                ProviderStandard = providerDetails,
                Course = courseTask.Result,
                AdditionalCourses = additionalCourses,
                OverallAchievementRates = overallAchievementRatesTask.Result.OverallAchievementRates,
                TotalProviders = ukprnsCountTask.Result.ProvidersCount,
                TotalProvidersAtLocation = ukprnsCountTask.Result.ProvidersCount,
                Location = locationTask.Result,
                ShortlistItemCount = shortlistTask.Result?.Count ?? 0
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

        private async Task<GetProviderStandardItem> GetProviderDetails(int providerId, int courseId, LocationItem locationItem, List<ShortlistItem> shortlistItems)
        {
            var latitude = locationItem?.GeoPoint?.FirstOrDefault();
            var longitude = locationItem?.GeoPoint?.LastOrDefault();

            var request = new GetProviderByCourseAndUkprnRequest(providerId, courseId, latitude, longitude);
            var apiResponse = await _roatpCourseManagementApiClient.Get<GetProviderDetailsForCourse>(request);

            if (apiResponse == null) return null;

            if (apiResponse != null && apiResponse.ProviderHeadOfficeDistanceInMiles == 0 && locationItem != null)
            {
                //provider found without location
                return new GetProviderStandardItem();
            }

            var matchingShortlistItem = (ShortlistItem)null;
            if (shortlistItems != null)
            {
                matchingShortlistItem = shortlistItems.FirstOrDefault(s =>
                                            s.Ukprn == providerId && s.Larscode == courseId && s.Latitude == latitude &&
                                            s.Longitude == longitude)
                                        ?? shortlistItems.FirstOrDefault(s =>
                                            s.Ukprn == providerId && s.Larscode == courseId);
            }


            var result = new GetProviderStandardItem
            {
                Ukprn = apiResponse.Ukprn,
                Name = apiResponse.Name,
                TradingName = apiResponse.TradingName,
                MarketingInfo = apiResponse.MarketingInfo,
                StandardInfoUrl = apiResponse.StandardInfoUrl,
                Email = apiResponse.Email,
                Phone = apiResponse.Phone,
                StandardId = apiResponse.LarsCode,
                ShortlistId = matchingShortlistItem?.Id,
                AchievementRates = apiResponse.AchievementRates,
                ProviderAddress = new GetProviderStandardItemAddress
                {
                    Address1 = apiResponse.Address1,
                    Address2 = apiResponse.Address2,
                    Address3 = apiResponse.Address3,
                    Address4 = apiResponse.Address4,
                    Town = apiResponse.Town,
                    Postcode = apiResponse.Postcode,
                    DistanceInMiles = apiResponse.ProviderHeadOfficeDistanceInMiles ?? 0
                },
                DeliveryModels = apiResponse.DeliveryModels
            };

            return result;
        }
    }
}