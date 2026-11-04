using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.CommitmentsInner;

public class PutApprovalsApiRequest : IPutApiRequest<ApprovalsRequestBody>
{
    public string PutUrl { get; }

    public ApprovalsRequestBody Data { get; set; }

    public PutApprovalsApiRequest(Guid learningKey, ApprovalsRequestBody data)
    {
        PutUrl = $"approvals/{learningKey}";
        Data = data;
    }
}

public class ApprovalsRequestBody
{
    public Guid LearningKey { get; set; }
    public long ApprenticeshipId { get; set; }
    public string LearningType { get; set; } = string.Empty;
    public string Ukprn { get; set; } = string.Empty;
    public string Uln { get; set; } = string.Empty;
    public List<ApprovalsFieldChange> Changes { get; set; } = [];
    public List<ApprovalsPrice> NewPrices { get; set; } = [];
}

public class ApprovalsFieldChange
{
    public string ChangeType { get; set; } = string.Empty;
    public ApprovalsChangeData Data { get; set; } = new();
}

public class ApprovalsChangeData
{
    public string? Old { get; set; }
    public string? New { get; set; }
}

public class ApprovalsPrice
{
    public string TrainingPrice { get; set; } = string.Empty;
    public string AssessmentPrice { get; set; } = string.Empty;
    public DateTime EffectiveFrom { get; set; }
}

/// <summary>
/// Mirrors Approvals' CocChangeField: the change types Approvals accepts, sent by name as <see cref="ApprovalsFieldChange.ChangeType"/>.
/// Approvals publish no package for it, so it is copied here; only the names matter on the wire, and they must match theirs exactly.
/// </summary>
public enum CocChangeField
{
    TNP1,
    TNP2,
    Firstname,
    Lastname,
    StartDate,
    PlannedEndDate
}
