using System.Net;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Application;

public class WhenHandlingProcessVacancyClosedEarlyCommand
{
    [Test]
    [MoqInlineAutoData("address1","address2","address3","address4","address4")]
    [MoqInlineAutoData("address1","address2","address3",null,"address3")]
    [MoqInlineAutoData("address1","address2",null,null,"address2")]
    [MoqInlineAutoData("address1",null,null,null,"address1")]
    public async Task Then_The_Candidates_Are_Found_Emails_Sent_And_Application_Status_Updated(
        string address1,
        string address2,
        string address3,
        string address4,
        string expectedAddress,
        ProcessVacancyClosedEarlyCommand command,
        GetCandidateApplicationApiResponse candidateApiResponseAll,
        GetLiveVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        ProcessVacancyClosedEarlyCommandHandler handler)
    {
        recruitApiResponse.EmployerLocation.AddressLine1 = address1;
        recruitApiResponse.EmployerLocation.AddressLine2 = address2;
        recruitApiResponse.EmployerLocation.AddressLine3 = address3;
        recruitApiResponse.EmployerLocation.AddressLine4 = address4;
        
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
            .Setup(x => x.Get<GetLiveVacancyApiResponse>(
                It.Is<GetLiveVacancyApiRequest>(c => 
                    c.GetUrl.Contains(command.VacancyReference.ToString()))))
            .ReturnsAsync(recruitApiResponse);

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
            notificationService.Verify(x=>x.Send(
                It.Is<SendEmailCommand>(c=>
                    c.RecipientsAddress == candidate.Candidate.Email
                    && c.TemplateId == emailEnvironmentHelper.VacancyClosedEarlyTemplateId
                    && c.Tokens["firstName"] == candidate.Candidate.FirstName
                    && c.Tokens["vacancy"] == recruitApiResponse.Title
                    && c.Tokens["employer"] == recruitApiResponse.EmployerName 
                    && c.Tokens["city"] == expectedAddress
                    && c.Tokens["dateApplicationStarted"] == candidate.ApplicationCreatedDate.ToString("d MMM yyyy") 
                    && c.Tokens["postcode"] == recruitApiResponse.EmployerLocation!.Postcode
                    && !string.IsNullOrEmpty(c.Tokens["vacancyUrl"])
                    && !string.IsNullOrEmpty(c.Tokens["settingsUrl"])
                )
            ), Times.Once);
        }
        
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_No_Candidates_Are_Found_No_Emails_Sent(
        ProcessVacancyClosedEarlyCommand command,
        GetLiveVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        ProcessVacancyClosedEarlyCommandHandler handler)
    {
        candidateApiClient
            .Setup(x => x.Get<GetCandidateApplicationApiResponse>(
                It.Is<GetCandidateApplicationsByVacancyRequest>(c =>
                    c.GetUrl.Contains(command.VacancyReference.ToString())
                    ))).ReturnsAsync(new GetCandidateApplicationApiResponse{Candidates = []});
        recruitApiClient
            .Setup(x => x.Get<GetLiveVacancyApiResponse>(
                It.Is<GetLiveVacancyApiRequest>(c => 
                    c.GetUrl.Contains(command.VacancyReference.ToString()))))
            .ReturnsAsync(recruitApiResponse);

        await handler.Handle(command, CancellationToken.None);

        notificationService.Verify(x=>x.Send(
            It.IsAny<SendEmailCommand>()
        ), Times.Never);
        candidateApiClient.Verify(x => 
                x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never);
        
    }
    
}