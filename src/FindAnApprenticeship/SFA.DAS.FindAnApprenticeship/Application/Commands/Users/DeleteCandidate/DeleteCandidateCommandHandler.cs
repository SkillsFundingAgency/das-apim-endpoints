﻿using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Domain.EmailTemplates;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DeleteCandidate
{
    public class DeleteCandidateCommandHandler(
        ILogger<DeleteCandidateCommandHandler> logger,
        IVacancyService vacancyService,
        INotificationService notificationService,
        IRecruitApiClient<RecruitApiConfiguration> recruitApiClient,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
        EmailEnvironmentHelper emailEnvironmentHelper) : IRequestHandler<DeleteCandidateCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteCandidateCommand command, CancellationToken cancellationToken)
        {
            var applicationsTask = candidateApiClient.Get<GetApplicationsApiResponse>(new GetApplicationsApiRequest(command.CandidateId));

            var candidateTask = candidateApiClient.Get<GetCandidateApiResponse>(new GetCandidateApiRequest(command.CandidateId.ToString()));

            await Task.WhenAll(applicationsTask, candidateTask);

            var applications = applicationsTask.Result;
            var candidate = candidateTask.Result;

            var applicationList = applications.Applications.Where(x =>
                    x.Status == ApplicationStatus.Submitted.ToString())
                .ToList();

            var emailNotifications = new List<EmailNotification>
            {
                Capacity = 0
            };

            foreach (var application in applicationList)
            {
                var response = await recruitApiClient.PostWithResponseCode<NullResponse>(
                    new PostWithdrawApplicationRequest(command.CandidateId, Convert.ToInt64(application.VacancyReference.Replace("VAC", ""))), false);

                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    logger.LogError("Unable to with draw application for candidate Id {CandidateId}", command.CandidateId);
                    throw new HttpRequestContentException($"Unable to withdraw application for candidate Id {command.CandidateId}", response.StatusCode, response.ErrorContent);
                }

                var jsonPatchDocument = new JsonPatchDocument<Domain.Models.Application>();

                jsonPatchDocument.Replace(x => x.Status, ApplicationStatus.Withdrawn);

                var vacancy = await vacancyService.GetVacancy(application.VacancyReference);
                var withDrawnApplicationEmail = vacancy is GetApprenticeshipVacancyItemResponse apprenticeshipVacancy
                    ? GetWithdrawApplicationEmail(candidate, apprenticeshipVacancy)
                    : GetWithdrawApplicationEmail(candidate, await vacancyService.GetClosedVacancy(application.VacancyReference) as GetClosedVacancyResponse);

                var patchRequest = new PatchApplicationApiRequest(application.Id, application.CandidateId, jsonPatchDocument);

                await candidateApiClient.PatchWithResponseCode(patchRequest);

                emailNotifications.Add(new EmailNotification(withDrawnApplicationEmail.TemplateId, withDrawnApplicationEmail.RecipientAddress, withDrawnApplicationEmail.Tokens));
            }

            if(emailNotifications.Count > 0) await SendApplicationWithDrawnNotificationEmail(emailNotifications);

            await Task.WhenAll(
                findApprenticeshipApiClient.Delete(new DeleteSavedSearchesApiRequest(command.CandidateId)),
                candidateApiClient.Delete(new DeleteAccountApiRequest(command.CandidateId)));

            return Unit.Value;
        }

        private async Task SendApplicationWithDrawnNotificationEmail(List<EmailNotification> emailNotifications)
        {
            foreach (var notification in emailNotifications)
            {
                await notificationService.Send(new SendEmailCommand(notification.TemplateId, notification.RecipientAddress,
                    notification.Tokens));
            }
        }

        private WithdrawApplicationEmail GetWithdrawApplicationEmail(GetCandidateApiResponse candidateApiResponse, GetClosedVacancyResponse vacancyResponse)
        {
            return new WithdrawApplicationEmail(
                emailEnvironmentHelper.WithdrawApplicationEmailTemplateId,
                candidateApiResponse.Email,
                candidateApiResponse.FirstName,
                vacancyResponse.Title,
                vacancyResponse.EmployerName,
                vacancyService.GetVacancyWorkLocation(vacancyResponse));
        }

        private WithdrawApplicationEmail GetWithdrawApplicationEmail(GetCandidateApiResponse candidateApiResponse, GetApprenticeshipVacancyItemResponse vacancyResponse)
        {
            return new WithdrawApplicationEmail(
                emailEnvironmentHelper.WithdrawApplicationEmailTemplateId,
                candidateApiResponse.Email,
                candidateApiResponse.FirstName,
                vacancyResponse.Title,
                vacancyResponse.EmployerName,
                vacancyService.GetVacancyWorkLocation(vacancyResponse));
        }
    }
}
