using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Helpers;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.SubmitApplication;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply;

public class WhenHandlingSubmitApplicationCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Application_Is_Submitted_To_Recruit(
        string vacancyReference,
        SubmitApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        SubmitApplicationCommandHandler handler)
    {
        var expectedGetApplicationRequest =
            new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true);
        applicationApiResponse.VacancyReference = vacancyReference;
        candidateApiClient.Setup(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                c.Data.Operations[0].path == "/Status" &&
                (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Submitted
            ))).ReturnsAsync(new ApiResponse<string>("",HttpStatusCode.Accepted,""));
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(c => 
                    c.GetUrl == expectedGetApplicationRequest.GetUrl
                    )))
            .ReturnsAsync(applicationApiResponse);
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.Is<PostSubmitApplicationRequest>(c => 
                    c.PostUrl.Contains(request.CandidateId.ToString())
                    && ((PostSubmitApplicationRequestData)c.Data).VacancyReference == vacancyReference
                ), false)).ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.NoContent, ""));

        candidateApiClient.Setup(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
            c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/Status" &&
            (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Submitted
        ))).ReturnsAsync(() => new ApiResponse<string>("", HttpStatusCode.OK, ""));

        var actual = await handler.Handle(request, CancellationToken.None);
        
        actual.Should().BeTrue();
    }
    
    [Test]
    [MoqInlineAutoData("address1","address2","address3","address4","address4")]
    [MoqInlineAutoData("address1","address2","address3",null,"address3")]
    [MoqInlineAutoData("address1","address2",null,null,"address2")]
    [MoqInlineAutoData("address1",null,null,null,"address1")]
    public async Task Then_The_ApplicationStatus_Is_Updated_If_Submitted_To_Recruit_And_Notification_Sent(
        string address1,
        string address2,
        string address3,
        string address4,
        string expectedAddress,
        SubmitApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        GetApprenticeshipVacancyItemResponse vacancyResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IVacancyService> vacancyService,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        SubmitApplicationCommandHandler handler)
    {
        vacancyResponse.Address.AddressLine1 = address1;
        vacancyResponse.Address.AddressLine2 = address2;
        vacancyResponse.Address.AddressLine3 = address3;
        vacancyResponse.Address.AddressLine4 = address4;
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(c => 
                    c.GetUrl.Contains(request.CandidateId.ToString()) && c.GetUrl.Contains(request.ApplicationId.ToString()))))
            .ReturnsAsync(applicationApiResponse);
        candidateApiClient.Setup(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
            c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/Status" &&
            (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Submitted
        ))).ReturnsAsync(new ApiResponse<string>("",HttpStatusCode.Accepted,""));
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.Is<PostSubmitApplicationRequest>(c => 
                    c.PostUrl.Contains(request.CandidateId.ToString())
                ), false)).ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.NoContent, ""));

        candidateApiClient.Setup(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
            c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/Status" &&
            (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Submitted
        ))).ReturnsAsync(() => new ApiResponse<string>("", HttpStatusCode.OK, ""));
        
        vacancyService.Setup(x => x.GetVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync(vacancyResponse);


        var actual = await handler.Handle(request, CancellationToken.None);

        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
            c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/Status" &&
            (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Submitted
            )), Times.Once
        );
        actual.Should().BeTrue();
        notificationService.Verify(x=>x.Send(
            It.Is<SendEmailCommand>(c=>
                c.RecipientsAddress == applicationApiResponse.Candidate.Email
                && c.TemplateId == emailEnvironmentHelper.SubmitApplicationEmailTemplateId
                && c.Tokens["firstName"] == applicationApiResponse.Candidate.FirstName
                && c.Tokens["vacancy"] == vacancyResponse.Title
                && c.Tokens["employer"] == vacancyResponse.EmployerName 
                && c.Tokens["city"] == expectedAddress
                && c.Tokens["postcode"] == vacancyResponse.Address.Postcode
                && !string.IsNullOrEmpty(c.Tokens["yourApplicationsURL"])
                )
            ), Times.Once);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_ApplicationStatus_Is_Not_Updated_If_Not_Successfully_Submitted(
        string vacancyReference,
        SubmitApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        SubmitApplicationCommandHandler handler)
    {
        var expectedGetApplicationRequest =
            new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true);
        applicationApiResponse.VacancyReference = vacancyReference;
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(c => 
                    c.GetUrl == expectedGetApplicationRequest.GetUrl
                )))
            .ReturnsAsync(applicationApiResponse);
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.Is<PostSubmitApplicationRequest>(c => c.PostUrl.Contains(request.CandidateId.ToString())), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(null!, HttpStatusCode.InternalServerError, "An error Occurred"));

        var actual = await handler.Handle(request, CancellationToken.None);

        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never);
        actual.Should().BeFalse();
        notificationService.Verify(x => x.Send(
            It.IsAny<SendEmailCommand>()), Times.Never());
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_ApplicationStatus_Is_Not_Updated_And_Not_Submitted_To_Recruit_If_Application_Is_Not_Found(
        SubmitApplicationCommand request,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        SubmitApplicationCommandHandler handler)
    {
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.IsAny<GetApplicationApiRequest>()))
            .ReturnsAsync((GetApplicationApiResponse)null!);
        
        var actual = await handler.Handle(request, CancellationToken.None);
        
        recruitApiClient
            .Verify(x => x.PostWithResponseCode<NullResponse>(
                It.IsAny<PostSubmitApplicationRequest>(), true), Times.Never);
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never);
        actual.Should().BeFalse();
        notificationService.Verify(x => x.Send(
            It.IsAny<SendEmailCommand>()), Times.Never());
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Application_Is_Not_Submitted_To_Recruit_If_Application_Is_Already_Submitted(
        GetApplicationApiResponse applicationApiResponse,
        SubmitApplicationCommand request,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        SubmitApplicationCommandHandler handler)
    {
        applicationApiResponse.Status = "Submitted";
        var expectedGetApplicationRequest =
            new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true);
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(c => 
                    c.GetUrl == expectedGetApplicationRequest.GetUrl
                )))
            .ReturnsAsync(applicationApiResponse);
        
        var actual = await handler.Handle(request, CancellationToken.None);
        
        recruitApiClient
            .Verify(x => x.PostWithResponseCode<NullResponse>(
                It.IsAny<PostSubmitApplicationRequest>(), true), Times.Never);
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never);
        actual.Should().BeFalse();
        notificationService.Verify(x => x.Send(
            It.IsAny<SendEmailCommand>()), Times.Never());
    }
}