using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.LearnerNotifications.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Commands.LearnerNotifications
{
    public class DeleteLearnerNotificationCommandHandler
        : IRequestHandler<DeleteLearnerNotificationCommand, Unit>
    {
        private readonly ILearnerNotificationsInnerApiClient<LearnerNotificationsApiConfiguration>
            _notificationsApiClient;

        public DeleteLearnerNotificationCommandHandler(
            ILearnerNotificationsInnerApiClient<LearnerNotificationsApiConfiguration>
                notificationsApiClient)
        {
            _notificationsApiClient = notificationsApiClient;
        }

        public async Task<Unit> Handle(
            DeleteLearnerNotificationCommand request,
            CancellationToken cancellationToken)
        {
            await _notificationsApiClient.Delete(
                new DeleteLearnerNotificationRequest(
                    request.AccountIdentifier,
                    request.NotificationIdentifier));

            return Unit.Value;
        }
    }
}