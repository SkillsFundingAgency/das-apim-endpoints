using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.Vacancies;

public class WhenCallingUpdateVacancyFromQaEdit
{
    [Test, MoqAutoData]
    public async Task Then_Patch_Is_Called_With_The_Vacancy_Id_And_Ok_Is_Returned(
        Guid id,
        UpdateVacancyRequest request,
        Mock<IRecruitApiClient<RecruitAiApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        request.Status = "Live";
        recruitApiClient
            .Setup(x => x.Patch(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .Returns(Task.CompletedTask);

        var result = await sut.UpdateVacancyFromQaEdit(id, recruitApiClient.Object, request, CancellationToken.None) as Ok;

        result.Should().NotBeNull();
        recruitApiClient.Verify(x => x.Patch(It.Is<PatchVacanciesByVacancyIdApiRequest>(
            r => r.VacancyId == id)), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Patch_Is_Called_With_The_Correct_Data(
        Guid id,
        UpdateVacancyRequest request,
        Mock<IRecruitApiClient<RecruitAiApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        request.Status = "Live";
        recruitApiClient
            .Setup(x => x.Patch(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .Returns(Task.CompletedTask);

        await sut.UpdateVacancyFromQaEdit(id, recruitApiClient.Object, request, CancellationToken.None);

        recruitApiClient.Verify(x => x.Patch(It.Is<PatchVacanciesByVacancyIdApiRequest>(r =>
            r.Data.OutcomeDescription == request.OutcomeDescription
            && r.Data.TrainingDescription == request.TrainingDescription
            && r.Data.AdditionalTrainingDescription == request.AdditionalTrainingDescription
            && r.Data.ShortDescription == request.ShortDescription
            && r.Data.Description == request.Description
            && r.Data.Wage.WorkingWeekDescription == request.WorkingWeekDescription
            && r.Data.Wage.CompanyBenefitsInformation == request.CompanyBenefitsInformation
            && r.Data.ThingsToConsider == request.ThingsToConsider
            && r.Data.ApplicationInstructions == request.ApplicationInstructions
            && r.Data.EmployerLocationInformation == request.EmployerLocationInformation
        )), Times.Once);
    }
}
