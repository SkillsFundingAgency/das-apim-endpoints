using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts
{
    public class RemoveTaskToKsbProgressCommandHandler : IRequestHandler<RemoveTaskToKsbProgressCommand, Unit>
    {
        private readonly IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> _progressApi;

        public RemoveTaskToKsbProgressCommandHandler(IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> progressApi)
        {
            _progressApi = progressApi;
        }

        public async Task<Unit> Handle(RemoveTaskToKsbProgressCommand request, CancellationToken cancellationToken)
        {
            await _progressApi.Delete(new DeleteTaskToKsbProgressRequest(request.ApprenticeshipId, request.KsbProgressId, request.TaskId));
            return Unit.Value;
        }
    }
}