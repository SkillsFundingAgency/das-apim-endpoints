using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts
{
    public class AddUpdateKsbProgressCommandHandler : IRequestHandler<AddUpdateKsbProgressCommand, Unit>
    {
        private readonly IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> _progressApi;

        public AddUpdateKsbProgressCommandHandler(IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> progressApi)
        {
            _progressApi = progressApi;
        }

        public async Task<Unit> Handle(AddUpdateKsbProgressCommand request, CancellationToken cancellationToken)
        {
            await _progressApi.PostWithResponseCode<PostApprenticeTaskRequest>(new PostKsbProgressRequest(request.ApprenticeshipId, request.Data));
            return Unit.Value;
        }
    }
}