using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.LearnerNotifications.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.LearnerNotifications
{
    public class GetLearnerNotificationByIdQueryHandler : IRequestHandler<GetLearnerNotificationByIdQuery, GetLearnerNotificationByIdQueryResult>
    {
        private readonly ILearnerNotificationsInnerApiClient<LearnerNotificationsApiConfiguration> _notificationsApiClient;

        public GetLearnerNotificationByIdQueryHandler(ILearnerNotificationsInnerApiClient<LearnerNotificationsApiConfiguration> notificationsApiClient)
        {
            _notificationsApiClient = notificationsApiClient;
        }

       public async Task<GetLearnerNotificationByIdQueryResult> Handle(GetLearnerNotificationByIdQuery request, CancellationToken cancellationToken)
{
    var result = await _notificationsApiClient.Get<LearnerNotification>(
        new GetLearnerNotificationByIdRequest(request.AccountIdentifier, request.NotificationIdentifier));

        return new GetLearnerNotificationByIdQueryResult
        {
            Notification = result
        };
            }
        }
}