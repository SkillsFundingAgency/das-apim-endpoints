using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using SFA.DAS.EmployerDemand.Domain.Models;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.Demand.Commands.StopEmployerDemand
{
    public class StopEmployerDemandCommandHandler : IRequestHandler<StopEmployerDemandCommand, StopEmployerDemandCommandResult> 
    {
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _demandApiClient;
        private readonly INotificationService _notificationService;

        public StopEmployerDemandCommandHandler(
            IEmployerDemandApiClient<EmployerDemandApiConfiguration> demandApiClient,
            INotificationService notificationService)
        {
            _demandApiClient = demandApiClient;
            _notificationService = notificationService;
        }

        public async Task<StopEmployerDemandCommandResult> Handle(StopEmployerDemandCommand request, CancellationToken cancellationToken)
        {
            var getEmployerDemandResponse = await _demandApiClient.Get<GetEmployerDemandResponse>(
                new GetEmployerDemandRequest(request.EmployerDemandId));

            if (getEmployerDemandResponse == null)
            {
                return new StopEmployerDemandCommandResult();
            }

            if (getEmployerDemandResponse.Stopped)
            {
                return new StopEmployerDemandCommandResult
                {
                    EmployerDemand = getEmployerDemandResponse
                };
            }

            var stopDemandResponse = await _demandApiClient.PatchWithResponseCode(
                new PatchCourseDemandRequest(request.EmployerDemandId, new PatchOperation
                {
                    Path = "Stopped",
                    Value = true
                }));

            if (stopDemandResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestContentException($"Response status code does not indicate success: {(int)stopDemandResponse.StatusCode} ({stopDemandResponse.StatusCode})", stopDemandResponse.StatusCode, stopDemandResponse.ErrorContent);
            }
            
            await _demandApiClient.PostWithResponseCode<object>(
                new PostEmployerDemandNotificationAuditRequest(request.Id, request.EmployerDemandId, NotificationType.StoppedByUser));

            var stopDemandResponseBody =
                JsonConvert.DeserializeObject<GetEmployerDemandResponse>(stopDemandResponse.Body);
            
            var emailArgs = new StopSharingEmployerDemandEmail(
                stopDemandResponseBody.ContactEmailAddress,
                stopDemandResponseBody.OrganisationName,
                stopDemandResponseBody.Course.Title,
                stopDemandResponseBody.Course.Level,
                stopDemandResponseBody.Location.Name,
                stopDemandResponseBody.NumberOfApprentices,
                stopDemandResponseBody.StartSharingUrl);
            await _notificationService.Send(new SendEmailCommand(
                EmailConstants.StopSharingEmployerDemandTemplateId,
                stopDemandResponseBody.ContactEmailAddress,
                emailArgs.Tokens));

            return new StopEmployerDemandCommandResult
            {
                EmployerDemand = stopDemandResponseBody
            };
        }
    }
}