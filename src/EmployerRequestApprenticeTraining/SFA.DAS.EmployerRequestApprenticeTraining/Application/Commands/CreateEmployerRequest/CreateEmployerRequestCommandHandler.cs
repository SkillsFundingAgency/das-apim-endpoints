using MediatR;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using SFA.DAS.EmployerRequestApprenticeTraining.Configuration;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.CreateEmployerRequest
{
    public class CreateEmployerRequestCommandHandler : IRequestHandler<CreateEmployerRequestCommand, CreateEmployerRequestResponse>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;
        private readonly INotificationService _notificationService;
        private readonly IOptions<RequestApprenticeTrainingConfiguration> _options;


        public CreateEmployerRequestCommandHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient, 
            INotificationService notificationService, IOptions<RequestApprenticeTrainingConfiguration> options)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
            _notificationService = notificationService;
            _options = options;
        }

        public async Task<CreateEmployerRequestResponse> Handle(CreateEmployerRequestCommand command, CancellationToken cancellationToken)
        {
            var request = new CreateEmployerRequestRequest(new CreateEmployerRequestData
            {
                OriginalLocation = command.OriginalLocation,
                RequestType = command.RequestType,
                AccountId = command.AccountId,
                StandardReference = command.StandardReference,
                NumberOfApprentices = command.NumberOfApprentices,
                SingleLocation = command.SingleLocation,
                SingleLocationLatitude = command.SingleLocationLatitude,
                SingleLocationLongitude = command.SingleLocationLongitude,
                AtApprenticesWorkplace = command.AtApprenticesWorkplace,
                DayRelease = command.DayRelease,
                BlockRelease = command.BlockRelease,
                RequestedBy = command.RequestedBy,
                ModifiedBy = command.ModifiedBy
            });

            var employerRequestResponse = await _requestApprenticeTrainingApiClient
                .PostWithResponseCode<CreateEmployerRequestData, CreateEmployerRequestResponse>(request);

            employerRequestResponse.EnsureSuccessStatusCode();

            var templateId = _options.Value.NotificationTemplates.FirstOrDefault(p => p.TemplateName == "SubmitEmployerRequest")?.TemplateId;
            if (templateId != null)
            {
                await _notificationService.Send(new SendEmailCommand(templateId.ToString(), command.RequestedByEmail, null));
            }

            return employerRequestResponse.Body;
        }
    }
}
