using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetTaskCategoriesQueryHandler : IRequestHandler<GetTaskCategoriesQuery, GetTaskCategoriesQueryResult>
    {
        private readonly IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> _progressApiClient;

        public GetTaskCategoriesQueryHandler(
            IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> progressApiClient
            )
        {
            _progressApiClient = progressApiClient;
        }

        public async Task<GetTaskCategoriesQueryResult> Handle(GetTaskCategoriesQuery request, CancellationToken cancellationToken)
        {
            var  taskCategories = await _progressApiClient.Get<ApprenticeTaskCategoriesCollection>(new GetTaskCategoriesRequest(request.ApprenticeshipId));
        
            return new GetTaskCategoriesQueryResult
            {
                TaskCategories = taskCategories
            };
        }
    }
}