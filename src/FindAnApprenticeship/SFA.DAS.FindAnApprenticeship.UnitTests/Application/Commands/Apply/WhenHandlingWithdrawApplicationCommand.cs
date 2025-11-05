using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.WithdrawApplication;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitV2Api.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply;

public class WhenHandlingWithdrawApplicationCommand
{
    [Test]
    [MoqInlineAutoData("address1", "address2", "address3", "address4", "postcode", "address4 (postcode)", AvailableWhere.OneLocation)]
    [MoqInlineAutoData("address1", "address2", "address3", null, "postcode", "address3 (postcode)", AvailableWhere.OneLocation)]
    [MoqInlineAutoData("address1", "address2", null, null, "postcode", "address2 (postcode)", AvailableWhere.OneLocation)]
    [MoqInlineAutoData("address1", null, null, null, "postcode", "address1 (postcode)", AvailableWhere.OneLocation)]
    [MoqInlineAutoData("address1", "address2", "address3", "address4", "postcode", "Recruiting nationally", AvailableWhere.AcrossEngland)]
    [MoqInlineAutoData("address1", "address2", "address3", null, "postcode", "Recruiting nationally", AvailableWhere.AcrossEngland)]
    [MoqInlineAutoData("address1", "address2", null, null, "postcode", "Recruiting nationally", AvailableWhere.AcrossEngland)]
    [MoqInlineAutoData("address1", null, null, null, "postcode", "Recruiting nationally", AvailableWhere.AcrossEngland)]
    public async Task Then_The_Application_Is_Withdrawn_From_Recruit_Status_Updated_And_Email_Sent(
        string address1,
        string address2,
        string address3,
        string address4,
        string postcode,
        string expectedAddress,
        AvailableWhere employerLocationOption,
        long vacancyRef,
        WithdrawApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        GetApprenticeshipVacancyItemResponse vacancyResponse,
        ApplicationReview applicationReview,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitApiV2Client,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Frozen] Mock<IVacancyService> vacancyService,
        WithdrawApplicationCommandHandler handler)
    {
        vacancyResponse.Address.AddressLine1 = address1;
        vacancyResponse.Address.AddressLine2 = address2;
        vacancyResponse.Address.AddressLine3 = address3;
        vacancyResponse.Address.AddressLine4 = address4;
        vacancyResponse.Address.Postcode = postcode;
        vacancyResponse.EmployerLocationOption = employerLocationOption;

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

        recruitApiV2Client.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchRecruitApplicationReviewApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.OK, null));
        recruitApiV2Client.Setup(x => x.GetWithResponseCode<ApplicationReview>(It.Is<GetApplicationReviewByApplicationIdRequest>(c=>c.GetUrl.Contains(request.ApplicationId.ToString()))))
            .ReturnsAsync(new ApiResponse<ApplicationReview>(applicationReview, HttpStatusCode.OK, null));
        vacancyService.Setup(x => x.GetVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync(vacancyResponse);
        vacancyService.Setup(x => x.GetVacancyWorkLocation(vacancyResponse, true)).Returns(expectedAddress);

        var actual = await handler.Handle(request, CancellationToken.None);

        actual.Should().BeTrue();
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
            c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/Status" &&
            (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Withdrawn
        )), Times.Once);
        notificationService.Verify(x=>x.Send(
            It.Is<SendEmailCommand>(c=>
                c.RecipientsAddress == applicationApiResponse.Candidate.Email
                && c.TemplateId == emailEnvironmentHelper.WithdrawApplicationEmailTemplateId
                && c.Tokens["firstName"] == applicationApiResponse.Candidate.FirstName
                && c.Tokens["vacancy"] == vacancyResponse.Title
                && c.Tokens["employer"] == vacancyResponse.EmployerName 
                && c.Tokens["location"] == $"{expectedAddress}"
            )
        ), Times.Once);
        recruitApiV2Client.Verify(x => x.PatchWithResponseCode(It.Is<PatchRecruitApplicationReviewApiRequest>(c =>
            c.PatchUrl.Contains(applicationReview.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/WithdrawnDate" &&
            DateTime.Parse(c.Data.Operations[0].value.ToString()!).Date == DateTime.UtcNow.Date
        )), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Already_Withdrawn_Not_Submitted(
        long vacancyRef,
        WithdrawApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitApiV2Client,
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
        recruitApiV2Client.Verify(x => x.PatchWithResponseCode(It.Is<PatchRecruitApplicationReviewApiRequest>(c =>
            c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/WithdrawnDate" &&
            DateTime.Parse(c.Data.Operations[0].value.ToString()!).Date == DateTime.UtcNow.Date
        )), Times.Never);
        notificationService.Verify(x => x.Send(
            It.IsAny<SendEmailCommand>()), Times.Never());
    }

    [Test, MoqAutoData]
    public async Task Then_If_Not_Found_Then_Not_Submitted(
        WithdrawApplicationCommand request,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitApiV2Client,
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
        recruitApiV2Client.Verify(x => x.PatchWithResponseCode(It.Is<PatchRecruitApplicationReviewApiRequest>(c =>
            c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/WithdrawnDate" &&
            DateTime.Parse(c.Data.Operations[0].value.ToString()!).Date == DateTime.UtcNow.Date
        )), Times.Never);
        notificationService.Verify(x => x.Send(
            It.IsAny<SendEmailCommand>()), Times.Never());
    }

    [Test]
    [MoqInlineAutoData("address1", "address2", "address3", "address4", "postcode", "address4 (postcode)", AvailableWhere.OneLocation)]
    [MoqInlineAutoData("address1", "address2", "address3", null, "postcode", "address3 (postcode)", AvailableWhere.OneLocation)]
    [MoqInlineAutoData("address1", "address2", null, null, "postcode", "address2 (postcode)", AvailableWhere.OneLocation)]
    [MoqInlineAutoData("address1", null, null, null, "postcode", "address1 (postcode)", AvailableWhere.OneLocation)]
    [MoqInlineAutoData("address1", "address2", "address3", "address4", "postcode", "Recruiting nationally", AvailableWhere.AcrossEngland)]
    [MoqInlineAutoData("address1", "address2", "address3", null, "postcode", "Recruiting nationally", AvailableWhere.AcrossEngland)]
    [MoqInlineAutoData("address1", "address2", null, null, "postcode", "Recruiting nationally", AvailableWhere.AcrossEngland)]
    [MoqInlineAutoData("address1", null, null, null, "postcode", "Recruiting nationally", AvailableWhere.AcrossEngland)]
    public async Task Then_The_Vacancy_Is_Closed_Application_Status_Updated_And_Email_Sent(
        string address1,
        string address2,
        string address3,
        string address4,
        string postcode,
        string expectedAddress,
        AvailableWhere employerLocationOption,
        long vacancyRef,
        WithdrawApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        GetClosedVacancyResponse closedVacancyResponse,
        ApplicationReview applicationReview,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitApiV2Client,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Frozen] Mock<IVacancyService> vacancyService,
        WithdrawApplicationCommandHandler handler)
    {
        closedVacancyResponse.Address.AddressLine1 = address1;
        closedVacancyResponse.Address.AddressLine2 = address2;
        closedVacancyResponse.Address.AddressLine3 = address3;
        closedVacancyResponse.Address.AddressLine4 = address4;
        closedVacancyResponse.Address.Postcode = postcode;
        closedVacancyResponse.EmployerLocationOption = employerLocationOption;
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
        candidateApiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>())).ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.Accepted, ""));
        recruitApiV2Client.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchRecruitApplicationReviewApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.OK, null));
        recruitApiV2Client.Setup(x => x.GetWithResponseCode<ApplicationReview>(It.Is<GetApplicationReviewByApplicationIdRequest>(c=>c.GetUrl.Contains(request.ApplicationId.ToString()))))
            .ReturnsAsync(new ApiResponse<ApplicationReview>(applicationReview, HttpStatusCode.OK, null));
        vacancyService.Setup(x => x.GetVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync((GetApprenticeshipVacancyItemResponse)null!);
        vacancyService.Setup(x => x.GetClosedVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync(closedVacancyResponse);
        vacancyService.Setup(x => x.GetVacancyWorkLocation(closedVacancyResponse, true)).Returns(expectedAddress);

        var actual = await handler.Handle(request, CancellationToken.None);

        actual.Should().BeTrue();
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
            c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/Status" &&
            (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Withdrawn
        )), Times.Once);
        notificationService.Verify(x => x.Send(
            It.Is<SendEmailCommand>(c =>
                c.RecipientsAddress == applicationApiResponse.Candidate.Email
                && c.TemplateId == emailEnvironmentHelper.WithdrawApplicationEmailTemplateId
                && c.Tokens["firstName"] == applicationApiResponse.Candidate.FirstName
                && c.Tokens["vacancy"] == closedVacancyResponse.Title
                && c.Tokens["employer"] == closedVacancyResponse.EmployerName
                && c.Tokens["location"] == $"{expectedAddress}"
            )
        ), Times.Once);
        recruitApiV2Client.Verify(x => x.PatchWithResponseCode(It.Is<PatchRecruitApplicationReviewApiRequest>(c =>
                    c.PatchUrl.Contains(applicationReview.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.Data.Operations[0].path == "/WithdrawnDate" &&
                    DateTime.Parse(c.Data.Operations[0].value.ToString()!).Date == DateTime.UtcNow.Date
                )), Times.Once);
    }

    [Test]
    [MoqAutoData]
    public async Task Then_The_Vacancy_With_MultipleLocations_Is_Closed_Application_Status_Updated_And_Email_Sent(
        List<Address> addresses,
        long vacancyRef,
        WithdrawApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        GetClosedVacancyResponse closedVacancyResponse,
        ApplicationReview applicationReview,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitApiV2Client,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Frozen] Mock<IVacancyService> vacancyService,
        WithdrawApplicationCommandHandler handler)
    {
        const string expectedAddress = "City1, City2, City3";
        closedVacancyResponse.EmployerLocations = addresses;
        closedVacancyResponse.EmployerLocationOption = AvailableWhere.MultipleLocations;
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
        candidateApiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>())).ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.Accepted, ""));
        recruitApiV2Client.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchRecruitApplicationReviewApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.OK, null));
        recruitApiV2Client.Setup(x => x.GetWithResponseCode<ApplicationReview>(It.Is<GetApplicationReviewByApplicationIdRequest>(c=>c.GetUrl.Contains(request.ApplicationId.ToString()))))
            .ReturnsAsync(new ApiResponse<ApplicationReview>(applicationReview, HttpStatusCode.OK, null));
        vacancyService.Setup(x => x.GetVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync((GetApprenticeshipVacancyItemResponse)null!);
        vacancyService.Setup(x => x.GetClosedVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync(closedVacancyResponse);
        vacancyService.Setup(x => x.GetVacancyWorkLocation(closedVacancyResponse, true)).Returns(expectedAddress);

        var actual = await handler.Handle(request, CancellationToken.None);

        actual.Should().BeTrue();
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
            c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/Status" &&
            (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Withdrawn
        )), Times.Once);
        notificationService.Verify(x => x.Send(
            It.Is<SendEmailCommand>(c =>
                c.RecipientsAddress == applicationApiResponse.Candidate.Email
                && c.TemplateId == emailEnvironmentHelper.WithdrawApplicationEmailTemplateId
                && c.Tokens["firstName"] == applicationApiResponse.Candidate.FirstName
                && c.Tokens["vacancy"] == closedVacancyResponse.Title
                && c.Tokens["employer"] == closedVacancyResponse.EmployerName
                && c.Tokens["location"] == $"{expectedAddress}"
            )
        ), Times.Once);
    }

    [Test]
    [MoqAutoData]
    public async Task Then_The_Vacancy_With_Anon_ML_Is_Closed_Application_Status_Updated_And_Email_Sent(
        long vacancyRef,
        WithdrawApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        GetClosedVacancyResponse closedVacancyResponse,
        ApplicationReview applicationReview,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitApiV2Client,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        [Frozen] Mock<IVacancyService> vacancyService,
        WithdrawApplicationCommandHandler handler)
    {
        const string expectedAddress = "Leeds and 2 other available locations";
        closedVacancyResponse.EmployerLocations =
        [
            new Address {AddressLine3 = "Leeds", Postcode = "LS6"},
            new Address {AddressLine3 = "Leeds", Postcode = "LS6"},
            new Address {AddressLine3 = "Leeds", Postcode = "LS16"},
            new Address {AddressLine3 = "Leeds", Postcode = "LS9"},
            new Address {AddressLine3 = "Leeds", Postcode = "LS9"}
        ];
        closedVacancyResponse.EmployerLocationOption = AvailableWhere.MultipleLocations;
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
        candidateApiClient.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>())).ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.Accepted, ""));
        
        recruitApiV2Client.Setup(x => x.PatchWithResponseCode(It.IsAny<PatchRecruitApplicationReviewApiRequest>()))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.OK, null));
        recruitApiV2Client.Setup(x => x.GetWithResponseCode<ApplicationReview>(It.Is<GetApplicationReviewByApplicationIdRequest>(c=>c.GetUrl.Contains(request.ApplicationId.ToString()))))
            .ReturnsAsync(new ApiResponse<ApplicationReview>(applicationReview, HttpStatusCode.OK, null));
        vacancyService.Setup(x => x.GetVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync((GetApprenticeshipVacancyItemResponse)null!);
        vacancyService.Setup(x => x.GetClosedVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync(closedVacancyResponse);
        vacancyService.Setup(x => x.GetVacancyWorkLocation(closedVacancyResponse, true)).Returns(expectedAddress);
        
        var actual = await handler.Handle(request, CancellationToken.None);

        actual.Should().BeTrue();
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
            c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/Status" &&
            (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Withdrawn
        )), Times.Once);
        notificationService.Verify(x => x.Send(
            It.Is<SendEmailCommand>(c =>
                c.RecipientsAddress == applicationApiResponse.Candidate.Email
                && c.TemplateId == emailEnvironmentHelper.WithdrawApplicationEmailTemplateId
                && c.Tokens["firstName"] == applicationApiResponse.Candidate.FirstName
                && c.Tokens["vacancy"] == closedVacancyResponse.Title
                && c.Tokens["employer"] == closedVacancyResponse.EmployerName
                && c.Tokens["location"] == $"{expectedAddress}"
            )
        ), Times.Once);
        recruitApiV2Client.Verify(x => x.PatchWithResponseCode(It.Is<PatchRecruitApplicationReviewApiRequest>(c =>
            c.PatchUrl.Contains(applicationReview.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/WithdrawnDate" &&
            DateTime.Parse(c.Data.Operations[0].value.ToString()!).Date == DateTime.UtcNow.Date
        )), Times.Once);
    }
}