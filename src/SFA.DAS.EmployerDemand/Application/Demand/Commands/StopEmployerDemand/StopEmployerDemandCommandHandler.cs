using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.Domain.Models;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.Notifications.Messages.Commands;
using SFA.DAS.SharedOuterApi.Configuration;
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
            var apiResponse = await _demandApiClient.PostWithResponseCode<GetEmployerDemandResponse>(
                    new PostStopEmployerDemandRequest(request.EmployerDemandId));

            var emailArgs = new StopSharingEmployerDemandEmail(
                apiResponse.Body.ContactEmailAddress,
                apiResponse.Body.OrganisationName,
                apiResponse.Body.Course.Title,
                apiResponse.Body.Course.Level,
                apiResponse.Body.Location.Name,
                apiResponse.Body.NumberOfApprentices,
                null);
            await _notificationService.Send(new SendEmailCommand(
                EmailConstants.StopSharingEmployerDemandTemplateId,
                apiResponse.Body.ContactEmailAddress,
                emailArgs.Tokens));

            return new StopEmployerDemandCommandResult
            {
                EmployerDemand = apiResponse.Body
            };
        }
    }
}