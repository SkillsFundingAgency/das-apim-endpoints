using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Services.VacancyService
{
    [TestFixture]
    public class WhenGettingAClosedVacancy
    {
        [Test, MoqAutoData]
        public async Task Then_The_ClosedVacancy_Is_Returned_From_Recruit(
            long vacancyReference,
            GetClosedVacancyResponse closedVacancyResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitApiClient,
            FindAnApprenticeship.Services.VacancyService service)
        {
            // Arrange
            var expectedRecruitApiRequest = new GetClosedVacancyRequest(vacancyReference.ToString());
            recruitApiClient.Setup(x =>
                    x.Get<GetClosedVacancyResponse>(
                        It.Is<GetClosedVacancyRequest>(closedVacancyRequest => closedVacancyRequest.GetUrl == expectedRecruitApiRequest.GetUrl)))
                .ReturnsAsync(closedVacancyResponse);

            // Act
            var result = await service.GetClosedVacancy($"VAC{vacancyReference}");

            // Assert
            result.Should().BeOfType<GetClosedVacancyResponse>();
            result.Should().BeEquivalentTo(closedVacancyResponse);
        }
    }
}