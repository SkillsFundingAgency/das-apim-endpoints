using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.WithdrawApplication;
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

public class WhenHandlingWithdrawApplicationCommand
{
    [Test]
    [MoqInlineAutoData("address1","address2","address3","address4","address4")]
    [MoqInlineAutoData("address1","address2","address3",null,"address3")]
    [MoqInlineAutoData("address1","address2",null,null,"address2")]
    [MoqInlineAutoData("address1",null,null,null,"address1")]
    public async Task Then_The_Application_Is_Withdrawn_From_Recruit_Status_Updated_And_Email_Sent(
        string address1,
        string address2,
        string address3,
        string address4,
        string expectedAddress,
        long vacancyRef,
        WithdrawApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        GetApprenticeshipVacancyItemResponse vacancyResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Frozen] Mock<IVacancyService> vacancyService,
        WithdrawApplicationCommandHandler handler)
    {
        vacancyResponse.Address.AddressLine1 = address1;
        vacancyResponse.Address.AddressLine2 = address2;
        vacancyResponse.Address.AddressLine3 = address3;
        vacancyResponse.Address.AddressLine4 = address4;
        applicationApiResponse.VacancyReference = $"VAC{vacancyRef}";
        applicationApiResponse.Status = ApplicationStatus.Submitted;
        var expectedGetApplicationRequest =
            new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true);
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(c => 
                    c.GetUrl == expectedGetApplicationRequest.GetUrl
                )))
            .ReturnsAsync(applicationApiResponse);
        candidateApiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>())).ReturnsAsync(new ApiResponse<string>("",HttpStatusCode.Accepted,""));
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.IsAny<PostWithdrawApplicationRequest>(), false)).ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.NoContent, ""));
        vacancyService.Setup(x => x.GetVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync(vacancyResponse);

        var actual = await handler.Handle(request, CancellationToken.None);

        actual.Should().BeTrue();
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
            c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/Status" &&
            (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Withdrawn
        )), Times.Once);
        recruitApiClient
            .Verify(x => x.PostWithResponseCode<NullResponse>(
                It.Is<PostWithdrawApplicationRequest>(c => 
                    c.PostUrl.Contains(request.CandidateId.ToString())
                    && c.PostUrl.Contains(vacancyRef.ToString())
                ), false), Times.Once);
        notificationService.Verify(x=>x.Send(
            It.Is<SendEmailCommand>(c=>
                c.RecipientsAddress == applicationApiResponse.Candidate.Email
                && c.TemplateId == emailEnvironmentHelper.WithdrawApplicationEmailTemplateId
                && c.Tokens["firstName"] == applicationApiResponse.Candidate.FirstName
                && c.Tokens["vacancy"] == vacancyResponse.Title
                && c.Tokens["employer"] == vacancyResponse.EmployerName 
                && c.Tokens["city"] == expectedAddress
                && c.Tokens["postcode"] == vacancyResponse.Address.Postcode
            )
        ), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Already_Withdrawn_Not_Submitted(
        long vacancyRef,
        WithdrawApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        WithdrawApplicationCommandHandler handler)
    {
        applicationApiResponse.VacancyReference = $"VAC{vacancyRef}";
        applicationApiResponse.Status = ApplicationStatus.Withdrawn;
        var expectedGetApplicationRequest =
            new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true);
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(c => 
                    c.GetUrl == expectedGetApplicationRequest.GetUrl
                )))
            .ReturnsAsync(applicationApiResponse);
        
        var actual = await handler.Handle(request, CancellationToken.None);

        actual.Should().BeFalse();
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never());
        recruitApiClient
            .Verify(x => x.PostWithResponseCode<NullResponse>(
                It.IsAny<PostWithdrawApplicationRequest>(), false), Times.Never);
        notificationService.Verify(x => x.Send(
            It.IsAny<SendEmailCommand>()), Times.Never());

    }

    [Test, MoqAutoData]
    public async Task Then_If_Not_Found_Then_Not_Submitted(
        WithdrawApplicationCommand request,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        WithdrawApplicationCommandHandler handler)
    {
        var expectedGetApplicationRequest =
            new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true);
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(c => 
                    c.GetUrl == expectedGetApplicationRequest.GetUrl
                )))!
            .ReturnsAsync((GetApplicationApiResponse)null!);
        
        var actual = await handler.Handle(request, CancellationToken.None);

        actual.Should().BeFalse();
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never());
        recruitApiClient
            .Verify(x => x.PostWithResponseCode<NullResponse>(
                It.IsAny<PostWithdrawApplicationRequest>(), false), Times.Never);
        notificationService.Verify(x => x.Send(
            It.IsAny<SendEmailCommand>()), Times.Never());
    }

    [Test, MoqAutoData]
    public async Task Then_If_Not_Successful_To_Recruit_Returns_False(
        long vacancyRef,
        WithdrawApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        WithdrawApplicationCommandHandler handler)
    {
        applicationApiResponse.VacancyReference = $"VAC{vacancyRef}";
        applicationApiResponse.Status = ApplicationStatus.Submitted;
        var expectedGetApplicationRequest =
            new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true);
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(c => 
                    c.GetUrl == expectedGetApplicationRequest.GetUrl
                )))
            .ReturnsAsync(applicationApiResponse);
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.IsAny<PostWithdrawApplicationRequest>(), false)).ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.InternalServerError, ""));

        
        var actual = await handler.Handle(request, CancellationToken.None);

        actual.Should().BeFalse();
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never());
        notificationService.Verify(x => x.Send(
            It.IsAny<SendEmailCommand>()), Times.Never());
    }
}