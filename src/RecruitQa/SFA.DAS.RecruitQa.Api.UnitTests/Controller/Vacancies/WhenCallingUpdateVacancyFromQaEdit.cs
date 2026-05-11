using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.Vacancies;

public class WhenCallingUpdateVacancyFromQaEdit
{
    [Test, MoqAutoData]
    public async Task Then_Patch_Is_Called_With_The_Vacancy_Id_And_Ok_Is_Returned(
        Guid id,
        UpdateVacancyRequest request,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        request.Status = "Live";
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        var result = await sut.UpdateVacancyFromQaEdit(id, recruitApiClient.Object, request, CancellationToken.None) as Ok;

        result.Should().NotBeNull();
        recruitApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchVacanciesByVacancyIdApiRequest>(
            r => r.VacancyId == id)), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Patch_Is_Called_With_The_Correct_Data(
        Guid id,
        UpdateVacancyRequest request,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        request.Status = "Live";
        PatchVacanciesByVacancyIdApiRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<Vacancy>>>(x => capturedRequest = x as PatchVacanciesByVacancyIdApiRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        await sut.UpdateVacancyFromQaEdit(id, recruitApiClient.Object, request, CancellationToken.None);

        capturedRequest.Should().NotBeNull();
        capturedRequest!.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/outcomeDescription", null, request.OutcomeDescription));
        capturedRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/trainingDescription", null, request.TrainingDescription));
        capturedRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/additionalTrainingDescription", null, request.AdditionalTrainingDescription));
        capturedRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/shortDescription", null, request.ShortDescription));
        capturedRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/description", null, request.Description));
        capturedRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/wage/workingWeekDescription", null, request.WorkingWeekDescription));
        capturedRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/wage/companyBenefitsInformation", null, request.CompanyBenefitsInformation));
        capturedRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/thingsToConsider", null, request.ThingsToConsider));
        capturedRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/applicationInstructions", null, request.ApplicationInstructions));
        capturedRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/employerLocationInformation", null, request.EmployerLocationInformation));
    }
}
