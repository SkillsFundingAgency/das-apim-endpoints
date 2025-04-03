using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Assessor;
public class GetEndpointAssessmentsResponse
{
    public DateTime? EarliestAssessment { get; set; }
    public int EndpointAssessmentCount { get; set; }
}