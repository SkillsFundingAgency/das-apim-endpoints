using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class PutUpsertTrainingCourseApiResponse
{
    public Guid Id { get; set; }
    public Guid CandidateId { get; set; }
    public string? Provider { get; set; }
    public int? FromYear { get; set; }
    public int ToYear { get; set; }
    public Guid ApplicationId { get; set; }
    public string Title { get; set; }
}
