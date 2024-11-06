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

            var vacanciesEmailSnippet = EmailTemplateBuilder.GetSavedSearchVacanciesSnippet(EmailEnvironmentHelper,
                command.Vacancies);

            var searchParamsEmailSnippet = EmailTemplateBuilder.GetSavedSearchSearchParams(command.SearchTerm,
                command.Distance,
                command.Location,
                command.Categories?.Select(cat => cat.Name).ToList(),
                command.Levels?.Select(lev => lev.Name).ToList(),
                command.DisabilityConfident);

            var queryParameters = EmailTemplateBuilder.GetSavedSearchUrl(command.SearchTerm,
                command.Distance,
                command.Location,
                command.Categories?.Select(cat => cat.Id.ToString()).ToList(),
                command.Levels?.Select(lev => lev.Code.ToString()).ToList(),
                command.DisabilityConfident);

            var email = new SavedSearchEmailNotificationTemplate(
                EmailEnvironmentHelper.SavedSearchEmailNotificationTemplateId,
                command.User.Email,
                command.User.FirstName,
                command.Vacancies.Count.ToString(),
                $"{command.SearchTerm} in {command.Location}",
                string.Concat(EmailEnvironmentHelper.SearchUrl, queryParameters), 
                string.Concat(EmailEnvironmentHelper.SavedSearchUnSubscribeUrl, command.UnSubscribeToken),
                vacanciesEmailSnippet,
                searchParamsEmailSnippet);

            await Task.WhenAll(
                FindApprenticeshipApiClient.PatchWithResponseCode(patchRequest),
                NotificationService.Send(new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens))
            );

            return Unit.Value;
        }
    }
}
