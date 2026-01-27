using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmail
{
    public class CreateSharingEmailCommandHandler : IRequestHandler<CreateSharingEmailCommand, CreateSharingEmailResult>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;
        private readonly INotificationService _notificationService;
        private readonly ILogger<CreateSharingEmailCommandHandler> _logger;

        public CreateSharingEmailCommandHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient, INotificationService notificationService, ILogger<CreateSharingEmailCommandHandler> logger)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<CreateSharingEmailResult> Handle(CreateSharingEmailCommand command, CancellationToken cancellationToken)
        {
            PostCreateSharingEmailRequestData requestData = command;

            var request = new PostCreateSharingEmailRequest(requestData, command.SharingId.ToString());

            var response = await _digitalCertificatesApiClient.PostWithResponseCode<PostCreateSharingEmailRequestData, PostCreateSharingEmailResponse>(request);

            response.EnsureSuccessStatusCode();

            var result = (CreateSharingEmailResult)response.Body;

            await SendSharingEmail(command, result).ConfigureAwait(false);

            return result;
        }

        private async Task SendSharingEmail(CreateSharingEmailCommand command, CreateSharingEmailResult result)
        {
            var tokens = new Dictionary<string, string>
            {
                { "UserName", command.UserName },
                { "LinkDomain", command.LinkDomain},
                { "EmailLinkCode", result.EmailLinkCode.ToString() },
                { "MessageText", command.MessageText}
            };

            _logger.LogInformation("Sending sharing email for sharing {SharingId} using template {TemplateId}", command.SharingId, command.TemplateId);

            var emailCommand = new SendEmailCommand(
                templateId: command.TemplateId,
                recipientsAddress: command.EmailAddress,
                tokens: tokens);

            await _notificationService.Send(emailCommand).ConfigureAwait(false);

            _logger.LogInformation("Successfully sent email notification for sharing {SharingId}", command.SharingId);
        }
    }
}
