using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.FindApprenticeshipJobs.Domain.EmailTemplates;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands.SavedSearch.SendNotification;

public record PostSendSavedSearchNotificationCommandHandler(
    IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> FindApprenticeshipApiClient,
    INotificationService NotificationService,
    EmailEnvironmentHelper EmailEnvironmentHelper) : IRequestHandler<PostSendSavedSearchNotificationCommand, Unit>
{
    public async Task<Unit> Handle(PostSendSavedSearchNotificationCommand command, CancellationToken cancellationToken)
    {
        var jsonPatchDocument = new JsonPatchDocument<PatchSavedSearch>();
        if (command.Vacancies.Count != 0)
        {
            jsonPatchDocument.Replace(x => x.EmailLastSendDate, DateTime.UtcNow);    
        }
            
        jsonPatchDocument.Replace(x => x.LastRunDate, DateTime.UtcNow);
            
        var patchRequest = new PatchSavedSearchApiRequest(command.Id, jsonPatchDocument);

        if (command.Vacancies.Count == 0)
        {
            await FindApprenticeshipApiClient.PatchWithResponseCode(patchRequest);
            return Unit.Value;
        }

        var vacanciesEmailSnippet = EmailTemplateBuilder.GetSavedSearchVacanciesSnippet(
            EmailEnvironmentHelper,
            command.Vacancies,
            command.Location != null);
        
        var searchParamsEmailSnippet = EmailTemplateBuilder.GetSavedSearchSearchParams(
            command.SearchTerm,
            command.Distance,
            command.Location,
            command.Categories != null && command.Categories.Any() ? command.Categories?.Select(cat => cat.Name).ToList() : null,
            command.Levels != null && command.Levels.Any() ? command.Levels.Select(lev => lev.Name).ToList() : null,
            command.DisabilityConfident,
            command.ExcludeNational,
            command.ApprenticeshipTypes);
        
        var queryParameters = EmailTemplateBuilder.GetSavedSearchUrl(
            command.SearchTerm,
            command.Distance,
            command.Location,
            command.Categories != null && command.Categories.Any() ? command.Categories?.Select(cat => cat.Id.ToString()).ToList() : null,
            command.Levels != null && command.Levels.Any() ? command.Levels.Select(lev => lev.Code.ToString()).ToList() : null,
            command.DisabilityConfident,
            command.ExcludeNational,
            command.ApprenticeshipTypes);

        var email = new SavedSearchEmailNotificationTemplate(
            EmailEnvironmentHelper.SavedSearchEmailNotificationTemplateId,
            command.User.Email,
            command.User.FirstName,
            $"{command.Vacancies.Count.ToString()} new {(command.Vacancies.Count == 1 ? "apprenticeship" : "apprenticeships")}",
            $"{EmailTemplateBuilder.BuildSearchAlertDescriptor(command)}",
            string.Concat(EmailEnvironmentHelper.SearchUrl, queryParameters), 
            string.Concat(EmailEnvironmentHelper.SavedSearchUnSubscribeUrl, command.UnSubscribeToken),
            vacanciesEmailSnippet,
            searchParamsEmailSnippet,
            command.Vacancies.Count > 5 ? "yes" : "no");

        await Task.WhenAll(
            FindApprenticeshipApiClient.PatchWithResponseCode(patchRequest),
            NotificationService.Send(new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens))
        );

        return Unit.Value;
    }
}