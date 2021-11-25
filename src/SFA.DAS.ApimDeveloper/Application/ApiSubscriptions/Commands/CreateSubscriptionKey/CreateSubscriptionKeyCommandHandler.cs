using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApimDeveloper.Configuration;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.ApimDeveloper.Interfaces;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Commands.CreateSubscriptionKey
{
    public class CreateSubscriptionKeyCommandHandler : IRequestHandler<CreateSubscriptionKeyCommand, Unit>
    {
        private readonly IApimDeveloperApiClient<ApimDeveloperApiConfiguration> _apimDeveloperApiClient;

        public CreateSubscriptionKeyCommandHandler (IApimDeveloperApiClient<ApimDeveloperApiConfiguration> apimDeveloperApiClient)
        {
            _apimDeveloperApiClient = apimDeveloperApiClient;
        }
        public async Task<Unit> Handle(CreateSubscriptionKeyCommand request, CancellationToken cancellationToken)
        {
            var createResponse = await _apimDeveloperApiClient.PostWithResponseCode<object>(
                new PostCreateSubscriptionKeyRequest(request.AccountIdentifier, request.ProductId));

            if (!string.IsNullOrEmpty(createResponse.ErrorContent))
            {
                throw new HttpRequestContentException($"Response status code does not indicate success: {(int)createResponse.StatusCode} ({createResponse.StatusCode})", createResponse.StatusCode, createResponse.ErrorContent);
            }

            return new Unit();
        }
    }
}