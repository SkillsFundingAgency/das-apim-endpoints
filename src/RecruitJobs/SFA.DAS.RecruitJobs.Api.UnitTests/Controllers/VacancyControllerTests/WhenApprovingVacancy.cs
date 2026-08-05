using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.RecruitJobs.Api.Controllers;
using SFA.DAS.RecruitJobs.GraphQL;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Recruit;
using StrawberryShake;
using System.Net;
using System.Threading;
using VacancyStatus = SFA.DAS.RecruitJobs.GraphQL.VacancyStatus;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.VacancyControllerTests;

public class WhenApprovingVacancy
{
    [Test, MoqAutoData]
    public async Task Then_If_There_Is_An_Error_Retrieving_The_Vacancy_Return_Problem(
        Guid vacancyId,
        Mock<IOperationResult<IGetVacancyByIdResult>> operationResult,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        operationResult.Setup(x => x.Errors).Returns([new ClientError("Error")]);
        recruitGqlClient
            .Setup(x => x.GetVacancyById.ExecuteAsync(vacancyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(operationResult.Object);

        // act
        var response = await sut.ApproveVacancy(null, recruitGqlClient.Object, null, vacancyId, CancellationToken.None) as ProblemHttpResult;

        // assert
        response.Should().NotBeNull();
    }
    
    [Test, MoqAutoData]
    public async Task Then_Deleted_Vacancy_Is_Skipped(
        Guid vacancyId,
        Mock<IGetVacancyById_Vacancies> gqlVacancy,
        Mock<IOperationResult<IGetVacancyByIdResult>> operationResult,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        gqlVacancy.Setup(x => x.DeletedDate).Returns(DateTimeOffset.Now);
        gqlVacancy.Setup(x => x.Status).Returns(VacancyStatus.Submitted);
        gqlVacancy.Setup(x => x.TransferInfo).Returns(() => null);
        operationResult.Setup(x => x.Errors).Returns([]);
        operationResult.Setup(x => x.Data.Vacancies).Returns([gqlVacancy.Object]);

        recruitGqlClient
            .Setup(x => x.GetVacancyById.ExecuteAsync(vacancyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(operationResult.Object);

        // act
        var response = await sut.ApproveVacancy(null, recruitGqlClient.Object, recruitApiClient.Object, vacancyId, CancellationToken.None) as NoContent;

        // assert
        response.Should().NotBeNull();
        recruitApiClient.Verify(x => x.PatchWithResponseCode<JsonPatchDocument<PatchableVacancyDto>, NullResponse>(It.IsAny<PatchVacancyRequest>(), false), Times.Never);
    }
    
    [Test, MoqAutoData]
    public async Task Then_Transferred_Vacancy_Is_Skipped(
        Guid vacancyId,
        Mock<IGetVacancyById_Vacancies> gqlVacancy,
        Mock<IOperationResult<IGetVacancyByIdResult>> operationResult,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        gqlVacancy.Setup(x => x.DeletedDate).Returns(() => null);
        gqlVacancy.Setup(x => x.Status).Returns(VacancyStatus.Draft);
        gqlVacancy.Setup(x => x.TransferInfo).Returns("Some json");
        
        operationResult.Setup(x => x.Errors).Returns([]);
        operationResult.Setup(x => x.Data.Vacancies).Returns([gqlVacancy.Object]);

        recruitGqlClient
            .Setup(x => x.GetVacancyById.ExecuteAsync(vacancyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(operationResult.Object);

        // act
        var response = await sut.ApproveVacancy(null, recruitGqlClient.Object, recruitApiClient.Object, vacancyId, CancellationToken.None) as NoContent;

        // assert
        response.Should().NotBeNull();
        recruitApiClient.Verify(x => x.PatchWithResponseCode<JsonPatchDocument<PatchableVacancyDto>, NullResponse>(It.IsAny<PatchVacancyRequest>(), false), Times.Never);
    }
    
    
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Approved(
        Guid vacancyId,
        Mock<IGetVacancyById_Vacancies> gqlVacancy,
        Mock<IOperationResult<IGetVacancyByIdResult>> operationResult,
        [Frozen] Mock<IRecruitGqlClient> recruitGqlClient,
        [Frozen] Mock<Recruit.Contracts.Client.IRecruitApiClient<Recruit.Contracts.Client.RecruitApiConfiguration>> recruitApiClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        gqlVacancy.Setup(x => x.DeletedDate).Returns(() => null);
        gqlVacancy.Setup(x => x.Status).Returns(VacancyStatus.Submitted);
        gqlVacancy.Setup(x => x.TransferInfo).Returns(() => null);
        
        operationResult.Setup(x => x.Errors).Returns([]);
        operationResult.Setup(x => x.Data.Vacancies).Returns([gqlVacancy.Object]);

        recruitGqlClient
            .Setup(x => x.GetVacancyById.ExecuteAsync(vacancyId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(operationResult.Object);

        recruitApiClient
            .Setup(x => x.PatchWithResponseCode<JsonPatchDocument<Recruit.Contracts.ApiResponses.Vacancy>, NullResponse>(It.IsAny<PatchVacanciesByVacancyIdApiRequest>(), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.NoContent, null));

        // act
        var response = await sut.ApproveVacancy(null, recruitGqlClient.Object, recruitApiClient.Object, vacancyId, CancellationToken.None) as NoContent;

        // assert
        response.Should().NotBeNull();
    }
}