using MediatR;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Threading;
using System.Threading.Tasks;

using EmployerRequest = SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Responses.EmployerRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetActiveEmployerRequest
{
    public class GetActiveEmployerRequestQueryHandler : IRequestHandler<GetActiveEmployerRequestQuery, GetActiveEmployerRequestResult>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;

        public GetActiveEmployerRequestQueryHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
        }

        public async Task<GetActiveEmployerRequestResult> Handle(GetActiveEmployerRequestQuery request, CancellationToken cancellationToken)
        {
            ApiResponse<EmployerRequest> employerRequest = await _requestApprenticeTrainingApiClient.
                GetWithResponseCode<EmployerRequest>(new GetActiveEmployerRequestRequest(request.AccountId.Value, request.StandardReference));

            if (employerRequest?.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                employerRequest.EnsureSuccessStatusCode();
            }

            return new GetActiveEmployerRequestResult
            {
                EmployerRequest = (EmployerRequestApprenticeTraining.Models.EmployerRequest)employerRequest?.Body
            };
        }
    }
}
