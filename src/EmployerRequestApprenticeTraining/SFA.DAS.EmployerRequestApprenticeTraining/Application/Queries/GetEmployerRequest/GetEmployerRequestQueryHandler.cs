using MediatR;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Threading;
using System.Threading.Tasks;

using EmployerRequest = SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Responses.EmployerRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequest
{
    public class GetEmployerRequestQueryHandler : IRequestHandler<GetEmployerRequestQuery, GetEmployerRequestResult>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;

        public GetEmployerRequestQueryHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
        }

        public async Task<GetEmployerRequestResult> Handle(GetEmployerRequestQuery request, CancellationToken cancellationToken)
        {
            ApiResponse<EmployerRequest> employerRequest = await _requestApprenticeTrainingApiClient.
                GetWithResponseCode<EmployerRequest>(new GetEmployerRequestRequest(request.EmployerRequestId.Value));
            
            if (employerRequest?.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                employerRequest.EnsureSuccessStatusCode();
            }

            return new GetEmployerRequestResult
            {
                EmployerRequest = (EmployerRequestApprenticeTraining.Models.EmployerRequest)employerRequest?.Body
            };
        }
    }
}
