using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequestsForResponseNotification
{
    public class GetEmployerRequestsForResponseNotificationQueryHandler : IRequestHandler<GetEmployerRequestsForResponseNotificationQuery, GetEmployerRequestsForResponseNotificationResult>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;

        public GetEmployerRequestsForResponseNotificationQueryHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
        }

        public async Task<GetEmployerRequestsForResponseNotificationResult> Handle(GetEmployerRequestsForResponseNotificationQuery query, CancellationToken cancellationToken)
        {
            var employerRequests = await _requestApprenticeTrainingApiClient.
                GetWithResponseCode<List<EmployerRequestForResponseNotification>>(new GetEmployerRequestsForResponseNotificationRequest());

            employerRequests.EnsureSuccessStatusCode();

            return new GetEmployerRequestsForResponseNotificationResult
            {
                EmployerRequests = employerRequests.Body
                .Select(c => (SharedOuterApi.Models.RequestApprenticeTraining.EmployerRequestForResponseNotification)c).ToList(),
            };
        }
    }
}
