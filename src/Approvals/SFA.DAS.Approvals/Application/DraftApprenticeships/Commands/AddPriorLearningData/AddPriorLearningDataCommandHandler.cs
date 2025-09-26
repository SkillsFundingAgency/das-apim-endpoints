using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddPriorLearningData;

public class AddPriorLearningDataCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
    ICourseTypeRulesService courseTypeRulesService)
    : IRequestHandler<AddPriorLearningDataCommand, AddPriorLearningDataCommandResult>
{
    public async Task<AddPriorLearningDataCommandResult> Handle(AddPriorLearningDataCommand request, CancellationToken cancellationToken)
    {
        var apprenticeship = await apiClient.Get<GetDraftApprenticeshipResponse>(new GetDraftApprenticeshipRequest(request.CohortId, request.DraftApprenticeshipId));
            
        var rplRules = await courseTypeRulesService.GetRplRulesAsync(apprenticeship.CourseCode);
            
        var addPriorLearningRequest = new AddPriorLearningDataRequest
        {
            TrainingTotalHours = request.TrainingTotalHours,
            DurationReducedByHours = request.DurationReducedByHours,
            IsDurationReducedByRpl = request.IsDurationReducedByRpl,
            DurationReducedBy = request.DurationReducedBy,
            PriceReducedBy = request.PriceReducedBy,
            MinimumOffTheJobTrainingHoursRequired = rplRules.RplRules.OffTheJobTrainingMinimumHours ?? 0
        };
        var result = await apiClient.PostWithResponseCode<AddPriorLearningDataResponse>(new PostAddPriorLearningDataRequest(request.CohortId, request.DraftApprenticeshipId, addPriorLearningRequest), false);

        result.EnsureSuccessStatusCode();

        var priorLearningSummary = await apiClient.Get<GetPriorLearningSummaryResponse>(new GetPriorLearningSummaryRequest(request.CohortId, request.DraftApprenticeshipId));

        return new AddPriorLearningDataCommandResult
        {
            HasStandardOptions = apprenticeship.HasStandardOptions,
            RplPriceReductionError = priorLearningSummary.RplPriceReductionError,
        };
    }
}