using System.Net;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands;
using SFA.DAS.FindApprenticeshipJobs.Application.Shared;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Application;

public class WhenHandlingProcessVacancyClosedEarlyCommand
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
    public async Task Then_The_Vacancy_Candidates_Are_Found_Emails_Sent_And_Application_Status_Updated(
        string address1,
        string address2,
        string address3,
        string address4,
        string postcode,
        string expectedAddress,
        AvailableWhere employerLocationOption,
        ProcessVacancyClosedEarlyCommand command,
        GetCandidateApplicationApiResponse candidateApiResponseAll,
        GetLiveVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        ProcessVacancyClosedEarlyCommandHandler handler)
    {
        recruitApiResponse.EmployerLocationOption = employerLocationOption;

        recruitApiResponse.EmployerLocations[0].AddressLine1 = address1;
        recruitApiResponse.EmployerLocations[0].AddressLine2 = address2;
        recruitApiResponse.EmployerLocations[0].AddressLine3 = address3;
        recruitApiResponse.EmployerLocations[0].AddressLine4 = address4;
        recruitApiResponse.EmployerLocations[0].Postcode = postcode;

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
            notificationService.Verify(x => x.Send(
                It.Is<SendEmailCommand>(c =>
                    c.RecipientsAddress == candidate.Candidate.Email
                    && c.TemplateId == emailEnvironmentHelper.VacancyClosedEarlyTemplateId
                    && c.Tokens["firstName"] == candidate.Candidate.FirstName
                    && c.Tokens["vacancy"] == recruitApiResponse.Title
                    && c.Tokens["employer"] == recruitApiResponse.EmployerName
                    && c.Tokens["dateApplicationStarted"] == candidate.ApplicationCreatedDate.ToString("d MMM yyyy")
                    && c.Tokens["location"] == expectedAddress
                    && !string.IsNullOrEmpty(c.Tokens["vacancyUrl"])
                    && !string.IsNullOrEmpty(c.Tokens["settingsUrl"])
                )
            ), Times.Once);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_With_Multiple_Locations_And_Candidates_Are_Found_Emails_Sent_And_Application_Status_Updated(
        List<Address> addresses,
        ProcessVacancyClosedEarlyCommand command,
        GetCandidateApplicationApiResponse candidateApiResponseAll,
        GetLiveVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        ProcessVacancyClosedEarlyCommandHandler handler)
    {
       
        recruitApiResponse.EmployerLocationOption = AvailableWhere.MultipleLocations;
        recruitApiResponse.EmployerLocations = addresses;

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
            var employmentWorkLocation =
                EmailTemplateAddressExtension.GetEmploymentLocationCityNames(recruitApiResponse.EmployerLocations);

            candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                    c.PatchUrl.Contains(candidate.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.PatchUrl.Contains(candidate.Candidate.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.Data.Operations[0].path == "/Status" &&
                    (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Expired
                )), Times.Once

            );
            notificationService.Verify(x => x.Send(
                It.Is<SendEmailCommand>(c =>
                    c.RecipientsAddress == candidate.Candidate.Email
                    && c.TemplateId == emailEnvironmentHelper.VacancyClosedEarlyTemplateId
                    && c.Tokens["firstName"] == candidate.Candidate.FirstName
                    && c.Tokens["vacancy"] == recruitApiResponse.Title
                    && c.Tokens["employer"] == recruitApiResponse.EmployerName
                    && c.Tokens["dateApplicationStarted"] == candidate.ApplicationCreatedDate.ToString("d MMM yyyy")
                    && c.Tokens["location"] == employmentWorkLocation
                    && !string.IsNullOrEmpty(c.Tokens["vacancyUrl"])
                    && !string.IsNullOrEmpty(c.Tokens["settingsUrl"])
                )
            ), Times.Once);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_With_Anon_Multiple_Locations_And_Candidates_Are_Found_Emails_Sent_And_Application_Status_Updated(
        ProcessVacancyClosedEarlyCommand command,
        GetCandidateApplicationApiResponse candidateApiResponseAll,
        GetLiveVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        ProcessVacancyClosedEarlyCommandHandler handler)
    {
        recruitApiResponse.EmployerLocationOption = AvailableWhere.MultipleLocations;
        const string expectedAddress = "Leeds and 2 other available locations";
        recruitApiResponse.EmployerLocations =
        [
            new Address {AddressLine3 = "Leeds", Postcode = "LS6"},
            new Address {AddressLine3 = "Leeds", Postcode = "LS6"},
            new Address {AddressLine3 = "Leeds", Postcode = "LS16"},
            new Address {AddressLine3 = "Leeds", Postcode = "LS9"},
            new Address {AddressLine3 = "Leeds", Postcode = "LS9"}
        ];

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
            var employmentWorkLocation =
                EmailTemplateAddressExtension.GetEmploymentLocationCityNames(recruitApiResponse.EmployerLocations);

            candidateApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(c =>
                    c.PatchUrl.Contains(candidate.ApplicationId.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.PatchUrl.Contains(candidate.Candidate.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) &&
                    c.Data.Operations[0].path == "/Status" &&
                    (ApplicationStatus)c.Data.Operations[0].value == ApplicationStatus.Expired
                )), Times.Once

            );
            notificationService.Verify(x => x.Send(
                It.Is<SendEmailCommand>(c =>
                    c.RecipientsAddress == candidate.Candidate.Email
                    && c.TemplateId == emailEnvironmentHelper.VacancyClosedEarlyTemplateId
                    && c.Tokens["firstName"] == candidate.Candidate.FirstName
                    && c.Tokens["vacancy"] == recruitApiResponse.Title
                    && c.Tokens["employer"] == recruitApiResponse.EmployerName
                    && c.Tokens["dateApplicationStarted"] == candidate.ApplicationCreatedDate.ToString("d MMM yyyy")
                    && c.Tokens["location"] == employmentWorkLocation
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