using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.Application.Commands
{
    public class UpdateApprenticeTaskStatusCommandHandler : IRequestHandler<UpdateApprenticeTaskStatusCommand, Unit>
    {
        private readonly IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> _progressApi;

        public UpdateApprenticeTaskStatusCommandHandler(IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> progressApi)
        {
            _progressApi = progressApi;
        }

        public async Task<Unit> Handle(UpdateApprenticeTaskStatusCommand request, CancellationToken cancellationToken)
        {
            ApprenticeTaskData data = new ApprenticeTaskData();
            await _progressApi.PostWithResponseCode<PatchApprenticeTaskStatusRequest>(new PatchApprenticeTaskStatusRequest(request.ApprenticeshipId, request.TaskId, request.StatusId, data), false);
            return Unit.Value;
        }
    }
}