using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerDemand.Application.ProviderInterest.Commands.CreateProviderInterests
{
    public class CreateProviderInterestsCommandHandler: IRequestHandler<CreateProviderInterestsCommand, Guid>
    {
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _apiClient;
        private readonly INotificationService _notificationService;

        public CreateProviderInterestsCommandHandler(
            IEmployerDemandApiClient<EmployerDemandApiConfiguration> apiClient,
            INotificationService notificationService)
        {
            _apiClient = apiClient;
            _notificationService = notificationService;
        }

        public async Task<Guid> Handle(CreateProviderInterestsCommand request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.PostWithResponseCode<PostCreateProviderInterestsResponse>(
                new PostCreateProviderInterestsRequest(request));

            return result.Body.Id;
        }
    }
}