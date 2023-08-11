using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipPriorLearningData
{
    public class GetEditDraftApprenticeshipPriorLearningDataQueryHandler : IRequestHandler<GetEditDraftApprenticeshipPriorLearningDataQuery, GetEditDraftApprenticeshipPriorLearningDataQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public GetEditDraftApprenticeshipPriorLearningDataQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async  Task<GetEditDraftApprenticeshipPriorLearningDataQueryResult> Handle(GetEditDraftApprenticeshipPriorLearningDataQuery request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.GetWithResponseCode<GetDraftApprenticeshipResponse>(new GetDraftApprenticeshipRequest(request.CohortId, request.DraftApprenticeshipId));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            var reducedDuration = response.Body.DurationReducedBy;
            var isDurationReducedByRpl = response.Body.IsDurationReducedByRpl;

            if (isDurationReducedByRpl == null && reducedDuration != null)
            {
                isDurationReducedByRpl = true;
            }

            if (isDurationReducedByRpl == false && reducedDuration != null)
            {
                reducedDuration = null;
            }

            return new GetEditDraftApprenticeshipPriorLearningDataQueryResult
            {
                TrainingTotalHours = response.Body.TrainingTotalHours,
                DurationReducedByHours = response.Body.DurationReducedByHours,
                IsDurationReducedByRpl = isDurationReducedByRpl,
                DurationReducedBy = reducedDuration,
                PriceReduced = response.Body.PriceReducedBy
            };
        }
    }
}
