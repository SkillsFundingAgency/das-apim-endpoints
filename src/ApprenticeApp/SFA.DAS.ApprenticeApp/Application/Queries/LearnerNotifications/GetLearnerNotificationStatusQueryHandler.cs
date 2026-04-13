using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.LearnerNotifications.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.LearnerNotifications
{
    public class GetLearnerNotificationStatusQueryHandler : IRequestHandler<GetLearnerNotificationStatusQuery, GetLearnerNotificationStatusQueryResult>
    {
        private readonly ILearnerNotificationsInnerApiClient<LearnerNotificationsApiConfiguration> _notificationsApiClient;

        public GetLearnerNotificationStatusQueryHandler(ILearnerNotificationsInnerApiClient<LearnerNotificationsApiConfiguration> notificationsApiClient)
        {
            _notificationsApiClient = notificationsApiClient;
        }

        public async Task<GetLearnerNotificationStatusQueryResult> Handle(GetLearnerNotificationStatusQuery request, CancellationToken cancellationToken)
        {
            var result = await _notificationsApiClient.Get<GetLearnerNotificationStatusQueryResult>(
                new GetLearnerNotificationStatusRequest(request.AccountIdentifier, request.NotificationIdentifier));

            return new GetLearnerNotificationStatusQueryResult
            {
                NotificationStatus = result.NotificationStatus
            };
        }
    }
}