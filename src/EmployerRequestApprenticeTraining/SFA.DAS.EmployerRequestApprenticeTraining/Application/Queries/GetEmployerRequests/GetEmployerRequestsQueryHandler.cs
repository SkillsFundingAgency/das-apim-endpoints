using MediatR;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EmployerRequest = SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Responses.EmployerRequest;

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
                Get<List<EmployerRequest>>(new GetEmployerRequestsRequest(request.AccountId));

            return new GetEmployerRequestsResult
            {
                EmployerRequests = employerRequests.Select(employerRequest => (Models.EmployerRequest)employerRequest).ToList()
            };
        }
    }
}
