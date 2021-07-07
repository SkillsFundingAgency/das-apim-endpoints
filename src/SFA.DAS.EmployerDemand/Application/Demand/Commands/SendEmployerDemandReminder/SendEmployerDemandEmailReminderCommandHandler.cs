using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Models;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.SendEmployerDemandReminder
{
    public class SendEmployerDemandEmailReminderCommandHandler : IRequestHandler<SendEmployerDemandEmailReminderCommand, Unit>
    {
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _employerDemandApiClient;
        private readonly INotificationService _notificationService;

        public SendEmployerDemandEmailReminderCommandHandler (IEmployerDemandApiClient<EmployerDemandApiConfiguration> employerDemandApiClient, INotificationService notificationService)
        {
            _employerDemandApiClient = employerDemandApiClient;
            _notificationService = notificationService;
        }
        public async Task<Unit> Handle(SendEmployerDemandEmailReminderCommand request, CancellationToken cancellationToken)
        {
            var courseDemand =
                await _employerDemandApiClient.Get<GetEmployerDemandResponse>(
                    new GetEmployerDemandRequest(request.EmployerDemandId));

            var emailModel = new CreateEmployerDemandReminderEmail(courseDemand.ContactEmailAddress,
                courseDemand.OrganisationName, courseDemand.Course.Title, courseDemand.Course.Level,
                courseDemand.Location.Name, courseDemand.NumberOfApprentices, courseDemand.StopSharingUrl);
            
            await _notificationService.Send(new SendEmailCommand(emailModel.TemplateId,emailModel.RecipientAddress, emailModel.Tokens));
            
            await _employerDemandApiClient.PostWithResponseCode<object>(
                new PostEmployerDemandNotificationAuditRequest(request.Id, request.EmployerDemandId, NotificationType.Reminder));
            
            return Unit.Value;
        }
    }
}