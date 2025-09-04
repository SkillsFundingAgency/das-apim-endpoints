using System;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Responses;

public class GetApplicationByReferenceApiResponse
{
    public Guid Id { get; set; }
    public Guid CandidateId { get; set; }
}