using System.Net;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteApplication;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Applications;

public class WhenHandlingDeleteApplicationCommand
{
    [Test, MoqAutoData]
    public async Task Handles_Null_Application_Response(
        DeleteApplicationCommand command,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Greedy] DeleteApplicationCommandHandler sut)
    {
        // arrange
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r =>
                r.GetUrl.Contains(command.ApplicationId.ToString()) &&
                r.GetUrl.Contains(command.ApplicationId.ToString()))))
            .ReturnsAsync((GetApplicationApiResponse)null!);

        // act
        var result = await sut.Handle(command, CancellationToken.None);

        // assert
        result.Should().Be(DeleteApplicationCommandResult.None);
    }
    
    [Test]
    [MoqInlineAutoData(ApplicationStatus.Expired)]
    [MoqInlineAutoData(ApplicationStatus.Submitted)]
    [MoqInlineAutoData(ApplicationStatus.Successful)]
    [MoqInlineAutoData(ApplicationStatus.UnSuccessful)]
    [MoqInlineAutoData(ApplicationStatus.Withdrawn)]
    public async Task Handles_Applications_Whose_Status_Is_Not_Draft(
        ApplicationStatus status,
        DeleteApplicationCommand command,
        GetApplicationApiResponse response,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Greedy] DeleteApplicationCommandHandler sut)
    {
        // arrange
        response.Status = status;
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r =>
                r.GetUrl.Contains(command.ApplicationId.ToString()) &&
                r.GetUrl.Contains(command.ApplicationId.ToString()))))
            .ReturnsAsync(response);

        // act
        var result = await sut.Handle(command, CancellationToken.None);

        // assert
        result.Should().Be(DeleteApplicationCommandResult.None);
    }
    
    [Test, MoqAutoData]
    public async Task The_Response_Is_Returned_Correctly(
        DeleteApplicationCommand command,
        GetApplicationApiResponse applicationResponse,
        GetApprenticeshipVacancyItemResponse vacancyResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<IVacancyService> vacancyService,
        [Greedy] DeleteApplicationCommandHandler sut)
    {
        // arrange
        applicationResponse.Status = ApplicationStatus.Draft;
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r =>
                r.GetUrl.Contains(command.ApplicationId.ToString()) &&
                r.GetUrl.Contains(command.ApplicationId.ToString()))))
            .ReturnsAsync(applicationResponse);

        vacancyService
            .Setup(x => x.GetVacancy(applicationResponse.VacancyReference))
            .ReturnsAsync(vacancyResponse);

        candidateApiClient
            .Setup(x => x.DeleteWithResponseCode<NullResponse>(It.Is<DeleteApplicationApiRequest>(req => req.ApplicationId == command.ApplicationId && req.CandidateId == command.CandidateId), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.NoContent, null));

        // act
        var result = await sut.Handle(command, CancellationToken.None);
        
        // assert
        result.EmployerName.Should().Be(vacancyResponse.EmployerName);
        result.VacancyTitle.Should().Be(vacancyResponse.Title);
    }
    
    [Test, MoqAutoData]
    public async Task Deletion_Failure_Is_Handled_Correctly(
        DeleteApplicationCommand command,
        GetApplicationApiResponse applicationResponse,
        GetApprenticeshipVacancyItemResponse vacancyResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<IVacancyService> vacancyService,
        [Greedy] DeleteApplicationCommandHandler sut)
    {
        // arrange
        applicationResponse.Status = ApplicationStatus.Draft;
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r =>
                r.GetUrl.Contains(command.ApplicationId.ToString()) &&
                r.GetUrl.Contains(command.ApplicationId.ToString()))))
            .ReturnsAsync(applicationResponse);

        vacancyService
            .Setup(x => x.GetVacancy(applicationResponse.VacancyReference))
            .ReturnsAsync(vacancyResponse);

        candidateApiClient
            .Setup(x => x.DeleteWithResponseCode<NullResponse>(It.Is<DeleteApplicationApiRequest>(req => req.ApplicationId == command.ApplicationId && req.CandidateId == command.CandidateId), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.NotFound, null));

        // act
        var result = await sut.Handle(command, CancellationToken.None);
        
        // assert
        result.Should().Be(DeleteApplicationCommandResult.None);
    }
}