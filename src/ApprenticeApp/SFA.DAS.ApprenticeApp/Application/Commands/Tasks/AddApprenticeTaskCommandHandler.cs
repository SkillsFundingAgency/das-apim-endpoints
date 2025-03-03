using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.Application.Commands
{
    public class AddApprenticeTaskCommandHandler : IRequestHandler<AddApprenticeTaskCommand, Unit>
    {
        private readonly IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> _progressApi;

        public AddApprenticeTaskCommandHandler(IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> progressApi)
        {
            _progressApi = progressApi;
        }

        public async Task<Unit> Handle(AddApprenticeTaskCommand request, CancellationToken cancellationToken)
        {
            await _progressApi.PostWithResponseCode<PostApprenticeTaskRequest>(new PostApprenticeTaskRequest(request.ApprenticeshipId, request.Data));
            return Unit.Value;
        }

    }
}