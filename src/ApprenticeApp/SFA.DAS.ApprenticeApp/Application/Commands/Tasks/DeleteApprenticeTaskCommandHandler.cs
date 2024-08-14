using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.Application.Commands
{
    public class DeleteApprenticeTaskCommandHandler : IRequestHandler<DeleteApprenticeTaskCommand, Unit>
    {
        private readonly IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> _progressApi;

        public DeleteApprenticeTaskCommandHandler(IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> progressApi)
        {
            _progressApi = progressApi;
        }

        public async Task<Unit> Handle(DeleteApprenticeTaskCommand request, CancellationToken cancellationToken)
        {
            await _progressApi.Delete(new DeleteApprenticeTaskRequest(request.ApprenticeshipId, request.TaskId));
            return Unit.Value;
        }

    }
}