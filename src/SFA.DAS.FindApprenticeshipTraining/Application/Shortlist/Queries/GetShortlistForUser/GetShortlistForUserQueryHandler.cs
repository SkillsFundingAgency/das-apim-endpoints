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
        private readonly ICachedCoursesService _cachedCoursesService;

        public GetShortlistForUserQueryHandler(
            ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient, 
            ICachedCoursesService cachedCoursesService)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _cachedCoursesService = cachedCoursesService;
        }

        public async Task<GetShortlistForUserResult> Handle(GetShortlistForUserQuery request, CancellationToken cancellationToken)
        {
            var apiShortlistRequest = new GetShortlistForUserRequest(request.ShortlistUserId);
            var shortListTask = _courseDeliveryApiClient.Get<GetShortlistForUserResponse>(apiShortlistRequest);
            var coursesTask = _cachedCoursesService.GetCourses();

            await Task.WhenAll(shortListTask, coursesTask);

            var shortlist = shortListTask.Result.Shortlist.ToList();
            foreach (var item in shortlist)
            {
                item.Course =
                    coursesTask.Result.Standards.FirstOrDefault(listItem =>
                        listItem.LarsCode == item.CourseId);
            }

            return new GetShortlistForUserResult
            {
                Shortlist = shortlist
            };
        }
    }
}