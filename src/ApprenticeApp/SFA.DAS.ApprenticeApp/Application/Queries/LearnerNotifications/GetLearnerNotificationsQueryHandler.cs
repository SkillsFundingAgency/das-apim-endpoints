using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.LearnerNotifications.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.LearnerNotifications
{
    public class GetLearnerNotificationsQueryHandler : IRequestHandler<GetLearnerNotificationsQuery, GetLearnerNotificationsQueryResult>
    {
        private readonly ILearnerNotificationsInnerApiClient<LearnerNotificationsApiConfiguration> _notificationsApiClient;

        public GetLearnerNotificationsQueryHandler(ILearnerNotificationsInnerApiClient<LearnerNotificationsApiConfiguration> notificationsApiClient)
        {
            _notificationsApiClient = notificationsApiClient;
        }

        public async Task<GetLearnerNotificationsQueryResult> Handle(GetLearnerNotificationsQuery request, CancellationToken cancellationToken)
        {
            var result = await _notificationsApiClient.Get<GetLearnerNotificationsQueryResult>(
                new GetLearnerNotificationsRequest(request.AccountIdentifier));

            return new GetLearnerNotificationsQueryResult
            {
                Notifications = result.Notifications
            };
        }
    }
}