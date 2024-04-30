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

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference
{
    public class GetApprenticeshipVacancyQueryHandler(
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
        ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
        ICourseService courseService,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        : IRequestHandler<GetApprenticeshipVacancyQuery, GetApprenticeshipVacancyQueryResult>
    {
        public async Task<GetApprenticeshipVacancyQueryResult> Handle(GetApprenticeshipVacancyQuery request, CancellationToken cancellationToken)
        {
            var result = await findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(new GetVacancyRequest(request.VacancyReference));

            if (result is null) return null;

            var courseResult = await coursesApiClient.Get<GetStandardsListItemResponse>(new GetStandardRequest(result.CourseId));
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
                ApprenticeshipVacancy = result,
                CourseDetail = courseResult,
                Levels = courseLevels.Levels.ToList(),
                Application = candidateApplicationDetails
            };
        }
    }
}