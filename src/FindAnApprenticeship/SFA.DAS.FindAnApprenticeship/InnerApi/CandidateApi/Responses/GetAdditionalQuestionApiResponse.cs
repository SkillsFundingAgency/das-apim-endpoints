using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class GetAdditionalQuestionApiResponse
{
    public string Question { get; set; }
    public string Answer { get; set; }
    public Guid Id { get; set; }
}
