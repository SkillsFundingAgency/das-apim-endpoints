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
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.SharedOuterApi.Extensions;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference
{
    public class GetApprenticeshipVacancyQueryHandler(
        ILogger<GetApprenticeshipVacancyQueryHandler> logger,
        ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
        ICourseService courseService,
        IVacancyService vacancyService,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        : IRequestHandler<GetApprenticeshipVacancyQuery, GetApprenticeshipVacancyQueryResult>
    {
        public async Task<GetApprenticeshipVacancyQueryResult> Handle(GetApprenticeshipVacancyQuery request, CancellationToken cancellationToken)
        {
            var vacancy = await vacancyService.GetVacancy(request.VacancyReference) 
                          ?? await vacancyService.GetClosedVacancy(request.VacancyReference);

            if (vacancy == null)
            {
                return null;
            }

            if (vacancy.VacancySource == VacancyDataSource.Nhs)
            {
                return new GetApprenticeshipVacancyQueryResult
                {
                    ApprenticeshipVacancy = GetApprenticeshipVacancyQueryResult.Vacancy.FromIVacancy(vacancy),
                    CourseDetail = null,
                    Levels = null,
                    Application = null,
                    CandidatePostcode = null,
                    IsSavedVacancy = false
                };
            }

            if (vacancy.CourseId <= 0)
            {
                logger.LogWarning("Vacancy '{vacancyReference}' has an unknown course", vacancy.VacancyReference);
            }
            
            var courseResult = await coursesApiClient.Get<GetStandardsListItemResponse>(new GetStandardRequest(vacancy.CourseId));
            var courseLevels = await courseService.GetLevels();

            GetApprenticeshipVacancyQueryResult.CandidateApplication candidateApplicationDetails = null;
            string candidatePostcode = null;
            DateTime? candidateDateOfBirth = null;
            var isSavedVacancy = false;

            if (request.CandidateId.HasValue)
            {
                var vacancyReference =
                    request.VacancyReference.TrimVacancyReference();

                var application = candidateApiClient.Get<GetApplicationByReferenceApiResponse>(
                    new GetApplicationByReferenceApiRequest(request.CandidateId.Value, vacancyReference));

                var candidate = candidateApiClient.Get<GetCandidateApiResponse>(
                    new GetCandidateApiRequest(request.CandidateId.Value.ToString()));

                var savedVacancy = candidateApiClient.Get<GetSavedVacancyApiResponse>(
                    new GetSavedVacancyApiRequest(request.CandidateId.Value, null, vacancyReference));

                await Task.WhenAll(application, candidate, savedVacancy);

                if (application.Result != null)
                {
                    candidateApplicationDetails = new GetApprenticeshipVacancyQueryResult.CandidateApplication
                    {
                        Status = application.Result.Status,
                        SubmittedDate = application.Result.SubmittedDate,
                        WithdrawnDate = application.Result.WithdrawnDate,
                        ApplicationId = application.Result.Id
                    };
                }

                if (candidate.Result != null)
                {
                    candidatePostcode = candidate.Result.Address.Postcode;
                    candidateDateOfBirth = candidate.Result.DateOfBirth;
                }

                if (savedVacancy.Result != null)
                {
                    isSavedVacancy = true;
                }
            }

            return new GetApprenticeshipVacancyQueryResult
            {
                ApprenticeshipVacancy = GetApprenticeshipVacancyQueryResult.Vacancy.FromIVacancy(vacancy, courseResult),
                CourseDetail = courseResult,
                Levels = courseLevels.Levels.ToList(),
                Application = candidateApplicationDetails,
                CandidatePostcode = candidatePostcode,
                CandidateDateOfBirth = candidateDateOfBirth,
                IsSavedVacancy = isSavedVacancy
            };
        }
    }
}