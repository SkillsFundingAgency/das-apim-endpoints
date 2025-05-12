using AutoFixture.NUnit3;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using PatchSavedSearch = SFA.DAS.FindApprenticeshipJobs.Domain.Models.PatchSavedSearch;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.SavedSearches;

[TestFixture]
public class WhenHandlingPostSendSavedSearchNotificationCommand
{
    [Test]
    [MoqAutoData]
    public async Task Then_The_SavedSearch_Is_Patched_LastEmailSent_Updated_And_Email_Sent_And_ShowSearchLink_As_No_For_Five_Or_Less_Vacancies(
        PostSendSavedSearchNotificationCommand.Vacancy vacancy,
        PostSendSavedSearchNotificationCommand command,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        PostSendSavedSearchNotificationCommandHandler handler)
    {
        command.Vacancies = [vacancy,vacancy,vacancy,vacancy,vacancy];
        var expectedPatchSavedSearchApiRequest =
            new PatchSavedSearchApiRequest(command.Id, new JsonPatchDocument<PatchSavedSearch>());

        findApprenticeshipApiClient
            .Setup(x => x.PatchWithResponseCode(
                It.Is<PatchSavedSearchApiRequest>(c =>
                    c.PatchUrl == expectedPatchSavedSearchApiRequest.PatchUrl
                )))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, string.Empty));


        await handler.Handle(command, CancellationToken.None);

        findApprenticeshipApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchSavedSearchApiRequest>(c =>
            c.PatchUrl.Contains(command.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) 
            && c.Data.Operations[1].path == "/LastRunDate" 
            && DateTime.Parse(c.Data.Operations[1].value.ToString()).Date.Equals(DateTime.UtcNow.Date)
            && c.Data.Operations[0].path == "/EmailLastSendDate" 
            && DateTime.Parse(c.Data.Operations[0].value.ToString()).Date.Equals(DateTime.UtcNow.Date))), Times.Once);

        notificationService.Verify(x => x.Send(
            It.Is<SendEmailCommand>(c =>
                c.RecipientsAddress == command.User.Email
                && c.TemplateId == emailEnvironmentHelper.SavedSearchEmailNotificationTemplateId
                && c.Tokens["firstName"] == command.User.FirstName 
                && c.Tokens["newApprenticeships"] == $"{command.Vacancies.Count.ToString()} new {(command.Vacancies.Count == 1 ? "apprenticeship" : "apprenticeships")}"
                && c.Tokens["searchAlertDescriptor"] == $"{command.SearchTerm} in {command.Location}"
                && c.Tokens["showSearchLink"] == "no"
                && !string.IsNullOrEmpty(c.Tokens["unsubscribeLink"])
                && !string.IsNullOrEmpty(c.Tokens["searchUrl"])
                && !string.IsNullOrEmpty(c.Tokens["searchParams"])
            )
        ), Times.Once);
    }
    
    [Test]
    [MoqAutoData]
    public async Task Then_The_SavedSearch_Has_ShowSearchLink_When_More_Than_Five_Results(
        PostSendSavedSearchNotificationCommand.Vacancy vacancy,
        PostSendSavedSearchNotificationCommand command,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        PostSendSavedSearchNotificationCommandHandler handler)
    {
        command.Vacancies = [vacancy,vacancy,vacancy,vacancy,vacancy,vacancy];
        var expectedPatchSavedSearchApiRequest =
            new PatchSavedSearchApiRequest(command.Id, new JsonPatchDocument<PatchSavedSearch>());

        findApprenticeshipApiClient
            .Setup(x => x.PatchWithResponseCode(
                It.Is<PatchSavedSearchApiRequest>(c =>
                    c.PatchUrl == expectedPatchSavedSearchApiRequest.PatchUrl
                )))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, string.Empty));


        await handler.Handle(command, CancellationToken.None);

        findApprenticeshipApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchSavedSearchApiRequest>(c =>
            c.PatchUrl.Contains(command.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) 
            && c.Data.Operations[1].path == "/LastRunDate" 
            && DateTime.Parse(c.Data.Operations[1].value.ToString()).Date.Equals(DateTime.UtcNow.Date)
            && c.Data.Operations[0].path == "/EmailLastSendDate" 
            && DateTime.Parse(c.Data.Operations[0].value.ToString()).Date.Equals(DateTime.UtcNow.Date))), Times.Once);

        notificationService.Verify(x => x.Send(
            It.Is<SendEmailCommand>(c =>
                c.RecipientsAddress == command.User.Email
                && c.TemplateId == emailEnvironmentHelper.SavedSearchEmailNotificationTemplateId
                && c.Tokens["firstName"] == command.User.FirstName 
                && c.Tokens["newApprenticeships"] == $"{command.Vacancies.Count.ToString()} new {(command.Vacancies.Count == 1 ? "apprenticeship" : "apprenticeships")}"
                && c.Tokens["searchAlertDescriptor"] == $"{command.SearchTerm} in {command.Location}"
                && c.Tokens["showSearchLink"] == "yes"
                && !string.IsNullOrEmpty(c.Tokens["unsubscribeLink"])
                && !string.IsNullOrEmpty(c.Tokens["searchUrl"])
                && !string.IsNullOrEmpty(c.Tokens["searchParams"])
            )
        ), Times.Once);
    }
    
    [Test]
    [MoqAutoData]
    public async Task Then_When_No_Vacancies_LastRun_Updated_And_No_Email_Sent(
        PostSendSavedSearchNotificationCommand command,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        PostSendSavedSearchNotificationCommandHandler handler)
    {
        command.Vacancies = [];
        var expectedPatchSavedSearchApiRequest =
            new PatchSavedSearchApiRequest(command.Id, new JsonPatchDocument<PatchSavedSearch>());

        findApprenticeshipApiClient
            .Setup(x => x.PatchWithResponseCode(
                It.Is<PatchSavedSearchApiRequest>(c =>
                    c.PatchUrl == expectedPatchSavedSearchApiRequest.PatchUrl
                )))
            .ReturnsAsync(new ApiResponse<string>(null!, HttpStatusCode.OK, string.Empty));

        await handler.Handle(command, CancellationToken.None);

        findApprenticeshipApiClient.Verify(x => x.PatchWithResponseCode(It.Is<PatchSavedSearchApiRequest>(c =>
            c.PatchUrl.Contains(command.Id.ToString(), StringComparison.CurrentCultureIgnoreCase) 
            && c.Data.Operations[0].path == "/LastRunDate" 
            && DateTime.Parse(c.Data.Operations[0].value.ToString()).Date.Equals(DateTime.UtcNow.Date))), Times.Once);

        notificationService.Verify(x => x.Send(It.IsAny<SendEmailCommand>()), Times.Never);
    }
}