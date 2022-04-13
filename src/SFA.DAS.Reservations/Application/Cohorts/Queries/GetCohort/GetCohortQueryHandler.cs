using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Reservations.Application.Providers.Queries.GetCohort;
using SFA.DAS.Reservations.InnerApi.Requests;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.Application.Cohorts.Queries.GetCohort
{
    public class GetCohortQueryHandler : IRequestHandler<GetCohortQuery, GetCohortResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;

        public GetCohortQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        {
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
        }

        public async Task<GetCohortResult> Handle(GetCohortQuery request, CancellationToken cancellationToken)
        {
            var cohort = await _commitmentsV2ApiClient.Get<GetCohortResponse>(new GetCohortRequest(request.CohortId));

            return new GetCohortResult
            {
                Cohort = cohort
            };
        }
    }
}