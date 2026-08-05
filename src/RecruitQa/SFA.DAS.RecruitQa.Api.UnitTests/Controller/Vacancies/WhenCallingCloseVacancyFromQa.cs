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

public class WhenCallingCloseVacancyFromQa
{
    [Test, MoqAutoData]
    public async Task Then_Patch_Is_Called_With_The_Vacancy_Id_And_Ok_Is_Returned(
        Guid id,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        var request = new CloseVacancyRequest { ClosureReason = "WithdrawnByQa" };
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        var result = await sut.CloseVacancyFromQa(id, recruitApiClient.Object, request, CancellationToken.None) as Ok;

        result.Should().NotBeNull();
        recruitApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchVacanciesByVacancyIdApiRequest>(
            r => r.VacancyId == id)), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_Patch_Is_Called_With_Status_Closed_And_The_Closure_Reason(
        Guid id,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        var request = new CloseVacancyRequest { ClosureReason = "WithdrawnByQa" };
        PatchVacanciesByVacancyIdApiRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<Vacancy>>>(x => capturedRequest = x as PatchVacanciesByVacancyIdApiRequest)
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, null!));

        await sut.CloseVacancyFromQa(id, recruitApiClient.Object, request, CancellationToken.None);

        capturedRequest.Should().NotBeNull();
        capturedRequest!.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/status", null, VacancyStatus.Closed));
        capturedRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/closureReason", null, ClosureReason.WithdrawnByQa));
    }
}
