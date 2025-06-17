using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Requests;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddPriorLearningData;

public class AddPriorLearningDataCommandHandler(
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
    ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
    ICourseTypesApiClient courseTypesApiClient,
    ILogger<AddPriorLearningDataCommandHandler> logger)
    : IRequestHandler<AddPriorLearningDataCommand, AddPriorLearningDataCommandResult>
{
    public async Task<AddPriorLearningDataCommandResult> Handle(AddPriorLearningDataCommand request, CancellationToken cancellationToken)
    {
        var apprenticeship = await apiClient.Get<GetDraftApprenticeshipResponse>(new GetDraftApprenticeshipRequest(request.CohortId, request.DraftApprenticeshipId));
            
        var standard = await coursesApiClient.Get<GetStandardsListItem>(new GetStandardDetailsByIdRequest(apprenticeship.CourseCode));
                
        if (standard == null)
        {
            logger.LogError("Standard not found for course ID {CourseId}", apprenticeship.CourseCode);
            throw new Exception($"Standard not found for course ID {apprenticeship.CourseCode}");
        }
                
        var priorLearningResponse = await courseTypesApiClient.Get<GetRecognitionOfPriorLearningResponse>(new GetRecognitionOfPriorLearningRequest(standard.ApprenticeshipType));

        if (priorLearningResponse == null)
        {
            logger.LogError("RPL rules not found for apprenticeship type {ApprenticeshipType}", standard.ApprenticeshipType);
            throw new Exception($"RPL rules not found for apprenticeship type {standard.ApprenticeshipType}");
        }
            
        var addPriorLearningRequest = new AddPriorLearningDataRequest
        {
            TrainingTotalHours = request.TrainingTotalHours,
            DurationReducedByHours = request.DurationReducedByHours,
            IsDurationReducedByRpl = request.IsDurationReducedByRpl,
            DurationReducedBy = request.DurationReducedBy,
            PriceReducedBy = request.PriceReducedBy,
            MinimumOffTheJobTrainingHoursRequired = priorLearningResponse.OffTheJobTrainingMinimumHours
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