using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.Api.Models.Responses;
using SFA.DAS.RecruitQa.InnerApi.Requests;
using SFA.DAS.RecruitQa.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.Vacancies;

public class WhenCallingPostVacancy
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_Ok_Result_With_Mapped_Response_Returned(
        Guid vacancyId,
        PostVacancyRequest request,
        PutVacancyResponse putResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ILogger<VacanciesController>> logger)
    {
        var apiResponse = new ApiResponse<PutVacancyResponse>(putResponse, HttpStatusCode.OK, "");

        recruitApiClient
            .Setup(x => x.PutWithResponseCode<PutVacancyResponse>(
                It.Is<PutVacancyRequest>(r => r.PutUrl == $"api/vacancies/{vacancyId}")))
            .ReturnsAsync(apiResponse);

        var controller = new VacanciesController(logger.Object);
        var actual = await controller.PostOne(vacancyId, request, new VacancyMapper(), recruitApiClient.Object) as OkObjectResult;

        actual.Should().NotBeNull();
        var body = actual!.Value as PostVacancyResponse;
        body.Should().NotBeNull();
        body!.Id.Should().Be(putResponse.Id);
        body.Status.Should().Be(putResponse.Status);
        body.Title.Should().Be(putResponse.Title);
    }

    [Test, MoqAutoData]
    public async Task Then_When_Api_Returns_An_Error_Status_Code_A_Problem_Result_Is_Returned(
        Guid vacancyId,
        PostVacancyRequest request,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ILogger<VacanciesController>> logger)
    {
        var apiResponse = new ApiResponse<PutVacancyResponse>(null!, HttpStatusCode.InternalServerError, "something went wrong");

        recruitApiClient
            .Setup(x => x.PutWithResponseCode<PutVacancyResponse>(It.IsAny<PutVacancyRequest>()))
            .ReturnsAsync(apiResponse);

        var controller = new VacanciesController(logger.Object);
        var actual = await controller.PostOne(vacancyId, request, new VacancyMapper(), recruitApiClient.Object) as ObjectResult;

        actual.Should().NotBeNull();
        actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
