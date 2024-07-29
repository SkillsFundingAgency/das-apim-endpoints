using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts
{
    public class AddOrUpdateApprenticeTaskCommandHandler : IRequestHandler<AddOrUpdateApprenticeTaskCommand, Unit>
    {
        private readonly IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> _progressApi;

        public AddOrUpdateApprenticeTaskCommandHandler(IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> progressApi)
        {
            _progressApi = progressApi;
        }

        public async Task<Unit> Handle(AddOrUpdateApprenticeTaskCommand request, CancellationToken cancellationToken)
        {
            var apiResponse =
                await _progressApi.PostWithResponseCode<PostApprenticeTaskRequest>(new PostApprenticeTaskRequest(request.ApprenticeshipId, request.Data));

            return Unit.Value;
        }
  
    }
}
