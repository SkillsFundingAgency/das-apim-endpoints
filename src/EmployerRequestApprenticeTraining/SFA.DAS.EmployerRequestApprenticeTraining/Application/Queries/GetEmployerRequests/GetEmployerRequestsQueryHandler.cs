using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EmployerRequest = SFA.DAS.SharedOuterApi.InnerApi.Responses.RequestApprenticeTraining.EmployerRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequests
{
    public class GetEmployerRequestsQueryHandler : IRequestHandler<GetEmployerRequestsQuery, GetEmployerRequestsResult>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;

        public GetEmployerRequestsQueryHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
        }

        public async Task<GetEmployerRequestsResult> Handle(GetEmployerRequestsQuery request, CancellationToken cancellationToken)
        {
            var employerRequests = await _requestApprenticeTrainingApiClient.
                GetWithResponseCode<List<EmployerRequest>>(new GetEmployerRequestsRequest(request.AccountId));

            employerRequests.EnsureSuccessStatusCode();

            return new GetEmployerRequestsResult
            {
                EmployerRequests = employerRequests.Body.Select(employerRequest => (SharedOuterApi.Models.RequestApprenticeTraining.EmployerRequest)employerRequest).ToList()
            };
        }
    }
}
