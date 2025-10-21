using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using static SFA.DAS.FindAnApprenticeship.InnerApi.Responses.PostGetVacanciesByReferenceApiResponse;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Services.VacancyService
{
    [TestFixture]
    public class WhenGettingMultipleVacancies
    {
        [Test, MoqAutoData]
        public async Task Then_The_Vacancies_Are_Returned_From_FindApprenticeshipApi(
            List<long> vacancyReferences,
            PostGetVacanciesByReferenceApiResponse apiResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            FindAnApprenticeship.Services.VacancyService service)
        {
            // Arrange
            var expectedRequest = new PostGetVacanciesByReferenceApiRequest(new PostGetVacanciesByReferenceApiRequestBody
            {
                VacancyReferences = vacancyReferences.Select(x => x.ToString()).ToList()
            });

            for (var i = 0; i < vacancyReferences.Count; i++)
            {
                apiResponse.ApprenticeshipVacancies.ToArray()[i].VacancyReference = $"VAC{vacancyReferences[i]}";
            }

            apiClient
                .Setup(client =>
                    client.PostWithResponseCode<PostGetVacanciesByReferenceApiResponse>(
                        It.Is<PostGetVacanciesByReferenceApiRequest>(r => r.PostUrl == expectedRequest.PostUrl), true))
                .ReturnsAsync(new ApiResponse<PostGetVacanciesByReferenceApiResponse>(apiResponse, HttpStatusCode.OK, ""));

            // Act
            var result = await service.GetVacancies(vacancyReferences.Select(x => x.ToString()).ToList());

            // Assert
            result.Should().BeOfType<List<IVacancy>>();
            result.Should().AllBeAssignableTo<ApprenticeshipVacancy>();
            result.Should().BeEquivalentTo(apiResponse.ApprenticeshipVacancies);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Vacancies_Are_Returned_From_Recruit_If_Not_Found_In_FindApprenticeshipApi(
            List<long> vacancyReferences,
            List<GetClosedVacancyResponse> closedVacancyResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitApiClient,
            FindAnApprenticeship.Services.VacancyService service)
        {
            // Arrange
            var emptyResponse = new PostGetVacanciesByReferenceApiResponse { ApprenticeshipVacancies = new List<PostGetVacanciesByReferenceApiResponse.ApprenticeshipVacancy>()};
            apiClient
                .Setup(client =>
                    client.PostWithResponseCode<PostGetVacanciesByReferenceApiResponse>(
                        It.IsAny<PostGetVacanciesByReferenceApiRequest>(), true))
                .ReturnsAsync(new ApiResponse<PostGetVacanciesByReferenceApiResponse>(emptyResponse, HttpStatusCode.OK, ""));

            var expectedRecruitApiRequest = new PostGetClosedVacanciesByReferenceApiRequest(new PostGetClosedVacanciesByReferenceApiRequestBody
            {
                VacancyReferences = vacancyReferences
            });

            recruitApiClient.Setup(x =>
                    x.PostWithResponseCode<List<GetClosedVacancyResponse>>(
                        It.Is<PostGetClosedVacanciesByReferenceApiRequest>(x => x.PostUrl == expectedRecruitApiRequest.PostUrl), true))
                .ReturnsAsync(new ApiResponse<List<GetClosedVacancyResponse>>(closedVacancyResponse, HttpStatusCode.OK, ""));

            // Act
            var result = await service.GetVacancies(vacancyReferences.Select(x => x.ToString()).ToList());

            // Assert
            result.Should().BeOfType<List<IVacancy>>();
            result.Should().AllBeAssignableTo<GetClosedVacancyResponse>();
            result.Should().BeEquivalentTo(closedVacancyResponse);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Vacancies_Not_Found_From_FindApprenticeshipApi(
            List<long> vacancyReferences,
            PostGetVacanciesByReferenceApiResponse apiResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            FindAnApprenticeship.Services.VacancyService service)
        {
            // Arrange
            apiClient
                .Setup(client =>
                    client.PostWithResponseCode<PostGetVacanciesByReferenceApiResponse>(
                        It.IsAny<PostGetVacanciesByReferenceApiRequest>(), true))
                .ReturnsAsync(new ApiResponse<PostGetVacanciesByReferenceApiResponse>((PostGetVacanciesByReferenceApiResponse)null!, HttpStatusCode.OK, ""));

            // Act
            var result = await service.GetVacancies(vacancyReferences.Select(x => x.ToString()).ToList());

            // Assert
            result.Should().AllBeAssignableTo<GetClosedVacancyResponse>();
            result.Count.Should().Be(0);
        }
    }
}
