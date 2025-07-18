using SFA.DAS.FindAnApprenticeship.Application.Queries.DeleteApplication;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Applications;

public class WhenHandlingConfirmDeleteApplicationQuery
{
    [Test, MoqAutoData]
    public async Task Handles_Null_Application_Response(
        ConfirmDeleteApplicationQuery query,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Greedy] ConfirmDeleteApplicationQueryHandler sut)
    {
        // arrange
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r =>
                r.GetUrl.Contains(query.ApplicationId.ToString()) &&
                r.GetUrl.Contains(query.ApplicationId.ToString()))))
            .ReturnsAsync((GetApplicationApiResponse)null!);

        // act
        var result = await sut.Handle(query, CancellationToken.None);

        // assert
        result.Should().Be(ConfirmDeleteApplicationQueryResult.None);
    }
    
    [Test]
    [MoqInlineAutoData(ApplicationStatus.Expired)]
    [MoqInlineAutoData(ApplicationStatus.Submitted)]
    [MoqInlineAutoData(ApplicationStatus.Successful)]
    [MoqInlineAutoData(ApplicationStatus.UnSuccessful)]
    [MoqInlineAutoData(ApplicationStatus.Withdrawn)]
    public async Task Handles_Applications_Whose_Status_Is_Not_Draft(
        ApplicationStatus status,
        ConfirmDeleteApplicationQuery query,
        GetApplicationApiResponse response,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Greedy] ConfirmDeleteApplicationQueryHandler sut)
    {
        // arrange
        response.Status = status;
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r =>
                r.GetUrl.Contains(query.ApplicationId.ToString()) &&
                r.GetUrl.Contains(query.ApplicationId.ToString()))))
            .ReturnsAsync(response);

        // act
        var result = await sut.Handle(query, CancellationToken.None);

        // assert
        result.Should().Be(ConfirmDeleteApplicationQueryResult.None);
    }
    
    [Test, MoqAutoData]
    public async Task The_Response_Is_Returned_Correctly(
        ConfirmDeleteApplicationQuery query,
        GetApplicationApiResponse applicationResponse,
        GetApprenticeshipVacancyItemResponse vacancyResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<IVacancyService> vacancyService,
        [Greedy] ConfirmDeleteApplicationQueryHandler sut)
    {
        // arrange
        applicationResponse.Status = ApplicationStatus.Draft;
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r =>
                r.GetUrl.Contains(query.ApplicationId.ToString()) &&
                r.GetUrl.Contains(query.ApplicationId.ToString()))))
            .ReturnsAsync(applicationResponse);

        vacancyService
            .Setup(x => x.GetVacancy(applicationResponse.VacancyReference))
            .ReturnsAsync(vacancyResponse);

        // act
        var result = await sut.Handle(query, CancellationToken.None);
        
        // assert
        result.Address.Should().BeEquivalentTo(vacancyResponse.Address);
        result.ApplicationId.Should().Be(applicationResponse.Id);
        result.ApplicationStartDate.Should().Be(applicationResponse.CreatedDate);
        result.ApprenticeshipType.Should().Be(vacancyResponse.ApprenticeshipType);
        result.EmployerLocationOption.Should().Be(vacancyResponse.EmployerLocationOption);
        result.EmployerName.Should().Be(vacancyResponse.EmployerName);
        result.OtherAddresses.Should().BeEquivalentTo(vacancyResponse.OtherAddresses);
        result.VacancyClosingDate.Should().Be(vacancyResponse.ClosingDate);
        result.VacancyClosedDate.Should().Be(vacancyResponse.ClosedDate);
        result.VacancyTitle.Should().Be(vacancyResponse.Title);
    }
}