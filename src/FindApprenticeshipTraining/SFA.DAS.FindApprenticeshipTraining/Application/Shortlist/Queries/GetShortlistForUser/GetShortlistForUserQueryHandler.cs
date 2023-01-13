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
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistForUser
{
    public class GetShortlistForUserQueryHandler : IRequestHandler<GetShortlistForUserQuery, GetShortlistForUserResult>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient;
        private readonly IShortlistApiClient<ShortlistApiConfiguration> _shortListApiClient;
        private readonly ICachedCoursesService _cachedCoursesService;

        public GetShortlistForUserQueryHandler(
            IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient,
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> employerFeedbackApiClient,
            ICachedCoursesService cachedCoursesService, IShortlistApiClient<ShortlistApiConfiguration> shortListApiClient)
        {
            _roatpCourseManagementApiClient = roatpCourseManagementApiClient;
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _employerFeedbackApiClient = employerFeedbackApiClient;
            _cachedCoursesService = cachedCoursesService;
            _shortListApiClient = shortListApiClient;

        }

        public async Task<GetShortlistForUserResult> Handle(GetShortlistForUserQuery request, CancellationToken cancellationToken)
        { 
            var shortlistForUserId = await _shortListApiClient.GetAll<ShortlistItem>(new GetShortlistForUserIdRequest(request.ShortlistUserId));
            if (shortlistForUserId == null || !shortlistForUserId.Any())
            {
                return new GetShortlistForUserResult
                {
                    Shortlist = new List<GetShortlistItem>()
                };
            }

            var providerDetailsTaskList = shortlistForUserId.Select(shortlistItem => new GetProviderByCourseAndUkprnRequest(shortlistItem.Ukprn, shortlistItem.Larscode, shortlistItem.Latitude, shortlistItem.Longitude))
                .Select(req => _roatpCourseManagementApiClient.Get<GetProviderDetailsForCourse>(req)).Cast<Task>().ToList();

            await Task.WhenAll(providerDetailsTaskList);

            var providerDetailsList = providerDetailsTaskList.Select(providerDetailsTask => ((Task<GetProviderDetailsForCourse>)providerDetailsTask).Result).ToList();

            var shortlist = new List<GetShortlistItem>();
            foreach (var sl in shortlistForUserId)
            {
                var shortlistItem = new GetShortlistItem
                {
                    Id = sl.Id,
                    ShortlistUserId = sl.ShortlistUserId,
                    CourseId = sl.Larscode,
                    LocationDescription = sl.LocationDescription,
                    CreatedDate = sl.CreatedDate
                };

                var providerDetails = providerDetailsList.First(x=>x.Ukprn==sl.Ukprn && x.LarsCode==sl.Larscode);

                var provider = new GetProviderStandardItem
                {
                    Ukprn = providerDetails.Ukprn,
                    Name = providerDetails.Name,
                    TradingName = providerDetails.TradingName,
                    MarketingInfo = providerDetails.MarketingInfo,
                    StandardInfoUrl = providerDetails.StandardInfoUrl,
                    Email = providerDetails.Email,
                    Phone = providerDetails.Phone,
                    StandardId = sl.Larscode,
                    ShortlistId = request.ShortlistUserId,
                    ProviderAddress = new GetProviderStandardItemAddress
                    {
                        Address1 = providerDetails.Address1,
                        Address2 = providerDetails.Address2,
                        Address3 = providerDetails.Address3,
                        Address4 = providerDetails.Address4,
                        Town = providerDetails.Town,
                        Postcode = providerDetails.Postcode,
                        DistanceInMiles = providerDetails.ProviderHeadOfficeDistanceInMiles ?? 0
                    },
                    AchievementRates = providerDetails.AchievementRates,
                    DeliveryModels = providerDetails.DeliveryModels
                };

                shortlistItem.ProviderDetails = provider;
                shortlist.Add(shortlistItem);
            }

            var coursesTask = _cachedCoursesService.GetCourses();
            var appFeedbackTask = _apprenticeFeedbackApiClient.GetAll<GetApprenticeFeedbackSummaryItem>(new GetApprenticeFeedbackSummaryRequest());
            var employerFeedbackTask = _employerFeedbackApiClient.GetAll<GetEmployerFeedbackSummaryItem>(new GetEmployerFeedbackSummaryRequest());

            await Task.WhenAll(coursesTask, appFeedbackTask, employerFeedbackTask);

            var appFeedbackResult = appFeedbackTask.Result ?? new List<GetApprenticeFeedbackSummaryItem>();
            var employerFeedbackResult = employerFeedbackTask.Result ?? new List<GetEmployerFeedbackSummaryItem>();

            foreach (var item in shortlist)
            {
                item.Course =
                    coursesTask.Result.Standards.FirstOrDefault(listItem =>
                        listItem.LarsCode == item.CourseId);

                item.ProviderDetails.ApprenticeFeedback = appFeedbackResult.FirstOrDefault(s => s.Ukprn == item.ProviderDetails.Ukprn);
                item.ProviderDetails.EmployerFeedback = employerFeedbackResult.FirstOrDefault(s => s.Ukprn == item.ProviderDetails.Ukprn);
            }

            return new GetShortlistForUserResult
            {
                Shortlist = shortlist
            };
        }
    }
}