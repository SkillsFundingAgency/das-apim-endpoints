using SFA.DAS.FindAnApprenticeship.Models;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class PutUpsertWorkHistoryApiResponse
{
    public Guid Id { get; set; }
    public WorkHistoryType WorkHistoryType { get; set; }
    public string? Employer { get; set; }
    public string? JobTitle { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid ApplicationId { get; set; }
    public string? Description { get; set; }
}
