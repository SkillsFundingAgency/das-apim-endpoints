using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddPriorLearningData
{
    public class AddPriorLearningDataCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        : IRequestHandler<AddPriorLearningDataCommand, AddPriorLearningDataCommandResult>
    {
        public async Task<AddPriorLearningDataCommandResult> Handle(AddPriorLearningDataCommand request, CancellationToken cancellationToken)
        {
            var addPriorLearningRequest = new AddPriorLearningDataRequest
            {
                TrainingTotalHours = request.TrainingTotalHours,
                DurationReducedByHours = request.DurationReducedByHours,
                IsDurationReducedByRpl = request.IsDurationReducedByRpl,
                DurationReducedBy = request.DurationReducedBy,
                PriceReducedBy = request.PriceReducedBy
            };
            var result = await apiClient.PostWithResponseCode<AddPriorLearningDataResponse>(new PostAddPriorLearningDataRequest(request.CohortId, request.DraftApprenticeshipId, addPriorLearningRequest), false);

            result.EnsureSuccessStatusCode();

            var apprenticeship = apiClient.Get<GetDraftApprenticeshipResponse>(new GetDraftApprenticeshipRequest(request.CohortId, request.DraftApprenticeshipId));
            var priorLearningSummary = apiClient.Get<GetPriorLearningSummaryResponse>(new GetPriorLearningSummaryRequest(request.CohortId, request.DraftApprenticeshipId));
            await Task.WhenAll(apprenticeship, priorLearningSummary);

            return new AddPriorLearningDataCommandResult
            {
                HasStandardOptions = (await apprenticeship).HasStandardOptions,
                RplPriceReductionError = (await priorLearningSummary).RplPriceReductionError,
            };
        }
    }
}