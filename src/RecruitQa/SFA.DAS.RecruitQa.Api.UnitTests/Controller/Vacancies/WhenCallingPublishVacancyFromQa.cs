using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.Vacancies;

public class WhenCallingPublishVacancyFromQa
{
    [Test, MoqAutoData]
    public async Task Then_Patch_Is_Called_With_The_Vacancy_Id_And_Ok_Is_Returned(
        Guid id,
        Mock<IRecruitApiClient<RecruitAiApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        recruitApiClient
            .Setup(x => x.Patch(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .Returns(Task.CompletedTask);

        var result = await sut.PublishVacancyFromQa(id, recruitApiClient.Object, CancellationToken.None) as Ok;

        result.Should().NotBeNull();
        recruitApiClient.Verify(x => x.Patch(It.Is<PatchVacanciesByVacancyIdApiRequest>(
            r => r.VacancyId == id)), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Patch_Is_Called_With_Status_Live(
        Guid id,
        Mock<IRecruitApiClient<RecruitAiApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        recruitApiClient
            .Setup(x => x.Patch(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .Returns(Task.CompletedTask);

        await sut.PublishVacancyFromQa(id, recruitApiClient.Object, CancellationToken.None);

        recruitApiClient.Verify(x => x.Patch(It.Is<PatchVacanciesByVacancyIdApiRequest>(
            r => r.Data.Status == VacancyStatus.Live
        )), Times.Once);
    }
}
