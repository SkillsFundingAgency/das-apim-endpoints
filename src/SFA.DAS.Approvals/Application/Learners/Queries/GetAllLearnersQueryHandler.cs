using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Learners.Queries
{
    public class GetAllLearnersQueryHandler : IRequestHandler<GetAllLearnersQuery, GetAllLearnersResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _apiClient;

        public GetAllLearnersQueryHandler(ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetAllLearnersResult> Handle(GetAllLearnersQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetAllLearnersResponse>(new GetAllLearnersRequest(request.SinceTime, request.BatchNumber, request.BatchSize ));

            return new GetAllLearnersResult(result.Learners, result.BatchNumber, result.BatchSize, result.TotalNumberOfBatches);
        }
    }
}
