using System.Collections.Generic;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.RecruitJobs.Api.Controllers;
using SFA.DAS.RecruitJobs.Api.Models.Requests;
using SFA.DAS.SharedOuterApi.Recruit.GraphQL;
using StrawberryShake;
using ClosureReason = SFA.DAS.Recruit.Contracts.ApiResponses.ClosureReason;
using VacancyStatus = SFA.DAS.Recruit.Contracts.ApiResponses.VacancyStatus;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.VacancyControllerTests;

public class WhenClosingVacancy
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Closed(
        long vacancyReference,
        Guid vacancyId,
        Mock<IGetVacancyById_Vacancies> vacancy,
        Mock<IGetVacancyByIdResult> vacancies,
        Mock<IOperationResult<IGetVacancyByIdResult>> operationResult,
        Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        Mock<IRecruitGqlClient> recruitGqlClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        vacancy.Setup(x => x.Id).Returns(vacancyId);
        vacancy.Setup(x => x.VacancyReference).Returns(vacancyReference);
        vacancies.Setup(x => x.Vacancies).Returns(new List<IGetVacancyById_Vacancies> { vacancy.Object });
        operationResult.Setup(x => x.Data).Returns(vacancies.Object);
        operationResult.Setup(x => x.Errors).Returns([]);
        
        var closeVacancyRequest = new CloseVacancyRequest
        {
            VacancyId = vacancyId,
            ClosureReason = ClosureReason.Auto
        };
        
        recruitGqlClient
            .Setup(x => x.GetVacancyById.ExecuteAsync(vacancyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(operationResult.Object);

        PatchVacanciesByVacancyIdApiRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode<JsonPatchDocument<Vacancy>, NullResponse>(It.IsAny<PatchVacanciesByVacancyIdApiRequest>(), false))
            .Callback<IPatchApiRequest<JsonPatchDocument<Vacancy>>, bool>((x, _) => capturedRequest = x as PatchVacanciesByVacancyIdApiRequest)
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.OK, null!));
        
        // act
        var response = await sut.CloseVacancy(vacancyReference, closeVacancyRequest, recruitGqlClient.Object, recruitApiClient.Object, CancellationToken.None) as NoContent;

        // assert
        response.Should().NotBeNull();
        capturedRequest.Should().NotBeNull();
        capturedRequest!.VacancyId.Should().Be(vacancyId);
        capturedRequest.Data.Operations.Should().Contain(x => x.op == "replace" && x.path == "/status" && (VacancyStatus)x.value == VacancyStatus.Closed);
        capturedRequest.Data.Operations.Should().Contain(x => x.op == "replace" && x.path == "/closureReason" && (ClosureReason)x.value == ClosureReason.Auto);
        capturedRequest.Data.Operations.Should().Contain(x => x.op == "replace" && x.path == "/closedDate");
    }
}