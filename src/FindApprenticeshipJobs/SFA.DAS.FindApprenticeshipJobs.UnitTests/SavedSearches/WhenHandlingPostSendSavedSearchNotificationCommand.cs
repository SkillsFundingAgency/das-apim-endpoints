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

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.SavedSearches
{
    [TestFixture]
    public class WhenHandlingPostSendSavedSearchNotificationCommand
    {
        [Test]
        [MoqInlineAutoData("someName", "someone@test.com", "Apprenticeship", "Apprenticeship", "address", "address")]
        [MoqInlineAutoData("someName", "someone@test.com", "Jobs", "Jobs", "address3", "address3")]
        [MoqInlineAutoData("someName", "someone@test.com", null, "", "address2", "address2")]
        [MoqInlineAutoData("someName", "test@test.com", "", "", null, "")]
        public async Task Then_The_SavedSearch_Is_Patched_LastEmailSent_Updated_And_Email_Sent(
        string firstName,
        string email,
        string searchTerm,
        string expectedSearchTerm,
        string location,
        string expectedLocation,
        PostSendSavedSearchNotificationCommand command,
        EmailEnvironmentHelper emailEnvironmentHelper,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        [Frozen] Mock<INotificationService> notificationService,
        PostSendSavedSearchNotificationCommandHandler handler)
        {
            command.User.Email = email;
            command.User.FirstName = firstName;
            command.SearchTerm = searchTerm;
            command.Location = location;

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
                c.PatchUrl.Contains(command.Id.ToString(), StringComparison.CurrentCultureIgnoreCase))), Times.Once);

            notificationService.Verify(x => x.Send(
                It.Is<SendEmailCommand>(c =>
                    c.RecipientsAddress == email
                    && c.TemplateId == emailEnvironmentHelper.SavedSearchEmailNotificationTemplateId
                    && c.Tokens["firstName"] == firstName
                    && c.Tokens["location"] == expectedLocation
                    && c.Tokens["keyword"] ==expectedSearchTerm
                    && c.Tokens["newApprenticeships"] == command.Vacancies.Count.ToString()
                    && !string.IsNullOrEmpty(c.Tokens["unsubscribeLink"])
                )
            ), Times.Once);
        }
    }
}