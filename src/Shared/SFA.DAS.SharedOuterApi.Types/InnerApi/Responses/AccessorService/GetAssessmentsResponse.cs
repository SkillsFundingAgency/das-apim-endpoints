namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.AccessorService;

public sealed class GetAssessmentsResponse(DateTime? earliestAssessment, int endpointAssessmentCount)
{
    public DateTime? EarliestAssessment { get; } = earliestAssessment;
    public int EndpointAssessmentCount { get; } = endpointAssessmentCount;
}
