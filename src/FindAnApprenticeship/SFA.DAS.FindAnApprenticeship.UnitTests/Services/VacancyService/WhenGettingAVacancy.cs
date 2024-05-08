using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Services.VacancyService
{
    [TestFixture]
    public class WhenGettingAVacancy
    {
        [Test, MoqAutoData]
        public async Task Then_The_Vacancy_Is_Returned_From_FindApprenticeshipApi(
            string vacancyReference,
            GetApprenticeshipVacancyItemResponse apiResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            FindAnApprenticeship.Services.VacancyService service)
        {
            // Arrange
            var expectedRequest = new GetVacancyRequest(vacancyReference);

            apiClient
                .Setup(client =>
                    client.Get<GetApprenticeshipVacancyItemResponse>(
                        It.Is<GetVacancyRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            // Act
            var result = await service.GetVacancy(vacancyReference);

            // Assert
            result.Should().BeOfType<GetApprenticeshipVacancyItemResponse>();
            result.Should().BeEquivalentTo(apiResponse);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Vacancy_Is_Returned_From_Recruit_If_Not_Found_In_FindApprenticeshipApi(
            long vacancyReference,
            GetClosedVacancyResponse closedVacancyResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            FindAnApprenticeship.Services.VacancyService service)
        {
            // Arrange
            var expectedRequest = new GetVacancyRequest(vacancyReference.ToString());

            apiClient
                .Setup(client =>
                    client.Get<GetApprenticeshipVacancyItemResponse>(
                        It.Is<GetVacancyRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(() => null);

            var expectedRecruitApiRequest = new GetClosedVacancyRequest(vacancyReference.ToString());
            recruitApiClient.Setup(x =>
                    x.Get<GetClosedVacancyResponse>(
                        It.Is<GetClosedVacancyRequest>(x => x.GetUrl == expectedRecruitApiRequest.GetUrl)))
                .ReturnsAsync(closedVacancyResponse);

            // Act
            var result = await service.GetVacancy($"VAC{vacancyReference}");

            // Assert
            result.Should().BeOfType<GetClosedVacancyResponse>();
            result.Should().BeEquivalentTo(closedVacancyResponse);
        }
    }
}
