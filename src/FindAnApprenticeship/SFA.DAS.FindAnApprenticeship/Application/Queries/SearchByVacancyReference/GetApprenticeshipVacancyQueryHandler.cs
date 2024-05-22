using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference
{
    public class GetApprenticeshipVacancyQueryHandler(
        ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
        ICourseService courseService,
        IVacancyService vacancyService,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        : IRequestHandler<GetApprenticeshipVacancyQuery, GetApprenticeshipVacancyQueryResult>
    {
        public async Task<GetApprenticeshipVacancyQueryResult> Handle(GetApprenticeshipVacancyQuery request, CancellationToken cancellationToken)
        {
            var vacancy = await vacancyService.GetVacancy(request.VacancyReference);

            if (vacancy == null) { return null; }

            var courseResult = await coursesApiClient.Get<GetStandardsListItemResponse>(new GetStandardRequest(vacancy.CourseId));
            var courseLevels = await courseService.GetLevels();

            GetApprenticeshipVacancyQueryResult.CandidateApplication candidateApplicationDetails = null;

            if (request.CandidateId.HasValue)
            {
                var vacancyReference =
                    request.VacancyReference.Replace("VAC", "", StringComparison.CurrentCultureIgnoreCase);

                var application = await candidateApiClient.Get<GetApplicationByReferenceApiResponse>(
                    new GetApplicationByReferenceApiRequest(request.CandidateId.Value, vacancyReference));

                if (application != null)
                {
                    candidateApplicationDetails = new GetApprenticeshipVacancyQueryResult.CandidateApplication
                    {
                        Status = application.Status,
                        SubmittedDate = application.SubmittedDate,
                        WithdrawnDate = application.WithdrawnDate,
                        ApplicationId = application.Id
                    };
                }
            }

            return new GetApprenticeshipVacancyQueryResult
            {
                ApprenticeshipVacancy = GetApprenticeshipVacancyQueryResult.Vacancy.FromIVacancy(vacancy),
                CourseDetail = courseResult,
                Levels = courseLevels.Levels.ToList(),
                Application = candidateApplicationDetails
            };
        }
    }
}