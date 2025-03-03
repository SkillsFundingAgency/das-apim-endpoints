using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands;
using SFA.DAS.FindApprenticeshipJobs.Application.Shared;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Application;

public class WhenHandlingProcessApplicationReminder
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
    public async Task Then_The_Vacancy_Candidates_Are_Found_And_Emails_Sent(
        string address1,
        string address2,
        string address3,
        string address4,
        string postcode,
        string expectedAddress,
        AvailableWhere employerLocationOption,
        ProcessApplicationReminderCommand command,
        Preference candidatePreference,
        Preference candidatePreference1,
        GetCandidateApplicationApiResponse candidateApiResponse,
        GetLiveVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        ProcessApplicationReminderCommandHandler handler)
    {
        recruitApiResponse.EmployerLocationOption = employerLocationOption;

        recruitApiResponse.EmployerLocation.AddressLine1 = address1;
        recruitApiResponse.EmployerLocation.AddressLine2 = address2;
        recruitApiResponse.EmployerLocation.AddressLine3 = address3;
        recruitApiResponse.EmployerLocation.AddressLine4 = address4;
        recruitApiResponse.EmployerLocation.Postcode = postcode;


        candidatePreference.PreferenceType = "Closing";
        candidateApiClient
            .Setup(x => x.Get<GetCandidateApplicationApiResponse>(
                It.Is<GetCandidateApplicationsByVacancyRequest>(c =>
                    c.GetUrl.Contains(command.VacancyReference.ToString())
                    && c.GetUrl.Contains(candidatePreference.PreferenceId.ToString())))).ReturnsAsync(candidateApiResponse);
        candidateApiClient
            .Setup(x => x.Get<GetPreferencesApiResponse>(
                It.IsAny<GetCandidatePreferencesRequest>())).ReturnsAsync(new GetPreferencesApiResponse{Preferences =
                [candidatePreference, candidatePreference1]
            });
        recruitApiClient
            .Setup(x => x.Get<GetLiveVacancyApiResponse>(
                It.Is<GetLiveVacancyApiRequest>(c => 
                    c.GetUrl.Contains(command.VacancyReference.ToString()))))
            .ReturnsAsync(recruitApiResponse);

        await handler.Handle(command, CancellationToken.None);

        foreach (var candidate in candidateApiResponse.Candidates)
        {
            notificationService.Verify(x=>x.Send(
                It.Is<SendEmailCommand>(c=>
                    c.RecipientsAddress == candidate.Candidate.Email
                    && c.TemplateId == emailEnvironmentHelper.ApplicationReminderEmailTemplateId
                    && c.Tokens["firstName"] == candidate.Candidate.FirstName
                    && c.Tokens["daysRemaining"] == command.DaysUntilClosing.ToString()
                    && c.Tokens["vacancy"] == recruitApiResponse.Title
                    && c.Tokens["employer"] == recruitApiResponse.EmployerName 
                    && c.Tokens["location"] == $"{expectedAddress}"
                    && c.Tokens["closingDate"] == recruitApiResponse.ClosingDate.ToString("d MMM yyyy") 
                    && !string.IsNullOrEmpty(c.Tokens["continueApplicationLink"])
                    && !string.IsNullOrEmpty(c.Tokens["vacancyUrl"])
                    && !string.IsNullOrEmpty(c.Tokens["settingsUrl"])
                )
            ), Times.Once);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_With_Multiple_Locations_And_Candidates_Are_Found_And_Emails_Sent(
        List<Address> addresses,
        ProcessApplicationReminderCommand command,
        Preference candidatePreference,
        Preference candidatePreference1,
        GetCandidateApplicationApiResponse candidateApiResponse,
        GetLiveVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        ProcessApplicationReminderCommandHandler handler)
    {
        recruitApiResponse.EmployerLocationOption = AvailableWhere.MultipleLocations;
        recruitApiResponse.EmployerLocations = addresses;

        candidatePreference.PreferenceType = "Closing";
        candidateApiClient
            .Setup(x => x.Get<GetCandidateApplicationApiResponse>(
                It.Is<GetCandidateApplicationsByVacancyRequest>(c =>
                    c.GetUrl.Contains(command.VacancyReference.ToString())
                    && c.GetUrl.Contains(candidatePreference.PreferenceId.ToString())))).ReturnsAsync(candidateApiResponse);
        candidateApiClient
            .Setup(x => x.Get<GetPreferencesApiResponse>(
                It.IsAny<GetCandidatePreferencesRequest>())).ReturnsAsync(new GetPreferencesApiResponse
                {
                    Preferences =
                [candidatePreference, candidatePreference1]
                });
        recruitApiClient
            .Setup(x => x.Get<GetLiveVacancyApiResponse>(
                It.Is<GetLiveVacancyApiRequest>(c =>
                    c.GetUrl.Contains(command.VacancyReference.ToString()))))
            .ReturnsAsync(recruitApiResponse);

        await handler.Handle(command, CancellationToken.None);

        foreach (var candidate in candidateApiResponse.Candidates)
        {
            var employmentWorkLocation =
                EmailTemplateAddressExtension.GetEmploymentLocationCityNames(recruitApiResponse.EmployerLocations);

            notificationService.Verify(x => x.Send(
                It.Is<SendEmailCommand>(c =>
                    c.RecipientsAddress == candidate.Candidate.Email
                    && c.TemplateId == emailEnvironmentHelper.ApplicationReminderEmailTemplateId
                    && c.Tokens["firstName"] == candidate.Candidate.FirstName
                    && c.Tokens["daysRemaining"] == command.DaysUntilClosing.ToString()
                    && c.Tokens["vacancy"] == recruitApiResponse.Title
                    && c.Tokens["employer"] == recruitApiResponse.EmployerName
                    && c.Tokens["location"] == $"{employmentWorkLocation}"
                    && c.Tokens["closingDate"] == recruitApiResponse.ClosingDate.ToString("d MMM yyyy")
                    && !string.IsNullOrEmpty(c.Tokens["continueApplicationLink"])
                    && !string.IsNullOrEmpty(c.Tokens["vacancyUrl"])
                    && !string.IsNullOrEmpty(c.Tokens["settingsUrl"])
                )
            ), Times.Once);
        }
    }

    [Test, MoqAutoData]
    public async Task Then_If_No_Candidates_Are_Found_No_Emails_Sent(
        ProcessApplicationReminderCommand command,
        Preference candidatePreference,
        Preference candidatePreference1,
        GetLiveVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        ProcessApplicationReminderCommandHandler handler)
    {
        candidatePreference.PreferenceType = "CLOSING";
        candidateApiClient
            .Setup(x => x.Get<GetCandidateApplicationApiResponse>(
                It.Is<GetCandidateApplicationsByVacancyRequest>(c =>
                    c.GetUrl.Contains(command.VacancyReference.ToString())
                    && c.GetUrl.Contains(candidatePreference.PreferenceId.ToString())))).ReturnsAsync(new GetCandidateApplicationApiResponse{Candidates = []});
        candidateApiClient
            .Setup(x => x.Get<GetPreferencesApiResponse>(
                It.IsAny<GetCandidatePreferencesRequest>())).ReturnsAsync(new GetPreferencesApiResponse{Preferences =
                [candidatePreference, candidatePreference1]
            });
        recruitApiClient
            .Setup(x => x.Get<GetLiveVacancyApiResponse>(
                It.Is<GetLiveVacancyApiRequest>(c => 
                    c.GetUrl.Contains(command.VacancyReference.ToString()))))
            .ReturnsAsync(recruitApiResponse);

        await handler.Handle(command, CancellationToken.None);

        notificationService.Verify(x=>x.Send(
            It.IsAny<SendEmailCommand>()
        ), Times.Never);
        
    }
    
    [Test, MoqAutoData]
    public async Task Then_If_No_Preference_Is_Found_No_Emails_Sent(
        ProcessApplicationReminderCommand command,
        Preference candidatePreference,
        Preference candidatePreference1,
        GetCandidateApplicationApiResponse candidateApiResponse,
        GetLiveVacancyApiResponse recruitApiResponse,
        EmailEnvironmentHelper emailEnvironmentHelper,
        GetCandidateApplicationApiResponse candidateApplicationApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        ProcessApplicationReminderCommandHandler handler)
    {
        candidateApiClient
            .Setup(x => x.Get<GetPreferencesApiResponse>(
                It.IsAny<GetCandidatePreferencesRequest>())).ReturnsAsync(new GetPreferencesApiResponse{Preferences =
                [candidatePreference, candidatePreference1]
            });

        await handler.Handle(command, CancellationToken.None);

        notificationService.Verify(x=>x.Send(
            It.IsAny<SendEmailCommand>()
        ), Times.Never);
        recruitApiClient
            .Verify(x => x.Get<GetLiveVacancyApiResponse>(
                It.IsAny<GetLiveVacancyApiRequest>()), Times.Never);
        candidateApiClient
            .Verify(x => x.Get<GetCandidateApplicationApiResponse>(
                It.IsAny<GetCandidateApplicationsByVacancyRequest>()), Times.Never());
    }
}