using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class GetInterviewAdjustmentsApiResponse
{
    public Guid CandidateId { get; set; }
    public string Support { get; set; }
}
