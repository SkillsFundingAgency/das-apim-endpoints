using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.RecruitJobs.Api.Controllers;
using SFA.DAS.RecruitJobs.Api.Models.Requests;
using SFA.DAS.RecruitJobs.Handlers;
using StrawberryShake;

namespace SFA.DAS.RecruitJobs.Api.UnitTests.Controllers.UpdatedEmployerPermissionsControllerTests;

public class WhenGettingVacanciesToTransfer
{
    public record MockVacancyDetails(
        Guid Id,
        long? VacancyReference,
        string? Title,
        VacancyStatus Status,
        string? TrainingProvider_Name,
        string? LegalEntityName,
        int? Ukprn) : IGetProviderTransferableVacancyDetails_Vacancies;
    
    [Test, MoqAutoData]
    public async Task Then_Vacancies_Are_Returned(
        int ukprn,
        long accountLegalEntityId,
        MockVacancyDetails vacancyDetails,
        IReadOnlyList<IGetProviderTransferableVacancies_Vacancies> vacancies,
        Mock<IGetProviderTransferableVacanciesResult> data,
        Mock<IRecruitGqlClient> recruitGqlClient,
        [Greedy] UpdatedEmployerPermissionsController sut)
    {
        // arrange
        data
            .Setup(x => x.Vacancies)
            .Returns(vacancies);
        var response = new OperationResult<IGetProviderTransferableVacanciesResult>(data.Object, null, null!, null); 
        recruitGqlClient
            .Setup(x => x.GetProviderTransferableVacancies.ExecuteAsync(ukprn, accountLegalEntityId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        // act
        var result = await sut.GetVacanciesToTransfer(recruitGqlClient.Object, ukprn, accountLegalEntityId, CancellationToken.None) as Ok<IEnumerable<Guid>>;

        // assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        result.Value.Should().BeEquivalentTo(vacancies.Select(x => x.Id));
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Transfer_Vacancy_Handler_Is_Called(
        Guid vacancyId,
        TransferVacancyRequest transferRequest,
        Mock<ITransferProviderVacancyToLegalEntityHandler> handler,
        [Greedy] UpdatedEmployerPermissionsController sut)
    {
        // act
        var result = await sut.TransferVacancy(handler.Object, vacancyId, transferRequest, CancellationToken.None) as Ok;

        // assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
        handler.Verify(x => x.HandleAsync(vacancyId, transferRequest.TransferReason, CancellationToken.None), Times.Once);
    }
}