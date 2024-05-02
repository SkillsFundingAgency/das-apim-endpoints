using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
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
        ICourseService courseService,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
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

            GetApprenticeshipVacancyQueryResult.CandidateApplication candidateApplicationDetails = null;

            if (!string.IsNullOrEmpty(request.CandidateId))
            {
                var vacancyReference =
                    request.VacancyReference.Replace("VAC", "", StringComparison.CurrentCultureIgnoreCase);

                var application = await candidateApiClient.Get<GetApplicationByReferenceApiResponse>(
                    new GetApplicationByReferenceApiRequest(Guid.Parse(request.CandidateId), vacancyReference));

                if (application != null)
                {
                    candidateApplicationDetails = new GetApprenticeshipVacancyQueryResult.CandidateApplication
                    {
                        Status = application.Status,
                        SubmittedDate = application.SubmittedDate,
                    };
                }
            }
            return new GetApprenticeshipVacancyQueryResult
            {
                ApprenticeshipVacancy = result != null ? result : closedVacancy,
                CourseDetail = courseResult,
                Levels = courseLevels.Levels.ToList(),
                Application = candidateApplicationDetails
            };
        }
    }
}