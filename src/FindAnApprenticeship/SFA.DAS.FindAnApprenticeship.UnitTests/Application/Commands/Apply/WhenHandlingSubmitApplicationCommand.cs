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
using System.Net;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitV2Api.Requests;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply;

public class WhenHandlingSubmitApplicationCommand
{
    [Test, MoqAutoData]
    public async Task Then_The_Application_Is_Submitted_To_Recruit(
        
        SubmitApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        GetApprenticeshipVacancyItemResponse vacancyResponse,
        [Frozen] Mock<IVacancyService> vacancyService,
        [Frozen] Mock<IMetrics> metricsService,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitV2ApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        SubmitApplicationCommandHandler handler)
    {
        // arrange
        const string vacancyReference = "VAC999999999";
        const int ukprn = 1111111;
        vacancyResponse.Ukprn = ukprn.ToString();
        vacancyResponse.VacancyReference = vacancyReference;
        var expectedGetApplicationRequestUrl = new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, true).GetUrl;
        applicationApiResponse.Status = ApplicationStatus.Draft;
        applicationApiResponse.VacancyReference = vacancyReference;
        
        candidateApiClient
            .Setup(x => x.Get<GetApplicationApiResponse>(It.IsAny<GetApplicationApiRequest>()))
            .ReturnsAsync(applicationApiResponse);
        
        candidateApiClient
            .Setup(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()))
            .ReturnsAsync(() => new ApiResponse<string>("", HttpStatusCode.OK, ""));
        
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(It.IsAny<PostSubmitApplicationRequest>(), false))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.NoContent, ""));
        
        vacancyService.Setup(x => x.GetVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync(vacancyResponse);

        recruitV2ApiClient
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<CreateApplicationReviewRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.Created, ""));
        
        // act
        var actual = await handler.Handle(request, CancellationToken.None);
        
        // assert
        candidateApiClient.Verify(x => x.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequestUrl)), Times.Once);
        
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
            c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/Status" &&
            (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Submitted
            )), Times.Once);
        
        recruitApiClient.Verify(x => x.PostWithResponseCode<NullResponse>(It.Is<PostSubmitApplicationRequest>(c =>
            c.PostUrl.Contains(request.CandidateId.ToString())
            && ((PostSubmitApplicationRequestData)c.Data).VacancyReference == vacancyReference
            && ((PostSubmitApplicationRequestData)c.Data).MigrationDate.Date == DateTime.UtcNow.Date
            ), false), Times.Once);
        
        recruitV2ApiClient.Verify(x => x.PutWithResponseCode<NullResponse>(It.IsAny<CreateApplicationReviewRequest>()), Times.Once);
         recruitV2ApiClient.Verify(x => x.PutWithResponseCode<NullResponse>(It.Is<CreateApplicationReviewRequest>(r =>
             ((CreateApplicationReviewRequestData)r.Data).CandidateId == applicationApiResponse.CandidateId 
             && ((CreateApplicationReviewRequestData)r.Data).ApplicationId == applicationApiResponse.Id 
             && ((CreateApplicationReviewRequestData)r.Data).AccountId == vacancyResponse.AccountId 
             && ((CreateApplicationReviewRequestData)r.Data).AccountLegalEntityId == vacancyResponse.AccountLegalEntityId 
             && ((CreateApplicationReviewRequestData)r.Data).Ukprn == ukprn  
             && ((CreateApplicationReviewRequestData)r.Data).VacancyReference == 999999999  
             && ((CreateApplicationReviewRequestData)r.Data).VacancyTitle == vacancyResponse.Title  
             && ((CreateApplicationReviewRequestData)r.Data).AdditionalQuestion1 == vacancyResponse.AdditionalQuestion1  
             && ((CreateApplicationReviewRequestData)r.Data).AdditionalQuestion2 == vacancyResponse.AdditionalQuestion2  
             && ((CreateApplicationReviewRequestData)r.Data).SubmittedDate.Date == DateTime.UtcNow.Date   
             )), Times.Once);
        
        metricsService.Verify(x => x.IncreaseVacancySubmitted(It.IsAny<string>(), 1), Times.Once);
        
        actual.Should().BeTrue();
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
    public async Task Then_The_ApplicationStatus_Is_Updated_If_Submitted_To_Recruit_And_Notification_Sent(
        string address1,
        string address2,
        string address3,
        string address4,
        string postcode,
        string expectedAddress,
        AvailableWhere employerLocationOption,
        SubmitApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        GetApprenticeshipVacancyItemResponse vacancyResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IMetrics> metricsService,
        [Frozen] Mock<IVacancyService> vacancyService,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitV2ApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        SubmitApplicationCommandHandler handler)
    {
        const string vacancyReference = "VAC999999999";
        const int ukprn = 1111111;
        vacancyResponse.VacancyReference = vacancyReference;
        vacancyResponse.Ukprn = ukprn.ToString();
        
        vacancyResponse.Address.AddressLine1 = address1;
        vacancyResponse.Address.AddressLine2 = address2;
        vacancyResponse.Address.AddressLine3 = address3;
        vacancyResponse.Address.AddressLine4 = address4;
        vacancyResponse.Address.Postcode = postcode;
        vacancyResponse.EmployerLocationOption = employerLocationOption;
        
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
        
        recruitV2ApiClient
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<CreateApplicationReviewRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.Created, ""));

        vacancyService.Setup(x => x.GetVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync(vacancyResponse);
        vacancyService.Setup(x => x.GetVacancyWorkLocation(vacancyResponse, true)).Returns(expectedAddress);


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
                && c.Tokens["location"] == $"{expectedAddress}"
                && !string.IsNullOrEmpty(c.Tokens["yourApplicationsURL"])
                )
            ), Times.Once);
        metricsService.Verify(x => x.IncreaseVacancySubmitted(It.IsAny<string>(), 1), Times.Once);
    }

    [Test]
    [MoqAutoData]
    public async Task Then_The_Vacancy_With_ML_ApplicationStatus_Is_Updated_If_Submitted_To_Recruit_And_Notification_Sent(
        List<Address> addresses,
        SubmitApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        GetApprenticeshipVacancyItemResponse vacancyResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IMetrics> metricsService,
        [Frozen] Mock<IVacancyService> vacancyService,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitV2ApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        SubmitApplicationCommandHandler handler)
    {
        const string vacancyReference = "VAC999999999";
        const int ukprn = 1111111;
        vacancyResponse.Ukprn = ukprn.ToString();
        vacancyResponse.VacancyReference = vacancyReference;
        const string expectedAddress = "City1, City2, City3";
        vacancyResponse.OtherAddresses = addresses;
        vacancyResponse.EmployerLocationOption = AvailableWhere.MultipleLocations;
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
        ))).ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.Accepted, ""));
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.Is<PostSubmitApplicationRequest>(c =>
                    c.PostUrl.Contains(request.CandidateId.ToString())
                ), false)).ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.NoContent, ""));
        
        recruitV2ApiClient
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<CreateApplicationReviewRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.Created, ""));

        vacancyService.Setup(x => x.GetVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync(vacancyResponse);
        vacancyService.Setup(x => x.GetVacancyWorkLocation(vacancyResponse, true)).Returns(expectedAddress);


        var actual = await handler.Handle(request, CancellationToken.None);

        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
            c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/Status" &&
            (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Submitted
            )), Times.Once
        );
        actual.Should().BeTrue();
        notificationService.Verify(x => x.Send(
            It.Is<SendEmailCommand>(c =>
                c.RecipientsAddress == applicationApiResponse.Candidate.Email
                && c.TemplateId == emailEnvironmentHelper.SubmitApplicationEmailTemplateId
                && c.Tokens["firstName"] == applicationApiResponse.Candidate.FirstName
                && c.Tokens["vacancy"] == vacancyResponse.Title
                && c.Tokens["employer"] == vacancyResponse.EmployerName
                && c.Tokens["location"] == $"{expectedAddress}"
                && !string.IsNullOrEmpty(c.Tokens["yourApplicationsURL"])
                )
            ), Times.Once);
        metricsService.Verify(x => x.IncreaseVacancySubmitted(It.IsAny<string>(), 1), Times.Once);
    }

    [Test]
    [MoqAutoData]
    public async Task Then_The_Vacancy_With_Anon_ML_ApplicationStatus_Is_Updated_If_Submitted_To_Recruit_And_Notification_Sent(
        SubmitApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        GetApprenticeshipVacancyItemResponse vacancyResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IMetrics> metricsService,
        [Frozen] Mock<IVacancyService> vacancyService,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<IRecruitApiClient<RecruitApiV2Configuration>> recruitV2ApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        SubmitApplicationCommandHandler handler)
    {
        const string vacancyReference = "VAC999999999";
        const int ukprn = 1111111;
        vacancyResponse.Ukprn = ukprn.ToString();
        vacancyResponse.VacancyReference = vacancyReference;
        const string expectedAddress = "Leeds and 2 other available locations";
        vacancyResponse.OtherAddresses =
        [
            new Address {AddressLine3 = "Leeds", Postcode = "LS6"},
            new Address {AddressLine3 = "Leeds", Postcode = "LS6"},
            new Address {AddressLine3 = "Leeds", Postcode = "LS16"},
            new Address {AddressLine3 = "Leeds", Postcode = "LS9"},
            new Address {AddressLine3 = "Leeds", Postcode = "LS9"}
        ];
        vacancyResponse.EmployerLocationOption = AvailableWhere.MultipleLocations;
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
        ))).ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.Accepted, ""));
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.Is<PostSubmitApplicationRequest>(c =>
                    c.PostUrl.Contains(request.CandidateId.ToString())
                ), false)).ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.NoContent, ""));
        
        recruitV2ApiClient
            .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<CreateApplicationReviewRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.Created, ""));

        vacancyService.Setup(x => x.GetVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync(vacancyResponse);
        vacancyService.Setup(x => x.GetVacancyWorkLocation(vacancyResponse, true)).Returns(expectedAddress);


        var actual = await handler.Handle(request, CancellationToken.None);

        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
            c.PatchUrl.Contains(request.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.PatchUrl.Contains(request.CandidateId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
            c.Data.Operations[0].path == "/Status" &&
            (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Submitted
            )), Times.Once
        );
        actual.Should().BeTrue();
        notificationService.Verify(x => x.Send(
            It.Is<SendEmailCommand>(c =>
                c.RecipientsAddress == applicationApiResponse.Candidate.Email
                && c.TemplateId == emailEnvironmentHelper.SubmitApplicationEmailTemplateId
                && c.Tokens["firstName"] == applicationApiResponse.Candidate.FirstName
                && c.Tokens["vacancy"] == vacancyResponse.Title
                && c.Tokens["employer"] == vacancyResponse.EmployerName
                && c.Tokens["location"] == $"{expectedAddress}"
                && !string.IsNullOrEmpty(c.Tokens["yourApplicationsURL"])
                )
            ), Times.Once);
        metricsService.Verify(x => x.IncreaseVacancySubmitted(It.IsAny<string>(), 1), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_The_ApplicationStatus_Is_Not_Updated_If_Not_Successfully_Submitted(
        string vacancyReference,
        SubmitApplicationCommand request,
        GetApplicationApiResponse applicationApiResponse,
        [Frozen] Mock<IMetrics> metricsService,
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
        metricsService.Verify(x => x.IncreaseVacancySubmitted(It.IsAny<string>(), 1), Times.Never);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_ApplicationStatus_Is_Not_Updated_And_Not_Submitted_To_Recruit_If_Application_Is_Not_Found(
        SubmitApplicationCommand request,
        [Frozen] Mock<IMetrics> metricsService,
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
        metricsService.Verify(x => x.IncreaseVacancySubmitted(It.IsAny<string>(), 1), Times.Never);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Application_Is_Not_Submitted_To_Recruit_If_Application_Is_Already_Submitted(
        GetApplicationApiResponse applicationApiResponse,
        SubmitApplicationCommand request,
        [Frozen] Mock<IMetrics> metricsService,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        SubmitApplicationCommandHandler handler)
    {
        applicationApiResponse.Status = ApplicationStatus.Submitted;
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
        metricsService.Verify(x => x.IncreaseVacancySubmitted(It.IsAny<string>(), 1), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Not_Found_Application_Is_Not_Submitted_Email_Not_Sent(
        GetApplicationApiResponse applicationApiResponse,
        SubmitApplicationCommand request,
        [Frozen] Mock<IVacancyService> vacancyService,
        [Frozen] Mock<IMetrics> metricsService,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        SubmitApplicationCommandHandler handler)
    {
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
        ))).ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.Accepted, ""));
        recruitApiClient
            .Setup(x => x.PostWithResponseCode<NullResponse>(
                It.Is<PostSubmitApplicationRequest>(c =>
                    c.PostUrl.Contains(request.CandidateId.ToString())
                ), false)).ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.NoContent, ""));

        vacancyService.Setup(x => x.GetVacancy(applicationApiResponse.VacancyReference)).ReturnsAsync((GetApprenticeshipVacancyItemResponse)null!);
        var actual = await handler.Handle(request, CancellationToken.None);

        recruitApiClient
            .Verify(x => x.PostWithResponseCode<NullResponse>(
                It.IsAny<PostSubmitApplicationRequest>(), true), Times.Never);
        candidateApiClient.Verify(x => x.PatchWithResponseCode(It.IsAny<PatchApplicationApiRequest>()), Times.Never);
        actual.Should().BeFalse();
        notificationService.Verify(x => x.Send(
            It.IsAny<SendEmailCommand>()), Times.Never());
        metricsService.Verify(x => x.IncreaseVacancySubmitted(It.IsAny<string>(), 1), Times.Never);
    }
}