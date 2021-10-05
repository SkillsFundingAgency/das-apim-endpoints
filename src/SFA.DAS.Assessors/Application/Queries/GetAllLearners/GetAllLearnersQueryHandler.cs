using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Assessors.InnerApi.Requests;
using SFA.DAS.Assessors.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Assessors.Application.Queries.GetAllLearners
{
    public class GetAllLearnersQueryHandler : IRequestHandler<GetAllLearnersQuery, GetAllLearnersResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;

        public GetAllLearnersQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        {
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
        }


        public async Task<GetAllLearnersResult> Handle(GetAllLearnersQuery request, CancellationToken cancellationToken)
        {
            var result = await _commitmentsV2ApiClient.Get<GetAllLearnersResponse>(new GetAllLearnersRequest(request.SinceTime, request.BatchNumber, request.BatchSize ));

            return new GetAllLearnersResult(result);
        }
    }
}
