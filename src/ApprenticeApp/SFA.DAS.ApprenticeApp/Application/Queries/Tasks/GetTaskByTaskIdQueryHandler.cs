using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetTaskByTaskIdQueryHandler : IRequestHandler<GetTaskByTaskIdQuery, GetTaskByTaskIdQueryResult>
    {
        private readonly IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> _progressApiClient;

        public GetTaskByTaskIdQueryHandler(
            IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> progressApiClient
            )
        {
            _progressApiClient = progressApiClient;
        }

        public async Task<GetTaskByTaskIdQueryResult> Handle(GetTaskByTaskIdQuery request, CancellationToken cancellationToken)
        {
            var  task = await _progressApiClient.Get<ApprenticeTasksCollection>(new GetApprenticeTaskRequest(request.ApprenticeshipId, request.TaskId));
        
            return new GetTaskByTaskIdQueryResult
            {
                Tasks = task
            };
        }
    }
}