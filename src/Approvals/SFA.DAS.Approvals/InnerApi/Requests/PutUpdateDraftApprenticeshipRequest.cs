using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class PutUpdateDraftApprenticeshipRequest(
    long cohortId,
    long apprenticeshipId,
    UpdateDraftApprenticeshipRequest data)
    : IPutApiRequest
{
    public long CohortId { get; } = cohortId;
    public long ApprenticeshipId { get; } = apprenticeshipId;
    public object Data { get; set; } = data;

    public string PutUrl => $"api/cohorts/{CohortId}/draft-apprenticeships/{ApprenticeshipId}";
}