using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class PostCandidateAddressApiResponse
{
    public Guid Id { get; set; }
    public Guid CandidateId { get; set; }
}
