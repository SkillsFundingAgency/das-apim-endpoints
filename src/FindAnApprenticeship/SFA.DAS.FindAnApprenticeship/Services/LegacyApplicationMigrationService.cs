using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Extensions.LegacyApi;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Enums;
using SFA.DAS.SharedOuterApi.Extensions;
using static SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests.PutSavedVacancyApiRequest;

namespace SFA.DAS.FindAnApprenticeship.Services
{
    public interface ILegacyApplicationMigrationService
    {
        Task<GetLegacyApplicationsByEmailApiResponse> GetLegacyApplications(string emailAddress);
        Task MigrateLegacyApplications(Guid candidateId, string emailAddress);
    }

    public class LegacyApplicationMigrationService(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
        IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration> legacyApiClient, IVacancyService vacancyService, ILogger<LegacyApplicationMigrationService> logger) : ILegacyApplicationMigrationService
    {
        private static readonly List<ApplicationStatus> LegacyImportStatuses =
        [
            ApplicationStatus.Draft,
            ApplicationStatus.Submitted,
            ApplicationStatus.Successful,
            ApplicationStatus.Unsuccessful
        ];

        public async Task<GetLegacyApplicationsByEmailApiResponse> GetLegacyApplications(string emailAddress)
        {
            logger.LogInformation("Fetching applications for candidate [using email address [{emailAddress}].", emailAddress);

            var legacyApplications =
                await legacyApiClient.Get<GetLegacyApplicationsByEmailApiResponse>(
                    new GetLegacyApplicationsByEmailApiRequest(emailAddress));

            if (legacyApplications?.Applications == null || legacyApplications.Applications.Count == 0)
            {
                logger.LogInformation("No legacy applications found for email address [{emailAddress}].", emailAddress);
                return new GetLegacyApplicationsByEmailApiResponse();
            }

            return legacyApplications;
        }

        public async Task MigrateLegacyApplications(Guid candidateId, string emailAddress)
        {
            logger.LogInformation($"Migrating applications for candidate [{candidateId}] using email address [{emailAddress}].");

            var legacyApplications =
                await legacyApiClient.Get<GetLegacyApplicationsByEmailApiResponse>(
                    new GetLegacyApplicationsByEmailApiRequest(emailAddress));

            if (legacyApplications?.Applications == null || legacyApplications.Applications.Count == 0)
            {
                logger.LogInformation($"No legacy applications found for email address [{emailAddress}].");
                return;
            }

            foreach (var legacyApplication in legacyApplications.Applications.Where(x => LegacyImportStatuses.Contains(x.Status)))
            {
                var vacancy = await vacancyService.GetVacancy(legacyApplication.Vacancy.VacancyReference);

                if (vacancy == null)
                {
                    logger.LogError($"Unable to retrieve vacancy [{legacyApplication.Vacancy.VacancyReference}].");
                    continue;
                }

                var data = new PostApplicationApiRequest.PostApplicationApiRequestData
                {
                    LegacyApplication = Map(legacyApplication, vacancy, candidateId)
                };
                var postRequest = new PostApplicationApiRequest(data);

                var applicationResult =
                    await candidateApiClient.PostWithResponseCode<PostApplicationApiResponse>(postRequest);

                applicationResult.EnsureSuccessStatusCode();
            }

            foreach (var legacyApplication in legacyApplications.Applications.Where(x => x.Status == ApplicationStatus.Saved))
            {
                var vacancy = await vacancyService.GetVacancy(legacyApplication.Vacancy.VacancyReference);

                if (vacancy == null)
                {
                    logger.LogError($"Unable to retrieve vacancy [{legacyApplication.Vacancy.VacancyReference}].");
                    continue;
                }

                if (vacancy.ClosingDate < DateTime.UtcNow)
                {
                    logger.LogWarning($"Ignoring saved vacancy reference [{legacyApplication.Vacancy.VacancyReference}] as closing date has passed.");
                    continue;
                }

                var vacancyReference = legacyApplication.Vacancy.VacancyReference.Replace("VAC", "", StringComparison.CurrentCultureIgnoreCase);
                var data = new PostSavedVacancyApiRequestData
                {

                    VacancyReference =  vacancyReference,
                    CreatedOn = legacyApplication.DateCreated ?? DateTime.UtcNow
                };
                var postRequest = new PutSavedVacancyApiRequest(candidateId, data);

                var applicationResult =
                    await candidateApiClient.PutWithResponseCode<PutSavedVacancyApiResponse>(postRequest);

                applicationResult.EnsureSuccessStatusCode();
            }
        }

        public static PostApplicationApiRequest.LegacyApplication Map(GetLegacyApplicationsByEmailApiResponse.Application source,
            IVacancy vacancy, Guid candidateId)
        {
            return new PostApplicationApiRequest.LegacyApplication
            {
                Id = source.Id,
                CandidateId = candidateId,
                VacancyReference =
                    source.Vacancy.VacancyReference.Replace("VAC", "",
                        StringComparison.CurrentCultureIgnoreCase),
                Status = source.Status.ToFaaApplicationStatus(),
                DateApplied = source.DateApplied,
                SuccessfulDateTime = source.SuccessfulDateTime,
                UnsuccessfulDateTime = source.UnsuccessfulDateTime,
                UnsuccessfulReason = source.UnsuccessfulReason,
                SkillsAndStrengths = source.CandidateInformation.AboutYou.Strengths,
                Support = source.CandidateInformation.AboutYou.Support,
                AdditionalQuestion1Answer = source.AdditionalQuestion1Answer,
                AdditionalQuestion2Answer = source.AdditionalQuestion2Answer,
                AdditionalQuestion1 = vacancy.AdditionalQuestion1,
                AdditionalQuestion2 = vacancy.AdditionalQuestion2,
                IsDisabilityConfident = vacancy.IsDisabilityConfident,
                Qualifications = source.CandidateInformation.Qualifications.Select(x => new PostApplicationApiRequest.LegacyApplication.Qualification
                {
                    Grade = x.Grade,
                    IsPredicted = x.IsPredicted,
                    QualificationType = x.QualificationType,
                    Subject = x.Subject,
                    Year = x.Year
                }).ToList(),
                TrainingCourses = source.CandidateInformation.TrainingCourses.Select(x => new PostApplicationApiRequest.LegacyApplication.TrainingCourse
                {
                    FromDate = x.FromDate,
                    Provider = x.Provider,
                    Title = x.Title,
                    ToDate = x.ToDate
                }).ToList(),
                WorkExperience = source.CandidateInformation.WorkExperiences.Select(x => new PostApplicationApiRequest.LegacyApplication.WorkExperienceItem
                {
                    Description = x.Description,
                    Employer = x.Employer,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate,
                    JobTitle = x.JobTitle
                }).ToList()
            };
        }
    }
}
