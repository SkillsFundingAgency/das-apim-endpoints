using SFA.DAS.SharedOuterApi.InnerApi.Responses.AccessorService;
using System;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

public sealed class EndpointAssessmentModel
{
    public DateTime? EarliestAssessment { get; set; }
    public int EndpointAssessmentCount { get; set; }

    public static implicit operator EndpointAssessmentModel(GetAssessmentsResponse source)
    {
        return new()
        {
            EarliestAssessment = source.EarliestAssessment,
            EndpointAssessmentCount = source.EndpointAssessmentCount
        };
    }
}
