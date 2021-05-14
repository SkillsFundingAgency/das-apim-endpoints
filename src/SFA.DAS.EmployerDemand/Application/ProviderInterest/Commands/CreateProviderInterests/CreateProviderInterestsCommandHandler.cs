using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.ProviderInterest.Commands.CreateProviderInterests
{
    public class CreateProviderInterestsCommandHandler: IRequestHandler<CreateProviderInterestsCommand, int>
    {
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _apiClient;

        public CreateProviderInterestsCommandHandler(
            IEmployerDemandApiClient<EmployerDemandApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<int> Handle(CreateProviderInterestsCommand request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.PostWithResponseCode<PostCreateProviderInterestsResponse>(
                new PostCreateProviderInterestsRequest(request));

            return result.Body.Ukprn;
        }
    }
}