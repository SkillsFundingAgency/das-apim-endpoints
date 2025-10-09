using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.Recruit.Application.Candidates.Commands.CandidateApplicationStatus;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System;
using System.Net;
using System.Threading;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;

namespace SFA.DAS.Recruit.UnitTests.Application.Candidates.Commands;

public class WhenHandlingCandidateApplicationStatusCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_The_Api_Called_And_Success_Email_Sent_When_Successful(
        EmailEnvironmentHelper emailEnvironmentHelper,
        CandidateApplicationStatusCommand request,
        GetCandidateApiResponse candidateResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> apiClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        CandidateApplicationStatusCommandHandler handler)
    {
        request.Outcome = "successful";
        apiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.Accepted, ""));
        apiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(It.Is<GetCandidateByIdApiRequest>(c=>c.GetUrl.Contains(request.CandidateId.ToString()))))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(candidateResponse, HttpStatusCode.OK, ""));
        recruitApiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchRecruitApplicationReviewStatusApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.Accepted, ""));

        await handler.Handle(request, CancellationToken.None);
        
        apiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.Data.Operations[0].path == "/ResponseNotes" &&
                c.Data.Operations[0].value.ToString() == request.Feedback &&
                c.Data.Operations[1].path == "/Status" &&
                (ApplicationStatus)c.Data.Operations[1].value == ApplicationStatus.Successful
            )), Times.Once
        );
        recruitApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchRecruitApplicationReviewStatusApiRequest>(c =>
                c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.Data.Operations[0].path == "/CandidateFeedback" &&
                c.Data.Operations[0].value.ToString() == request.Feedback &&
                c.Data.Operations[1].path == "/Status" &&
                (string?)c.Data.Operations[1].value == Enum.GetName(ApplicationStatus.Successful)
            )), Times.Once
        );
        notificationService.Verify(x=>x.Send(
            It.Is<SendEmailCommand>(c=>
                c.RecipientsAddress == candidateResponse.Email
                && c.TemplateId == emailEnvironmentHelper.SuccessfulApplicationEmailTemplateId
                && c.Tokens["firstName"] == candidateResponse.FirstName
                && c.Tokens["vacancy"] == request.VacancyTitle
                && c.Tokens["employer"] == request.VacancyEmployerName
                && c.Tokens["location"] == request.VacancyLocation
            )
        ), Times.Once);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_The_Api_Called_And_UnSuccess_Email_Sent_When_Unsuccessful(
        CandidateApplicationStatusCommand request,
        EmailEnvironmentHelper emailEnvironmentHelper,
        GetCandidateApiResponse candidateResponse,
        [Frozen]Mock<ICandidateApiClient<CandidateApiConfiguration>> apiClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        CandidateApplicationStatusCommandHandler handler)
    {
        request.Outcome = "unsuccessful";
        apiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.Accepted, ""));
        apiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(It.Is<GetCandidateByIdApiRequest>(c=>c.GetUrl.Contains(request.CandidateId.ToString()))))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(candidateResponse, HttpStatusCode.OK, ""));
        recruitApiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchRecruitApplicationReviewStatusApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.Accepted, ""));

        await handler.Handle(request, CancellationToken.None);
        
        apiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.Data.Operations[0].path == "/ResponseNotes" &&
                c.Data.Operations[0].value.ToString() == request.Feedback &&
                c.Data.Operations[1].path == "/Status" &&
                (ApplicationStatus)c.Data.Operations[1].value == ApplicationStatus.UnSuccessful
            )), Times.Once
        );
        recruitApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchRecruitApplicationReviewStatusApiRequest>(c =>
                c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.Data.Operations[0].path == "/CandidateFeedback" &&
                c.Data.Operations[0].value.ToString() == request.Feedback &&
                c.Data.Operations[1].path == "/Status" &&
                (string?)c.Data.Operations[1].value == Enum.GetName(ApplicationStatus.UnSuccessful)
            )), Times.Once
        );
        notificationService.Verify(x=>x.Send(
            It.Is<SendEmailCommand>(c=>
                c.RecipientsAddress == candidateResponse.Email
                && c.TemplateId == emailEnvironmentHelper.UnsuccessfulApplicationEmailTemplateId
                && c.Tokens["firstName"] == candidateResponse.FirstName
                && c.Tokens["vacancy"] == request.VacancyTitle
                && c.Tokens["employer"] == request.VacancyEmployerName
                && c.Tokens["location"] == request.VacancyLocation
            )
        ), Times.Once);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_The_Api_Called_And_Success_Email_Sent_When_Successful_With_Null_Or_Empty_Location_Values(
        EmailEnvironmentHelper emailEnvironmentHelper,
        CandidateApplicationStatusCommand request,
        GetCandidateApiResponse candidateResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> apiClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        CandidateApplicationStatusCommandHandler handler)
    {
        request.VacancyLocation = null;
        request.Outcome = "successful";
        apiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.Accepted, ""));
        apiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(It.Is<GetCandidateByIdApiRequest>(c=>c.GetUrl.Contains(request.CandidateId.ToString()))))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(candidateResponse, HttpStatusCode.OK, ""));
        recruitApiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchRecruitApplicationReviewStatusApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.Accepted, ""));

        await handler.Handle(request, CancellationToken.None);
        
        apiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.Data.Operations[0].path == "/ResponseNotes" &&
                c.Data.Operations[0].value.ToString() == request.Feedback &&
                c.Data.Operations[1].path == "/Status" &&
                (ApplicationStatus)c.Data.Operations[1].value == ApplicationStatus.Successful
            )), Times.Once
        );
        recruitApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchRecruitApplicationReviewStatusApiRequest>(c =>
                c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.Data.Operations[0].path == "/CandidateFeedback" &&
                c.Data.Operations[0].value.ToString() == request.Feedback &&
                c.Data.Operations[1].path == "/Status" &&
                (string?)c.Data.Operations[1].value == Enum.GetName(ApplicationStatus.Successful)
            )), Times.Once
        );
        notificationService.Verify(x=>x.Send(
            It.Is<SendEmailCommand>(c=>
                c.RecipientsAddress == candidateResponse.Email
                && c.TemplateId == emailEnvironmentHelper.SuccessfulApplicationEmailTemplateId
                && c.Tokens["firstName"] == candidateResponse.FirstName
                && c.Tokens["vacancy"] == request.VacancyTitle
                && c.Tokens["employer"] == request.VacancyEmployerName
                && c.Tokens["location"] == "Unknown"
            )
        ), Times.Once);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Command_Is_Handled_And_The_Api_Called_And_UnSuccess_Email_Sent_When_Unsuccessful_With_Null_Or_Empty_Location_Values(
        CandidateApplicationStatusCommand request,
        EmailEnvironmentHelper emailEnvironmentHelper,
        GetCandidateApiResponse candidateResponse,
        [Frozen]Mock<ICandidateApiClient<CandidateApiConfiguration>> apiClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        CandidateApplicationStatusCommandHandler handler)
    {
        request.VacancyLocation = null;
        request.Outcome = "unsuccessful";
        apiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.Accepted, ""));
        apiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(It.Is<GetCandidateByIdApiRequest>(c=>c.GetUrl.Contains(request.CandidateId.ToString()))))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(candidateResponse, HttpStatusCode.OK, ""));
        recruitApiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchRecruitApplicationReviewStatusApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.Accepted, ""));

        await handler.Handle(request, CancellationToken.None);
        
        apiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.Data.Operations[0].path == "/ResponseNotes" &&
                c.Data.Operations[0].value.ToString() == request.Feedback &&
                c.Data.Operations[1].path == "/Status" &&
                (ApplicationStatus)c.Data.Operations[1].value == ApplicationStatus.UnSuccessful
            )), Times.Once
        );

        recruitApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchRecruitApplicationReviewStatusApiRequest>(c =>
                c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.Data.Operations[0].path == "/CandidateFeedback" &&
                c.Data.Operations[0].value.ToString() == request.Feedback &&
                c.Data.Operations[1].path == "/Status" &&
                (string?)c.Data.Operations[1].value == Enum.GetName(ApplicationStatus.UnSuccessful)
            )), Times.Once
        );

        notificationService.Verify(x=>x.Send(
            It.Is<SendEmailCommand>(c=>
                c.RecipientsAddress == candidateResponse.Email
                && c.TemplateId == emailEnvironmentHelper.UnsuccessfulApplicationEmailTemplateId
                && c.Tokens["firstName"] == candidateResponse.FirstName
                && c.Tokens["vacancy"] == request.VacancyTitle
                && c.Tokens["employer"] == request.VacancyEmployerName
                && c.Tokens["location"] == "Unknown"
            )
        ), Times.Once);
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_The_Application_Id_Is_Empty_The_Candidate_Is_Looked_Up_And_Application_Is_Looked_Up_By_CandidateId_And_VacancyReference_And_Submitted_Application_ReSubmitted(
        GetCandidateApiResponse apiCandidateResponse,
        GetApplicationByReferenceApiResponse apiResponse,
        CandidateApplicationStatusCommand request,
        [Frozen]Mock<ICandidateApiClient<CandidateApiConfiguration>> apiClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        CandidateApplicationStatusCommandHandler handler)
    {
        request.ApplicationId = Guid.Empty;
        request.Outcome = "successful";
        var expectedGetRequestMigratedCandidate = new GetCandidateByMigratedCandidateIdApiRequest(request.CandidateId);
        apiClient.Setup(x =>
                x.GetWithResponseCode<GetCandidateApiResponse>(
                    It.Is<GetCandidateByMigratedCandidateIdApiRequest>(c => c.GetUrl == expectedGetRequestMigratedCandidate.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(apiCandidateResponse, HttpStatusCode.OK, ""));
        var expectedGetRequest = new GetApplicationByReferenceApiRequest(apiCandidateResponse.Id, request.VacancyReference);
        apiClient.Setup(x =>
                x.GetWithResponseCode<GetApplicationByReferenceApiResponse>(
                    It.Is<GetApplicationByReferenceApiRequest>(c => c.GetUrl == expectedGetRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetApplicationByReferenceApiResponse>(apiResponse, HttpStatusCode.OK, ""));
        apiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.Accepted, ""));
        recruitApiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchRecruitApplicationReviewStatusApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.Accepted, ""));

        await handler.Handle(request, CancellationToken.None);
        
        apiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                c.PatchUrl.Contains(apiResponse.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.PatchUrl.Contains(apiResponse.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.Data.Operations[0].path == "/ResponseNotes" &&
                c.Data.Operations[0].value.ToString() == request.Feedback &&
                c.Data.Operations[1].path == "/Status" &&
                (ApplicationStatus)c.Data.Operations[1].value == ApplicationStatus.Successful
            )), Times.Once
        );

        recruitApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchRecruitApplicationReviewStatusApiRequest>(c =>
                c.PatchUrl.Contains(apiResponse.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.Data.Operations[0].path == "/CandidateFeedback" &&
                c.Data.Operations[0].value.ToString() == request.Feedback &&
                c.Data.Operations[1].path == "/Status" &&
                (string?) c.Data.Operations[1].value == Enum.GetName(ApplicationStatus.Successful)
            )), Times.Once
        );
    }
    [Test, MoqAutoData]
    public async Task Then_If_The_Application_Id_Is_Empty_The_Candidate_Is_Looked_Up_And_If_Not_Found_Then_Nothing_Updated(
        GetCandidateApiResponse apiCandidateResponse,
        GetApplicationByReferenceApiResponse apiResponse,
        CandidateApplicationStatusCommand request,
        [Frozen]Mock<ICandidateApiClient<CandidateApiConfiguration>> apiClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        CandidateApplicationStatusCommandHandler handler)
    {
        request.ApplicationId = Guid.Empty;
        var expectedGetRequestMigratedCandidate = new GetCandidateByMigratedCandidateIdApiRequest(request.CandidateId);
        apiClient.Setup(x =>
                x.GetWithResponseCode<GetCandidateApiResponse>(
                    It.Is<GetCandidateByMigratedCandidateIdApiRequest>(c => c.GetUrl == expectedGetRequestMigratedCandidate.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null, HttpStatusCode.NotFound, ""));
        
        await handler.Handle(request, CancellationToken.None);
        
        apiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>(
            )), Times.Never
        );
        recruitApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchRecruitApplicationReviewStatusApiRequest>(
            )), Times.Never
        );
    }
}