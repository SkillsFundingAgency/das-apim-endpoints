using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipPriorLearningSummary
{
    public class GetEditDraftApprenticeshipPriorLearningSummaryQueryHandler : IRequestHandler<GetEditDraftApprenticeshipPriorLearningSummaryQuery, GetEditDraftApprenticeshipPriorLearningSummaryQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public GetEditDraftApprenticeshipPriorLearningSummaryQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async  Task<GetEditDraftApprenticeshipPriorLearningSummaryQueryResult> Handle(GetEditDraftApprenticeshipPriorLearningSummaryQuery request, CancellationToken cancellationToken)
        {
            var apprenticeshipTask = _apiClient.Get<GetDraftApprenticeshipResponse>(new GetDraftApprenticeshipRequest(request.CohortId, request.DraftApprenticeshipId));
            var rplSummaryTask = _apiClient.Get<GetPriorLearningSummaryResponse>(new GetPriorLearningSummaryRequest(request.CohortId, request.DraftApprenticeshipId));
            await Task.WhenAll(apprenticeshipTask, rplSummaryTask);
            var apprenticeship = await apprenticeshipTask;
            var rplSummary = await rplSummaryTask;

            return new GetEditDraftApprenticeshipPriorLearningSummaryQueryResult
            {
                TrainingTotalHours = rplSummary?.TrainingTotalHours,
                DurationReducedByHours = rplSummary?.DurationReducedByHours,
                PriceReducedBy = rplSummary?.PriceReducedBy,
                FundingBandMaximum = rplSummary?.FundingBandMaximum,
                PercentageOfPriorLearning = rplSummary?.PercentageOfPriorLearning,
                MinimumPercentageReduction = rplSummary?.MinimumPercentageReduction,
                MinimumPriceReduction = rplSummary?.MinimumPriceReduction,
                RplPriceReductionError = rplSummary?.RplPriceReductionError,
                TotalCost = apprenticeship.Cost,
                LastName = apprenticeship.LastName,
                FirstName = apprenticeship.FirstName,
                HasStandardOptions = apprenticeship.HasStandardOptions
            };
        }
    }
}
