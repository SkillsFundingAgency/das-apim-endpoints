using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.RecruitJobs.Api.Controllers;
using SFA.DAS.RecruitJobs.InnerApi.Requests.DeleteVacancy;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.VacanciesControllerTests;

[TestFixture]
internal class WhenDeletingOneVacancy
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_To_Delete_Message_Is_Sent_Correctly(
        Guid id,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        var response = new ApiResponse<NullResponse>(null!, HttpStatusCode.NoContent, null);
        DeleteVacancyByIdRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.DeleteWithResponseCode<NullResponse>(It.IsAny<DeleteVacancyByIdRequest>(), false))
            .Callback<IDeleteApiRequest, bool>((x, _) => capturedRequest = x as DeleteVacancyByIdRequest)
            .ReturnsAsync(response);

        var expectedUrl = $"api/vacancies/{id}";

        // act
        var result = await sut.DeleteOne(recruitApiClient.Object, id) as StatusCodeHttpResult;

        // assert
        result!.StatusCode.Should().Be((int)response.StatusCode);
        capturedRequest.Should().NotBeNull();
        capturedRequest!.DeleteUrl.Should().Be(expectedUrl);
    }
}