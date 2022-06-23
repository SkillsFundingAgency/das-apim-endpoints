using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Models;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.CourseStopped
{
    public class CourseStoppedCommandHandler : IRequestHandler<CourseStoppedCommand, Unit> 
    {
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _employerDemandApiClient;
        private readonly INotificationService _notificationService;

        public CourseStoppedCommandHandler(
            IEmployerDemandApiClient<EmployerDemandApiConfiguration> employerDemandApiClient, 
            INotificationService notificationService)
        {
            _employerDemandApiClient = employerDemandApiClient;
            _notificationService = notificationService;
        }

        public async Task<Unit> Handle(CourseStoppedCommand request, CancellationToken cancellationToken)
        {
            var courseDemand =
                await _employerDemandApiClient.Get<GetEmployerDemandResponse>(
                    new GetEmployerDemandRequest(request.EmployerDemandId));

            var emailModel = new StopSharingEmployerDemandCourseClosedEmail(
                courseDemand.ContactEmailAddress,
                courseDemand.OrganisationName, 
                courseDemand.Course.Title, 
                courseDemand.Course.Level,
                courseDemand.Location.Name, 
                courseDemand.NumberOfApprentices);
            
            await _notificationService.Send(new SendEmailCommand(emailModel.TemplateId,emailModel.RecipientAddress, emailModel.Tokens));
            
            var auditTask = _employerDemandApiClient.PostWithResponseCode<object>(
                new PostEmployerDemandNotificationAuditRequest(request.Id, request.EmployerDemandId, NotificationType.StoppedCourseClosed));
            var patchTask = _employerDemandApiClient.PatchWithResponseCode(new PatchCourseDemandRequest(
                request.EmployerDemandId, new PatchOperation
                {
                    Path = "Stopped",
                    Value = true
                }));

            await Task.WhenAll(auditTask, patchTask);
            
            return Unit.Value;
        }
    }
}