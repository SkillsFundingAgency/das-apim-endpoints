using System;

namespace SFA.DAS.Recruit.InnerApi.Responses;

public class GetCandidateApiResponse
{
    public Guid Id { get; set; }
    public string GovUkIdentifier { get; set; }
    public string FirstName { get; set; }
    public string MiddleNames { get; set; }
    public DateTime? DateOfBirth { get; set; }
}