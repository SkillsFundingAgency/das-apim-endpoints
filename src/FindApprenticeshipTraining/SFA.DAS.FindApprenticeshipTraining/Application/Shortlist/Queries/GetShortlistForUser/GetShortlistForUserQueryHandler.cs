using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Queries.GetShortlistForUser
{
    public class GetShortlistForUserQueryHandler : IRequestHandler<GetShortlistForUserQuery, GetShortlistForUserResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly ICachedCoursesService _cachedCoursesService;

        public GetShortlistForUserQueryHandler(
            ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient,
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            ICachedCoursesService cachedCoursesService)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _cachedCoursesService = cachedCoursesService;
        }

        public async Task<GetShortlistForUserResult> Handle(GetShortlistForUserQuery request, CancellationToken cancellationToken)
        {
            var apiShortlistRequest = new GetShortlistForUserRequest(request.ShortlistUserId);
            var shortListTask = _courseDeliveryApiClient.Get<GetShortlistForUserResponse>(apiShortlistRequest);
            var coursesTask = _cachedCoursesService.GetCourses();

            await Task.WhenAll(shortListTask, coursesTask);

            var apprenticeFeedbackTasks = new List<Task<GetApprenticeFeedbackResponse>>();
            var shortlist = shortListTask.Result.Shortlist.ToList();

            var ukprns = shortlist.Select(s => s.ProviderDetails.Ukprn);
            
            var apprenticeFeedbackRatings =
                await _apprenticeFeedbackApiClient.
                PostWithResponseCode<IEnumerable<GetApprenticeFeedbackResponse>>(
                    new PostApprenticeFeedbackRatingRequest
                    {
                        Data = new PostApprenticeFeedbackRatingRequestData { Ukprns = ukprns }
                    });

            foreach (var item in shortlist)
            {
                item.Course =
                    coursesTask.Result.Standards.FirstOrDefault(listItem =>
                        listItem.LarsCode == item.CourseId);

                item.ProviderDetails.ApprenticeFeedback = apprenticeFeedbackRatings.Body.FirstOrDefault(s => s.Ukprn == item.ProviderDetails.Ukprn);
            }

            return new GetShortlistForUserResult
            {
                Shortlist = shortlist
            };
        }
    }
}