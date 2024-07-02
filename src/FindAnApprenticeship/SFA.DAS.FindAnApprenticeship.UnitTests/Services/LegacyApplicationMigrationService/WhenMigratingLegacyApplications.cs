using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses.Enums;
using SFA.DAS.FindAnApprenticeship.Services;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Services.LegacyApplicationMigrationService
{
    internal class WhenMigratingLegacyApplications
    {
        [Test, MoqAutoData]
        public async Task Then_Legacy_Applications_Are_Migrated(
            Guid candidateId,
            string candidateEmailAddress,
            GetLegacyApplicationsByEmailApiResponse legacyApplicationsResponse,
            GetLegacyUserByEmailApiResponse legacyUserByEmailApiResponse,
            PostApplicationApiResponse candidateApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
            [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> mockLegacyApiClient,
            FindAnApprenticeship.Services.LegacyApplicationMigrationService service)
        {
            legacyApplicationsResponse.Applications.ForEach(x => x.Status = ApplicationStatus.Draft);

            var legacyGetRequest = new GetLegacyUserByEmailApiRequest(candidateEmailAddress);
            mockLegacyApiClient
                .Setup(client => client.Get<GetLegacyUserByEmailApiResponse>(
                    It.Is<GetLegacyUserByEmailApiRequest>(r => r.GetUrl == legacyGetRequest.GetUrl)))
                .ReturnsAsync(legacyUserByEmailApiResponse);

            var legacyGetApplicationsRequest = new GetLegacyApplicationsByEmailApiRequest(candidateEmailAddress);
            mockLegacyApiClient
                .Setup(client => client.Get<GetLegacyApplicationsByEmailApiResponse>(
                    It.Is<GetLegacyApplicationsByEmailApiRequest>(r => r.GetUrl == legacyGetApplicationsRequest.GetUrl)))
                .ReturnsAsync(() => legacyApplicationsResponse);

            mockApiClient
                .Setup(client => client.PostWithResponseCode<PostApplicationApiResponse>(
                    It.IsAny<PostApplicationApiRequest>(), true))
                .ReturnsAsync(new ApiResponse<PostApplicationApiResponse>(candidateApiResponse, HttpStatusCode.OK, string.Empty));

            await service.MigrateLegacyApplications(candidateId, candidateEmailAddress);

            mockApiClient.Verify(
                x => x.PostWithResponseCode<PostApplicationApiResponse>(It.IsAny<PostApplicationApiRequest>(), true),
                Times.Exactly(legacyApplicationsResponse.Applications.Count));
        }

        [Test, MoqAutoData]
        public async Task Then_Saved_Vacancies_Are_Migrated(
            Guid candidateId,
            string candidateEmailAddress,
            GetLegacyApplicationsByEmailApiResponse legacyApplicationsResponse,
            GetLegacyUserByEmailApiResponse legacyUserByEmailApiResponse,
            PutSavedVacancyApiResponse candidateApiResponse,
            Mock<IVacancy> vacancy,
            [Frozen] Mock<IVacancyService> vacancyService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
            [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> mockLegacyApiClient,
            FindAnApprenticeship.Services.LegacyApplicationMigrationService service)
        {
            legacyApplicationsResponse.Applications.ForEach(x => { x.Status = ApplicationStatus.Saved; });
            
            var legacyGetRequest = new GetLegacyUserByEmailApiRequest(candidateEmailAddress);
            mockLegacyApiClient
                .Setup(client => client.Get<GetLegacyUserByEmailApiResponse>(
                    It.Is<GetLegacyUserByEmailApiRequest>(r => r.GetUrl == legacyGetRequest.GetUrl)))
                .ReturnsAsync(legacyUserByEmailApiResponse);

            var legacyGetApplicationsRequest = new GetLegacyApplicationsByEmailApiRequest(candidateEmailAddress);
            mockLegacyApiClient
                .Setup(client => client.Get<GetLegacyApplicationsByEmailApiResponse>(
                    It.Is<GetLegacyApplicationsByEmailApiRequest>(r => r.GetUrl == legacyGetApplicationsRequest.GetUrl)))
                .ReturnsAsync(() => legacyApplicationsResponse);

            vacancy.Setup(x => x.ClosingDate).Returns(DateTime.UtcNow.AddMonths(1));
            vacancyService.Setup(x => x.GetVacancy(It.IsAny<string>())).ReturnsAsync(vacancy.Object);

            mockApiClient
                .Setup(client => client.PutWithResponseCode<PutSavedVacancyApiResponse>(
                    It.IsAny<PutSavedVacancyApiRequest>()))
                .ReturnsAsync(new ApiResponse<PutSavedVacancyApiResponse>(candidateApiResponse, HttpStatusCode.OK, string.Empty));

            await service.MigrateLegacyApplications(candidateId, candidateEmailAddress);

            mockApiClient.Verify(
                x => x.PutWithResponseCode<PutSavedVacancyApiResponse>(It.IsAny<PutSavedVacancyApiRequest>()),
                Times.Exactly(legacyApplicationsResponse.Applications.Count));
        }
    }
}
