using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistForUser
{
    public class GetShortlistForUserQueryHandler : IRequestHandler<GetShortlistForUserQuery, GetShortlistForUserResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient;
        private readonly IShortlistApiClient<ShortlistApiConfiguration> _shortListApiClient;
        private readonly ICachedCoursesService _cachedCoursesService;

        public GetShortlistForUserQueryHandler(
            ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient,
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> employerFeedbackApiClient,
            ICachedCoursesService cachedCoursesService, IShortlistApiClient<ShortlistApiConfiguration> shortListApiClient)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _employerFeedbackApiClient = employerFeedbackApiClient;
            _cachedCoursesService = cachedCoursesService;
            _shortListApiClient = shortListApiClient;
        }

        public async Task<GetShortlistForUserResult> Handle(GetShortlistForUserQuery request, CancellationToken cancellationToken)
        { 
            var shortlists = await _shortListApiClient.GetAll<ShortlistItem>(new GetShortlistForUserIdRequest(request.ShortlistUserId));

            var apiShortlistRequest = new GetShortlistForUserRequest(request.ShortlistUserId);
            var shortListTask = _courseDeliveryApiClient.Get<GetShortlistForUserResponse>(apiShortlistRequest);
            var coursesTask = _cachedCoursesService.GetCourses();
            var appFeedbackTask = _apprenticeFeedbackApiClient.GetAll<GetApprenticeFeedbackSummaryItem>(new GetApprenticeFeedbackSummaryRequest());
            var employerFeedbackTask = _employerFeedbackApiClient.GetAll<GetEmployerFeedbackSummaryItem>(new GetEmployerFeedbackSummaryRequest());

            await Task.WhenAll(shortListTask, coursesTask, appFeedbackTask, employerFeedbackTask);

            var shortlist = shortListTask.Result.Shortlist.ToList();
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