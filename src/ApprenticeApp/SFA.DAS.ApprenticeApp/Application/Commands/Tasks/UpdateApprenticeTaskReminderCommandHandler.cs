using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Commands.Tasks
{
    public class UpdateApprenticeTaskReminderCommandHandler : IRequestHandler<UpdateApprenticeTaskReminderCommand, Unit>
    {
        private readonly IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> _progressApi;

        public UpdateApprenticeTaskReminderCommandHandler(IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> progressApi)
        {
            _progressApi = progressApi;
        }

        public async Task<Unit> Handle(UpdateApprenticeTaskReminderCommand request, CancellationToken cancellationToken)
        {
            await _progressApi.PostWithResponseCode<PatchApprenticeTaskReminderRequest>(new PatchApprenticeTaskReminderRequest(request.TaskId, request.StatusId),false);
            return Unit.Value;
        }
    }
}
