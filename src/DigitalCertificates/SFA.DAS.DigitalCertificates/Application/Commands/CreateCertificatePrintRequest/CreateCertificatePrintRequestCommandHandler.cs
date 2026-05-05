using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificatePrintRequest
{
    public class CreateCertificatePrintRequestCommandHandler : IRequestHandler<CreateCertificatePrintRequestCommand, Unit>
    {
        private readonly IAssessorsApiClient<AssessorsApiConfiguration> _assessorsApiClient;
        private readonly INotificationService _notificationService;
        private readonly ILogger<CreateCertificatePrintRequestCommandHandler> _logger;

        public CreateCertificatePrintRequestCommandHandler(IAssessorsApiClient<AssessorsApiConfiguration> assessorsApiClient, INotificationService notificationService, ILogger<CreateCertificatePrintRequestCommandHandler> logger)
        {
            _assessorsApiClient = assessorsApiClient;
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateCertificatePrintRequestCommand command, CancellationToken cancellationToken)
        {
            var certificateResponse = await _assessorsApiClient.GetWithResponseCode<GetStandardCertificateResponse>(new GetStandardCertificateRequest(command.CertificateId, false));

            if (certificateResponse == null || certificateResponse.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ArgumentException("Certificate not found");
            }

            certificateResponse.EnsureSuccessStatusCode();

            var certificate = certificateResponse.Body;

            if (!string.Equals(certificate.LatestEPAOutcome, "Pass", StringComparison.OrdinalIgnoreCase)
                || !string.Equals(certificate.Status, "Submitted", StringComparison.OrdinalIgnoreCase)
                || certificate.PrintRequestedAt != null)
            {
                throw new ArgumentException("Certificate is not eligible for print request");
            }

            var requestData = (PutCertificatePrintRequestData)command;

            var request = new PutCertificatePrintRequest(requestData, command.CertificateId.ToString());

            var response = await _assessorsApiClient.PutWithResponseCode<PutCertificatePrintRequestData, NullResponse>(request);

            response.EnsureSuccessStatusCode();

            try
            {
                await SendConfirmationEmail(command, certificate.Type).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Print requested for certificate {CertificateId} but failed to send confirmation email. Print request has already been sent.", command.CertificateId);
            }

            return Unit.Value;
        }

        private async Task SendConfirmationEmail(CreateCertificatePrintRequestCommand command, string certificateType)
        {
            var tokens = new Dictionary<string, string>
            {
                { "UserName", command.Email?.UserName },
                { "LinkDomain", command.Email?.LinkDomain },
                { "CertificateId", command.CertificateId.ToString() },
                { "Type", certificateType?.ToLower() }
            };

            _logger.LogInformation("Sending print confirmation email for certificate {CertificateId} using template {TemplateId}", command.CertificateId, command.Email?.TemplateId);

            var emailCommand = new SendEmailCommand(
                templateId: command.Email?.TemplateId,
                recipientsAddress: command.Email?.EmailAddress,
                tokens: tokens);

            await _notificationService.Send(emailCommand).ConfigureAwait(false);

            _logger.LogInformation("Successfully sent print confirmation email for certificate {CertificateId}", command.CertificateId);
        }
    }
}
