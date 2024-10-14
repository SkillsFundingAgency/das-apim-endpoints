using MediatR;
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
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DeleteCandidate
{
    public record DeleteCandidateCommandHandler(
        ILogger<DeleteCandidateCommandHandler> Logger,
        IVacancyService VacancyService,
        INotificationService NotificationService,
        IRecruitApiClient<RecruitApiConfiguration> RecruitApiClient,
        ICandidateApiClient<CandidateApiConfiguration> CandidateApiClient,
        EmailEnvironmentHelper EmailEnvironmentHelper) : IRequestHandler<DeleteCandidateCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteCandidateCommand command, CancellationToken cancellationToken)
        {
            var applicationsTask = CandidateApiClient.Get<GetApplicationsApiResponse>(new GetApplicationsApiRequest(command.CandidateId));

            var candidateTask = CandidateApiClient.Get<GetCandidateApiResponse>(new GetCandidateApiRequest(command.CandidateId.ToString()));

            await Task.WhenAll(applicationsTask, candidateTask);

            var applications = applicationsTask.Result;
            var candidate = candidateTask.Result;

            var applicationList = applications.Applications.Where(x =>
                    x.Status == ApplicationStatus.Submitted.ToString())
                .ToList();

            foreach (var application in applicationList)
            {
                var response = await RecruitApiClient.PostWithResponseCode<NullResponse>(
                    new PostWithdrawApplicationRequest(command.CandidateId, Convert.ToInt64(application.VacancyReference.Replace("VAC", ""))), false);

                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    Logger.LogError("Unable to with draw application for candidate Id {CandidateId}", command.CandidateId);
                    throw new HttpRequestContentException($"Unable to withdraw application for candidate Id {command.CandidateId}", response.StatusCode, response.ErrorContent);
                }

                var jsonPatchDocument = new JsonPatchDocument<Domain.Models.Application>();

                jsonPatchDocument.Replace(x => x.Status, ApplicationStatus.Withdrawn);

                var vacancy = await VacancyService.GetVacancy(application.VacancyReference) as GetApprenticeshipVacancyItemResponse;

                var patchRequest = new PatchApplicationApiRequest(application.Id, application.CandidateId, jsonPatchDocument);
                var email = new WithdrawApplicationEmail(
                    EmailEnvironmentHelper.WithdrawApplicationEmailTemplateId,
                    candidate.Email,
                    candidate.FirstName,
                    vacancy?.Title, vacancy?.EmployerName,
                    vacancy?.Address.AddressLine4 ?? vacancy?.Address.AddressLine3 ?? vacancy?.Address.AddressLine2 ?? vacancy?.Address.AddressLine1 ?? "Unknown",
                    vacancy?.Address.Postcode);

                await Task.WhenAll(
                    CandidateApiClient.PatchWithResponseCode(patchRequest)
                    , NotificationService.Send(new SendEmailCommand(email.TemplateId, email.RecipientAddress, email.Tokens))
                );
            }

            var apiRequest = new DeleteAccountApiRequest(command.CandidateId);
            await CandidateApiClient.Delete(apiRequest);

            return Unit.Value;
        }
    }
}
