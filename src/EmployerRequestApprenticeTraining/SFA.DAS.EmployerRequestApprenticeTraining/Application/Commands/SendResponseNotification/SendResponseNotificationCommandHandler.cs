﻿using Azure;
using Azure.Core;
using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerRequestApprenticeTraining.Configuration;
using SFA.DAS.Encoding;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.SendResponseNotification
{
    public class SendResponseNotificationCommandHandler : IRequestHandler<SendResponseNotificationCommand, Unit>
    {
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;
        private readonly IEmployerProfilesApiClient<EmployerProfilesApiConfiguration> _employerProfilesApiClient;
        private readonly INotificationService _notificationService;
        private readonly IOptions<EmployerRequestApprenticeTrainingConfiguration> _options;
        private readonly IEncodingService _encodingService;

        public SendResponseNotificationCommandHandler(IAccountsApiClient<AccountsConfiguration> accountsApiClient, 
            IEmployerProfilesApiClient<EmployerProfilesApiConfiguration> employerProfilesApiClient, 
            INotificationService notificationService,
            IEncodingService encodingService,
            IOptions<EmployerRequestApprenticeTrainingConfiguration> options)
        {
            _accountsApiClient = accountsApiClient;
            _employerProfilesApiClient = employerProfilesApiClient;
            _notificationService = notificationService;
            _encodingService = encodingService;
            _options = options;
        }

        public async Task<Unit> Handle(SendResponseNotificationCommand command, CancellationToken cancellationToken)
        {
            var userResponse = await _employerProfilesApiClient.GetWithResponseCode<EmployerProfileUsersApiResponse>(
                new GetEmployerUserAccountRequest(command.RequestedBy.ToString()));
            userResponse.EnsureSuccessStatusCode();

            var teamMembers = await _accountsApiClient.GetAll<GetAccountTeamMembersResponse>(
                        new GetAccountTeamMembersRequest(command.AccountId));

            if (teamMembers == null || !teamMembers.Any())
            {
                throw new ApiResponseException(System.Net.HttpStatusCode.BadRequest, "No team members found or API returned an empty list.");
            }

            var member = teamMembers.FirstOrDefault(c =>
                c.UserRef.Equals(command.RequestedBy.ToString(), StringComparison.OrdinalIgnoreCase) 
                && c.CanReceiveNotifications);

            var encodedAccountId = _encodingService.Encode(command.AccountId, EncodingType.AccountId);

            if (member != null)
            {
                var templateId = _options.Value.NotificationTemplates.FirstOrDefault(p => p.TemplateName == "RATProviderResponseConfirmation")?.TemplateId;
                if (templateId != null)
                {
                    await _notificationService.Send(
                        new SendEmailCommand(templateId.ToString(), member.Email, 
                        GetResponseEmailData(userResponse.Body, command.Standards, encodedAccountId)));
                }
            }
            return Unit.Value;
        }

        public Dictionary<string, string> GetResponseEmailData(EmployerProfileUsersApiResponse profile, List<StandardDetails> standardDetails, string encodedAccountId)
        {
            var courseList = new StringBuilder();
            courseList.Append("<ul>");
            foreach (var course in standardDetails)
            {
                courseList.Append("<li>");
                courseList.Append($"{course.StandardTitle} level({course.StandardLevel})");
                courseList.Append("</li>");
            }
            courseList.Append("</ul>");

            var manageRequestsLink = $"{_options.Value.EmployerRequestApprenticeshipTrainingWebBaseUrl}{encodedAccountId}/dashboard";

            return new Dictionary<string, string>
            {
                { "Name",  profile.FirstName},
                { "Courses", courseList.ToString()},
                { "ManageRequestsLink", manageRequestsLink}
            };
        }
    }
}
