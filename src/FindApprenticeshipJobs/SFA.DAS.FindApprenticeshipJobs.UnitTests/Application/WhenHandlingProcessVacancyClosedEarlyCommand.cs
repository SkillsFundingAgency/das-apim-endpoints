using SFA.DAS.FindApprenticeshipJobs.Application.Commands;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Application;

public class WhenHandlingProcessVacancyClosedEarlyCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Candidates_Are_Found_Emails_Sent_And_Application_Status_Updated(
        ProcessVacancyClosedEarlyCommand command,
        GetCandidateApplicationApiResponse candidateApiResponseAll,
        GetClosedVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        ProcessVacancyClosedEarlyCommandHandler handler)
    {
        candidateApiClient.Setup(x => 
                x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.Accepted, ""));
        
        var candidateGetRequestAll =
            new GetCandidateApplicationsByVacancyRequest(command.VacancyReference.ToString(), null,
                false);
       candidateApiClient
            .Setup(x => x.Get<GetCandidateApplicationApiResponse>(
                It.Is<GetCandidateApplicationsByVacancyRequest>(c =>
                    c.GetUrl == candidateGetRequestAll.GetUrl))).ReturnsAsync(candidateApiResponseAll);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetClosedVacancyApiResponse>(
                It.Is<GetClosedVacancyApiRequest>(c => 
                    c.GetUrl.Contains(command.VacancyReference.ToString()))))
            .ReturnsAsync(new ApiResponse<GetClosedVacancyApiResponse>(recruitApiResponse, HttpStatusCode.OK, ""));

        await handler.Handle(command, CancellationToken.None);

        foreach (var candidate in candidateApiResponseAll.Candidates)
        {
            candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                    c.PatchUrl.Contains(candidate.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.PatchUrl.Contains(candidate.Candidate.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.Data.Operations[0].path == "/Status" &&
                    (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Expired
                )), Times.Once
                
            );
        }
    }
    
    [Test,MoqAutoData]
    public async Task Then_The_Get_Closed_Vacancy_Request_Is_Retried_If_NotFound_Candidates_Are_Found_Emails_Sent_And_Application_Status_Updated(
        ProcessVacancyClosedEarlyCommand command,
        GetCandidateApplicationApiResponse candidateApiResponseAll,
        GetClosedVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        ProcessVacancyClosedEarlyCommandHandler handler)
    {
        candidateApiClient.Setup(x => 
                x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.Accepted, ""));
        
        var candidateGetRequestAll =
            new GetCandidateApplicationsByVacancyRequest(command.VacancyReference.ToString(), null,
                false);
       candidateApiClient
            .Setup(x => x.Get<GetCandidateApplicationApiResponse>(
                It.Is<GetCandidateApplicationsByVacancyRequest>(c =>
                    c.GetUrl == candidateGetRequestAll.GetUrl))).ReturnsAsync(candidateApiResponseAll);
        recruitApiClient
            .SetupSequence(x => x.GetWithResponseCode<GetClosedVacancyApiResponse>(
                It.Is<GetClosedVacancyApiRequest>(c => 
                    c.GetUrl.Contains(command.VacancyReference.ToString()))))
            .ReturnsAsync(new ApiResponse<GetClosedVacancyApiResponse>(recruitApiResponse, HttpStatusCode.OK, ""));

        await handler.Handle(command, CancellationToken.None);

        foreach (var candidate in candidateApiResponseAll.Candidates)
        {
            candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                    c.PatchUrl.Contains(candidate.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.PatchUrl.Contains(candidate.Candidate.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.Data.Operations[0].path == "/Status" &&
                    (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Expired
                )), Times.Once
            );
        }
    }

    [Test, MoqAutoData]
    public Task Then_If_Cannot_Find_Vacancy_Exception_Is_Thrown(
        ProcessVacancyClosedEarlyCommand command,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        ProcessVacancyClosedEarlyCommandHandler handler)
    {
        candidateApiClient
            .Setup(x => x.Get<GetCandidateApplicationApiResponse>(
                It.Is<GetCandidateApplicationsByVacancyRequest>(c =>
                    c.GetUrl.Contains(command.VacancyReference.ToString())
                    ))).ReturnsAsync(new GetCandidateApplicationApiResponse{Candidates = []});
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetClosedVacancyApiResponse>(
                It.Is<GetClosedVacancyApiRequest>(c => 
                    c.GetUrl.Contains(command.VacancyReference.ToString()))))
            .ReturnsAsync(new ApiResponse<GetClosedVacancyApiResponse>(null!, HttpStatusCode.NotFound, ""));

        Assert.ThrowsAsync<Exception>(()=> handler.Handle(command, CancellationToken.None));

        return Task.CompletedTask;
    }
}