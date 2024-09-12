using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.EmployerRequestApprenticeTraining.Configuration;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.EmployerRequestApprenticeTraining.Models;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests.PostSubmitEmployerRequestRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Commands.SubmitEmployerRequest
{
    public class SubmitEmployerRequestCommandHandler : IRequestHandler<SubmitEmployerRequestCommand, SubmitEmployerRequestResponse>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;
        private readonly INotificationService _notificationService;
        private readonly IOptions<EmployerRequestApprenticeTrainingConfiguration> _options;


        public SubmitEmployerRequestCommandHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient, 
            INotificationService notificationService, IOptions<EmployerRequestApprenticeTrainingConfiguration> options)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
            _notificationService = notificationService;
            _options = options;
        }

        public async Task<SubmitEmployerRequestResponse> Handle(SubmitEmployerRequestCommand command, CancellationToken cancellationToken)
        {
            var request = new PostSubmitEmployerRequestRequest(command.AccountId, new PostSubmitEmployerRequestData
            {
                OriginalLocation = command.OriginalLocation,
                RequestType = command.RequestType,
                StandardReference = command.StandardReference,
                NumberOfApprentices = command.NumberOfApprentices,
                SameLocation = command.SameLocation,
                SingleLocation = command.SingleLocation,
                SingleLocationLatitude = command.SingleLocationLatitude,
                SingleLocationLongitude = command.SingleLocationLongitude,
                MultipleLocations = command.MultipleLocations,
                AtApprenticesWorkplace = command.AtApprenticesWorkplace,
                DayRelease = command.DayRelease,
                BlockRelease = command.BlockRelease,
                RequestedBy = command.RequestedBy,
                ModifiedBy = command.ModifiedBy
            });

            var response = await _requestApprenticeTrainingApiClient
                .PostWithResponseCode<PostSubmitEmployerRequestData, SubmitEmployerRequestResponse>(request);

            response.EnsureSuccessStatusCode();

            var templateId = _options.Value.NotificationTemplates.FirstOrDefault(p => p.TemplateName == EmailTemplateNames.RATEmployerRequestConfirmation)?.TemplateId;
            if (templateId != null)
            {
                await _notificationService.Send(new SendEmailCommand(
                    templateId.ToString(), 
                    command.RequestedByEmail, 
                    GetEmailData(command)));
            }

            return response.Body;
        }

        public Dictionary<string, string> GetEmailData(SubmitEmployerRequestCommand command)
        {
            return new Dictionary<string, string>
            {
                { "course_level", command.CourseLevel },
                { "user_name", command.RequestedByEmail },
                { "expiry_months", command.ExpiryAfterMonths.ToString() },
                { "email_address", command.RequestedByEmail },
                { "dashboard_url", command.DashboardUrl }
            };
        }
    }
}
