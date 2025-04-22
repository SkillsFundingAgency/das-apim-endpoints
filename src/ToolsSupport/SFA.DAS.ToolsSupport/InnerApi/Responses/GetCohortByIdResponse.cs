using SFA.DAS.SharedOuterApi.InnerApi.Interfaces;

namespace SFA.DAS.ToolsSupport.InnerApi.Responses;


// TODO remove unused fields
public class GetCohortByIdResponse 
{
    public long AccountId { get; set; }
    public TransferApprovalStatus? TransferApprovalStatus { get; set; }
    public bool IsLinkedToChangeOfPartyRequest { get; set; }
    public long? ChangeOfPartyRequestId { get; set; }
    public ApprenticeshipEmployerType LevyStatus { get; set; }
    public bool IsCompleteForProvider { get; set; }
    public bool IsCompleteForEmployer { get; set; }
    public bool IsApprovedByProvider { get; set; }
    public bool IsApprovedByEmployer { get; set; }
    public LastAction LastAction { get; set; }
    public string LatestMessageCreatedByProvider { get; set; }
    public Party WithParty { get; set; }
    public int? PledgeApplicationId { get; set; }
    public long? TransferSenderId { get; set; }
    public bool IsFundedByTransfer { get; }
    public long ProviderId { get; set; }
    public string ProviderName { get; set; }
    public string LegalEntityName { get; set; }
    public long AccountLegalEntityId { get; set; }
    public long CohortId { get; set; }
    public string CohortReference { get; set; }
    public string LatestMessageCreatedByEmployer { get; set; }
    public bool ApprenticeEmailIsRequired { get; set; }
}

public enum TransferApprovalStatus : byte
{
    Pending = 0,
    Approved = 1,
    Rejected = 2
}

public enum ApprenticeshipEmployerType : byte
{
    NonLevy = 0,
    Levy = 1
}

public enum LastAction : short
{
    None = 0,
    Amend = 1,
    Approve = 2,
    AmendAfterRejected = 3
}

[Flags]
public enum Party : short
{
    None = 0,
    Employer = 1,
    Provider = 2,
    TransferSender = 4
}