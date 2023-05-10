using MediatR;
using Microsoft.Azure.Amqp.Framing;
using SFA.DAS.Approvals.Application.BulkUpload.Commands;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.NServiceBus;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddPriorLearningData
{
    public class AddPriorLearningDataCommandHandler : IRequestHandler<AddPriorLearningDataCommand, AddPriorLearningDataCommandResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        
        public AddPriorLearningDataCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<AddPriorLearningDataCommandResult> Handle(AddPriorLearningDataCommand request, CancellationToken cancellationToken)
        {
            var addPriorLearningRequest = new AddPriorLearningDataRequest
            {
                TrainingTotalHours = request.TrainingTotalHours,
                DurationReducedByHours = request.DurationReducedByHours,
                IsDurationReducedByRpl = request.IsDurationReducedByRpl,
                DurationReducedBy = request.DurationReducedBy,
                CostBeforeRpl = request.CostBeforeRpl,
                PriceReducedBy = request.PriceReducedBy
            };
            var result = await _apiClient.PostWithResponseCode<AddPriorLearningDataResponse>(new PostAddPriorLearningDataRequest(request.CohortId, request.DraftApprenticeshipId, addPriorLearningRequest), false);

            result.EnsureSuccessStatusCode();

            var apprenticeship = await _apiClient.Get<GetDraftApprenticeshipResponse>(new GetDraftApprenticeshipRequest(request.CohortId, request.DraftApprenticeshipId));
            var priorLearningSummary = await _apiClient.Get<GetPriorLearningSummaryResponse>(new GetPriorLearningSummaryRequest(request.CohortId, request.DraftApprenticeshipId));

            return new AddPriorLearningDataCommandResult
            {
                HasStandardOptions = apprenticeship.HasStandardOptions,
                RplPriceReductionError = priorLearningSummary.RplPriceReductionError,
            };
        }
    }
}