using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Net;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.Vacancies;

public class WhenCallingPublishVacancyFromQa
{
    [Test, MoqAutoData]
    public async Task Then_Patch_Is_Called_With_The_Vacancy_Id_And_Ok_Is_Returned(
        Guid id,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        var result = await sut.PublishVacancyFromQa(id, recruitApiClient.Object, CancellationToken.None) as Ok;

        result.Should().NotBeNull();
        recruitApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchVacanciesByVacancyIdApiRequest>(
            r => r.VacancyId == id)), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Patch_Is_Called_With_Status_Live(
        Guid id,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        PatchVacanciesByVacancyIdApiRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<Vacancy>>>(x => capturedRequest = x as PatchVacanciesByVacancyIdApiRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        await sut.PublishVacancyFromQa(id, recruitApiClient.Object, CancellationToken.None);

        capturedRequest.Should().NotBeNull();
        capturedRequest!.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/status", null, VacancyStatus.Live));
    }
}
