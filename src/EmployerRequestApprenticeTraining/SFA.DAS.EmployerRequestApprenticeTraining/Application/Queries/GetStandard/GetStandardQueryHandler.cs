using MediatR;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Responses;
using SFA.DAS.EmployerRequestApprenticeTraining.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using GetStandardRequest = SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests.GetStandardRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetStandard
{
    public class GetStandardQueryHandler : IRequestHandler<GetStandardQuery, GetStandardResult>
    {
        private readonly IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> _requestApprenticeTrainingApiClient;

        public GetStandardQueryHandler(IRequestApprenticeTrainingApiClient<RequestApprenticeTrainingApiConfiguration> requestApprenticeTrainingApiClient)
        {
            _requestApprenticeTrainingApiClient = requestApprenticeTrainingApiClient;
        }

        public async Task<GetStandardResult> Handle(GetStandardQuery request, CancellationToken cancellationToken)
        {
            var response = await _requestApprenticeTrainingApiClient.
                GetWithResponseCode<StandardResponse>(new GetStandardRequest(request.StandardReference));

            if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                response.EnsureSuccessStatusCode();
            }
            return new GetStandardResult { Standard = (Standard)response.Body };
        }
    }
}
