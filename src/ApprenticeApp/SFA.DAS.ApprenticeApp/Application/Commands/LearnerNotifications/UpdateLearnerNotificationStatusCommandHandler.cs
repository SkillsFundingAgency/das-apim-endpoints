using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.LearnerNotifications.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Commands.LearnerNotifications
{
    public class UpdateLearnerNotificationStatusCommandHandler : IRequestHandler<UpdateLearnerNotificationStatusCommand, Unit>
    {
        private readonly ILearnerNotificationsInnerApiClient<LearnerNotificationsApiConfiguration> _notificationsApiClient;

        public UpdateLearnerNotificationStatusCommandHandler(ILearnerNotificationsInnerApiClient<LearnerNotificationsApiConfiguration> notificationsApiClient)
        {
            _notificationsApiClient = notificationsApiClient;
        }

        public async Task<Unit> Handle(UpdateLearnerNotificationStatusCommand request, CancellationToken cancellationToken)
        {
            var data = new UpdateNotificationStatusData
            {
                Status = request.Status
            };

            await _notificationsApiClient.Put(
                new UpdateLearnerNotificationStatusRequest(
                    request.AccountIdentifier, 
                    request.NotificationIdentifier, 
                    data));

            return Unit.Value;
        }
    }
}