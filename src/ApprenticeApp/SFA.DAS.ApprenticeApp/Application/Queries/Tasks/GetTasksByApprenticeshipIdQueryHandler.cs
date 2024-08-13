using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetTasksByApprenticeshipIdQueryHandler : IRequestHandler<GetTasksByApprenticeshipIdQuery, GetTasksByApprenticeshipIdQueryResult>
    {
        private readonly IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> _progressApiClient;

        public GetTasksByApprenticeshipIdQueryHandler(
            IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> progressApiClient
            )
        {
            _progressApiClient = progressApiClient;
        }

        public async Task<GetTasksByApprenticeshipIdQueryResult> Handle(GetTasksByApprenticeshipIdQuery request, CancellationToken cancellationToken)
        {
            var apprenticeTasks = await _progressApiClient.Get<ApprenticeTasksCollection>(new GetApprenticeTasksRequest(request.ApprenticeshipId, request.Status, request.FromDate.GetValueOrDefault().ToString("MM-dd-yy"), request.ToDate.GetValueOrDefault().ToString("MM-dd-yy")));
        
            return new GetTasksByApprenticeshipIdQueryResult
            {
                Tasks = apprenticeTasks
            };
        }
    }
}