using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference
{
    public class GetApprenticeshipVacancyQueryHandler : IRequestHandler<GetApprenticeshipVacancyQuery, GetApprenticeshipVacancyQueryResult>
    {
        private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ICourseService _courseService;

        public GetApprenticeshipVacancyQueryHandler(
            IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ICourseService courseService)
        {
            _findApprenticeshipApiClient = findApprenticeshipApiClient;
            _coursesApiClient = coursesApiClient;
            _courseService = courseService;
        }

        public async Task<GetApprenticeshipVacancyQueryResult> Handle(GetApprenticeshipVacancyQuery request, CancellationToken cancellationToken)
        {
            var result = await _findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(new GetVacancyRequest(request.VacancyReference));

            if (result is null) return null;

            var courseResult = await _coursesApiClient.Get<GetStandardsListItemResponse>(new GetStandardRequest(result.CourseId));
            var courseLevels = await _courseService.GetLevels();

            return new GetApprenticeshipVacancyQueryResult
            {
                ApprenticeshipVacancy = result,
                CourseDetail = courseResult,
                Levels = courseLevels.Levels.ToList()
            };
        }
    }
}