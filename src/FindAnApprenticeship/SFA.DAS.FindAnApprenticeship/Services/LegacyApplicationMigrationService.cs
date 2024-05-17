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
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Enums;
using SFA.DAS.SharedOuterApi.Extensions;

namespace SFA.DAS.FindAnApprenticeship.Services
{
    public interface ILegacyApplicationMigrationService
    {
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
                    LegacyApplication = PostApplicationApiRequest.LegacyApplication.Map(legacyApplication, vacancy, candidateId)
                };
                var postRequest = new PostApplicationApiRequest(data);

                var applicationResult =
                    await candidateApiClient.PostWithResponseCode<PostApplicationApiResponse>(postRequest);

                applicationResult.EnsureSuccessStatusCode();
            }
        }
    }
}
