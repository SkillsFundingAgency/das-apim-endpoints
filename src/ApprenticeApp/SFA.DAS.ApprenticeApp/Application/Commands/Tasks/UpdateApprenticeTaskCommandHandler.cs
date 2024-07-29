using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts
{
    public class UpdateApprenticeTaskCommandHandler : IRequestHandler<UpdateApprenticeTaskCommand, Unit>
    {
        private readonly IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> _progressApi;

        public UpdateApprenticeTaskCommandHandler(IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> progressApi)
        {
            _progressApi = progressApi;
        }

        public async Task<Unit> Handle(UpdateApprenticeTaskCommand request, CancellationToken cancellationToken)
        {
            var apiResponse =
                await _progressApi.PostWithResponseCode<PatchApprenticeTaskRequest>(new PatchApprenticeTaskRequest(request.ApprenticeshipId, request.TaskId, request.Data));

            return Unit.Value;
        }

    }
}