using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeProgress.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress
{
    public class GetKsbProgressForTaskQueryHandler : IRequestHandler<GetKsbProgressForTaskQuery, GetKsbProgressForTaskQueryResult>
    {
        private readonly IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> _progressApiClient;

        public GetKsbProgressForTaskQueryHandler(IApprenticeProgressApiClient<ApprenticeProgressApiConfiguration> progressApiClient)
        {
            _progressApiClient = progressApiClient;
        }

        public async Task<GetKsbProgressForTaskQueryResult> Handle(GetKsbProgressForTaskQuery request, CancellationToken cancellationToken)
        {
            var task = await _progressApiClient.Get<GetKsbProgressForTaskQueryResult>(new GetKsbProgressForTaskQueryRequest(request.ApprenticeshipId, request.TaskId));

            return new GetKsbProgressForTaskQueryResult
            {
                KSBProgress = task.KSBProgress
            };
        }
    }
}
