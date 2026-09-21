using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.Recruit.GraphQL;
using SFA.DAS.RecruitQa.Api.Controllers;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using StrawberryShake;
using VacancyStatus = SFA.DAS.Recruit.GraphQL.VacancyStatus;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Controller.Vacancies;

public class WhenReferringVacancy
{
    private static IGetVacancyByReference_Vacancies CreateVacancy(long vacancyReference, VacancyStatus vacancyStatus, DateTime? closedDate)
    {
        var vacancy = new Mock<IGetVacancyByReference_Vacancies>();
        vacancy
            .Setup(x => x.Id)
            .Returns(Guid.NewGuid());
        vacancy
            .Setup(x => x.VacancyReference)
            .Returns(vacancyReference);
        vacancy
            .Setup(x => x.ClosedDate)
            .Returns(closedDate);
        vacancy
            .Setup(x => x.Status)
            .Returns(vacancyStatus);
        
        return vacancy.Object;
    }
    
    [Test, MoqAutoData]
    public async Task Vacancy_Is_Referred(
        long vacancyReference,
        Mock<IRecruitGqlClient> recruitGqlClient,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        Mock<IOperationResult<IGetVacancyByReferenceResult>> operationResult)
    {
        // arrange
        var sut = new VacanciesController();

        var vacancy = CreateVacancy(vacancyReference, VacancyStatus.Submitted, null);
        
        operationResult
            .Setup(x => x.Data!.Vacancies)
            .Returns(new List<IGetVacancyByReference_Vacancies> { vacancy });
        operationResult
            .Setup(x => x.Errors)
            .Returns(new List<IClientError>());
        
        recruitGqlClient
            .Setup(x => x.GetVacancyByReference.ExecuteAsync(vacancyReference, It.IsAny<CancellationToken>()))
            .ReturnsAsync(operationResult.Object);
        
        PatchVacanciesByVacancyIdApiRequest? capturedRequest = null;
        recruitApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchVacanciesByVacancyIdApiRequest>()))
            .Callback<IPatchApiRequest<JsonPatchDocument<Vacancy>>>(x => capturedRequest = x as PatchVacanciesByVacancyIdApiRequest)
            .ReturnsAsync(new ApiResponse<string>(string.Empty, HttpStatusCode.OK, string.Empty));

        // act
        var result = await sut.ReferVacancyFromQa(recruitGqlClient.Object, recruitApiClient.Object, vacancyReference, CancellationToken.None);

        // assert
        result.Should().BeOfType<Ok>();
        capturedRequest.Should().NotBeNull();
        capturedRequest.VacancyId.Should().Be(vacancy.Id);
        capturedRequest.Data.Should().NotBeNull();
        capturedRequest.Data.Operations.Should().ContainEquivalentOf(new Operation<Vacancy>("replace", "/status", null, VacancyStatus.Referred));
    }
    
    [Test, MoqAutoData]
    public async Task Vacancy_Is_Not_Found(
        long vacancyReference,
        Mock<IRecruitGqlClient> recruitGqlClient,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        Mock<IOperationResult<IGetVacancyByReferenceResult>> operationResult)
    {
        // arrange
        var sut = new VacanciesController();

        operationResult
            .Setup(x => x.Data!.Vacancies)
            .Returns(new List<IGetVacancyByReference_Vacancies>());
        operationResult
            .Setup(x => x.Errors)
            .Returns(new List<IClientError>());
        
        recruitGqlClient
            .Setup(x => x.GetVacancyByReference.ExecuteAsync(vacancyReference, It.IsAny<CancellationToken>()))
            .ReturnsAsync(operationResult.Object);
        
        // act
        var result = await sut.ReferVacancyFromQa(recruitGqlClient.Object, recruitApiClient.Object, vacancyReference, CancellationToken.None);

        // assert
        result.Should().BeOfType<NotFound>();
    }
    
    [Test]
    [MoqInlineAutoData(VacancyStatus.Draft, false)]
    [MoqInlineAutoData(VacancyStatus.Review, false)]
    [MoqInlineAutoData(VacancyStatus.Rejected, false)]
    [MoqInlineAutoData(VacancyStatus.Submitted, true)]
    [MoqInlineAutoData(VacancyStatus.Referred, false)]
    [MoqInlineAutoData(VacancyStatus.Live, false)]
    [MoqInlineAutoData(VacancyStatus.Closed, false)]
    [MoqInlineAutoData(VacancyStatus.Approved, false)]
    public async Task Vacancy_Is_Not_In_The_Correct_State_To_Be_Referred(
        VacancyStatus vacancyStatus,
        bool isClosed,
        long vacancyReference,
        Mock<IRecruitGqlClient> recruitGqlClient,
        Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        Mock<IOperationResult<IGetVacancyByReferenceResult>> operationResult)
    {
        // arrange
        var sut = new VacanciesController();
        var vacancy = CreateVacancy(vacancyReference, vacancyStatus, isClosed ? DateTime.UtcNow : null);
        
        operationResult
            .Setup(x => x.Data!.Vacancies)
            .Returns(new List<IGetVacancyByReference_Vacancies> { vacancy });
        operationResult
            .Setup(x => x.Errors)
            .Returns(new List<IClientError>());
        
        recruitGqlClient
            .Setup(x => x.GetVacancyByReference.ExecuteAsync(vacancyReference, It.IsAny<CancellationToken>()))
            .ReturnsAsync(operationResult.Object);

        // act
        var result = await sut.ReferVacancyFromQa(recruitGqlClient.Object, recruitApiClient.Object, vacancyReference, CancellationToken.None);

        // assert
        result.Should().BeOfType<BadRequest<string>>();
    }
}