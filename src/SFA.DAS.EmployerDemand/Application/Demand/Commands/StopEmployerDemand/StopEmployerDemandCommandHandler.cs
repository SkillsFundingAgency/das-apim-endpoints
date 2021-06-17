using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
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

            var stopDemandResponse = await _demandApiClient.PostWithResponseCode<GetEmployerDemandResponse>(
                new PostStopEmployerDemandRequest(request.EmployerDemandId));

            if (stopDemandResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestContentException($"Response status code does not indicate success: {(int)stopDemandResponse.StatusCode} ({stopDemandResponse.StatusCode})", stopDemandResponse.StatusCode, stopDemandResponse.ErrorContent);
            }

            var emailArgs = new StopSharingEmployerDemandEmail(
                stopDemandResponse.Body.ContactEmailAddress,
                stopDemandResponse.Body.OrganisationName,
                stopDemandResponse.Body.Course.Title,
                stopDemandResponse.Body.Course.Level,
                stopDemandResponse.Body.Location.Name,
                stopDemandResponse.Body.NumberOfApprentices,
                stopDemandResponse.Body.StartSharingUrl);
            await _notificationService.Send(new SendEmailCommand(
                EmailConstants.StopSharingEmployerDemandTemplateId,
                stopDemandResponse.Body.ContactEmailAddress,
                emailArgs.Tokens));

            return new StopEmployerDemandCommandResult
            {
                EmployerDemand = stopDemandResponse.Body
            };
        }
    }
}