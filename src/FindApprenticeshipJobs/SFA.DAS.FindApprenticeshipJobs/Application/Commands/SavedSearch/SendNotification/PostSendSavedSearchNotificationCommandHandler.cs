using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification
{
    public record PostSendSavedSearchNotificationCommandHandler(
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> FindApprenticeshipApiClient,
        INotificationService NotificationService,
        EmailEnvironmentHelper EmailEnvironmentHelper) : IRequestHandler<PostSendSavedSearchNotificationCommand, Unit>
    {
        public async Task<Unit> Handle(PostSendSavedSearchNotificationCommand command, CancellationToken cancellationToken)
        {
            var jsonPatchDocument = new JsonPatchDocument<PatchSavedSearch>();
            jsonPatchDocument.Replace(x => x.EmailLastSendDate, DateTime.UtcNow);
            jsonPatchDocument.Replace(x => x.LastRunDate, DateTime.UtcNow);

            var patchRequest = new PatchSavedSearchApiRequest(command.Id, jsonPatchDocument);

            var vacanciesEmailSnippet =
                EmailTemplateBuilder.GetSavedSearchVacanciesSnippet(EmailEnvironmentHelper, command.Vacancies);

            var email = new SavedSearchEmailNotificationTemplate(
                EmailEnvironmentHelper.SavedSearchEmailNotificationTemplateId,
                command.User.Email,
                command.User.FirstName,
                command.Vacancies.Count.ToString(),
                $"{command.SearchTerm} in {command.Location}",
                !string.IsNullOrEmpty(command.SearchTerm) ? command.SearchTerm : string.Empty,
                EmailEnvironmentHelper.SearchUrl,
                !string.IsNullOrEmpty(command.Location) ? command.Location : string.Empty,
                command.Categories is {Count: > 0} ? string.Join(",", command.Categories) : string.Empty,
                command.Levels is { Count: > 0 } ? string.Join(",", command.Levels) : string.Empty,
                EmailEnvironmentHelper.SavedSearchUnSubscribeUrl.Replace("{search-Id}", command.Id.ToString()),
                vacanciesEmailSnippet);

            await Task.WhenAll(
                FindApprenticeshipApiClient.PatchWithResponseCode(patchRequest)
                , NotificationService.Send(new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens))
            );

            return Unit.Value;
        }
    }
}
