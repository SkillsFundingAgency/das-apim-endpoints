namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.AccessorService;

public sealed class GetAssessmentsResponse
{
    public DateTime? EarliestAssessment { get; }
    public int EndpointAssessmentCount { get; }

    public GetAssessmentsResponse(DateTime? earliestAssessment, int endpointAssessmentCount)
    {
        EarliestAssessment = earliestAssessment;
        EndpointAssessmentCount = endpointAssessmentCount;
    }
}
