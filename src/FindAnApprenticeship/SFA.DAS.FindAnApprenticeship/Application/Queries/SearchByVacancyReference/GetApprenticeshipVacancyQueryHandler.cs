using System;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference
{
    public class GetApprenticeshipVacancyQueryHandler(
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
        IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
        ICourseService courseService)
        : IRequestHandler<GetApprenticeshipVacancyQuery, GetApprenticeshipVacancyQueryResult>
    {
        public async Task<GetApprenticeshipVacancyQueryResult> Handle(GetApprenticeshipVacancyQuery request, CancellationToken cancellationToken)
        {
            var result = await findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(new GetVacancyRequest(request.VacancyReference));
            GetClosedVacancyResponse closedVacancy = null;

            if (result is null)
            {
                closedVacancy = await recruitApiClient.Get<GetClosedVacancyResponse>(new GetClosedVacancyRequest(request.VacancyReference));

                if (closedVacancy is null) return null;
            }

            var courseId = result?.CourseId ?? Convert.ToInt32(closedVacancy.ProgrammeId);

            var courseResult = await coursesApiClient.Get<GetStandardsListItemResponse>(new GetStandardRequest(courseId));
            var courseLevels = await courseService.GetLevels();

            return new GetApprenticeshipVacancyQueryResult
            {
                ApprenticeshipVacancy = result != null ? result : closedVacancy,
                CourseDetail = courseResult,
                Levels = courseLevels.Levels.ToList()
            };
        }
    }
}